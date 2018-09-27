
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using System.Runtime.InteropServices;
using System.Reflection;

using ImageProcessingUtils;


namespace AutoSegment
{
    public partial class Form1 : Form
    {

        // constraints or defaults
        public static int NBOFFEATURES;                 // nb. of features for identifying our objects
        public static int FEATURES0ARRAYLENGTH;         // nb. of integer data in Features0 array
        public static int TEACHERSARRAYLENGTH;          // nb. of integer data in Teachers array
        public static Color PROCESSBACKCOLOR;           // color of Process button, if enabled
        public static Color SAVEBACKCOLOR;              // color of Save button, if enabled
        public static Color DISABLEDCOLOR;              // color of disabled buttons
        public static int DOWNSCALINGFACTOR;            // the downscaled bitmaps are diminished with 1/DOWNSCALINGFACTOR ratio
        public static int CROSS_SIZE;                   // size of cross in zoom window - for picking up object
        public static double SENSITIVITY;               // max. distance for accepting an object as "closest"
        public static int DISTANCEINIT;                 // illegal value for giving init. value to "distance"

        // current session's data
        public static string Title0;                    // application's title - without addon info

        // bitmaps - and attached data
        public static Bitmap Input_Bitmap;              // the original - loaded - input image
        public static Bitmap Grayscale_Bitmap;          // input - after grayscale conversion (8bpp, linear palette)
        public static Bitmap Preprocessed_Bitmap;       // preprocessed (thresholded, 8bpp) input (0: bobject, 255: background)
        public static Bitmap Teachers_Bitmap;           // bitmap with the filled teacher objects (24bpp RGB - only for visualization)
        public static Bitmap Result_Bitmap;             // result of pixel-by-pixel processing, all "other object" or background
                                                        // pixels are {0,0,0}, except the pixels of "our object" (they have 'objectColour')

        public static Bitmap Input_D_Bitmap;            // downscaled original input image
        public static Bitmap Preprocessed_D_Bitmap;     // downscaled preprocessed image
        public static Bitmap Teachers_D_Bitmap;         // downscaled bitmap with the teachers (24bpp RGB - only for visualization)
        public static Bitmap Result_D_Bitmap;           // downscaled version of Result_Bitmap

        public static Boolean bTeachersBitmapCreated;   // true, if the Teachers_Bitmap's (and Teachers_D_Bitmap's) content is relevant
        public static Boolean bInputBitmapLocked;       // true, if "somebody" is using exclusively the Input_Bitmap (or Input_D_Bitmap)
        public static Boolean bTeachersBitmapLocked;    // true, if "somebody" is using exclusively the Teachers_Bitmap (or Teachers_D_Bitmap)
        public static Boolean bResultBitmapLocked;      // true, if "somebody" is using exclusively the Result_Bitmap (or Result_D_Bitmap)

        // basic features for segmented spots
        public static int[] Features0 = { };            // Basic features for all spots, for speed up the teaching and identifying
        //                                                Format (for every serial number of spots - at most 65535 pieces):
        //                                                      [0] =0: OTHER spot  =1: OUR object (filled after processing)
        //                                                      [1] = count, number of pixel within spot
        //                                                      [2] = x-coordinate of left-top corner of container rectangle
        //                                                      [3] = y-coordinate of left-top corner of container rectangle
        //                                                      [4] = x-coordinate of right-bottom corner of container rectangle
        //                                                      [5] = y-coordinate of right-bottom corner of container rectangle
        public static int[] Segmented = { };            // segmented 16bpp image (using the thresholded image, all spots get own pixel value)

        // Teachers (spots bordered with closed contour), for telling the program, which of them are our objects, and which one are not
        public static double[] Teachers = { };          // list of segmented spots - with additional data and contour (vertex) positions
                                                        // Format:
                                                        //      [0]                                        number of teachers     (=N)
                                                        //      N pieces of consecutive (variable length)  blocks:
                                                        //      [0]                                        serial number of "name" identifying the object category
                                                        //      [1]                                        x-coordinate of "centre" of representing spot
                                                        //      [2]                                        y-coordinate of "centre" of representing spot
                                                        //      [3]...(NBOFFEATURES piexces of data)       features extracted for the given teacher
                                                        //                  presently:  - R, G, b color components
                                                        //                              - I: gray level (average of R,G,B components)
                                                        //                              - area (number of pixels inside the spot)
                                                        //                              - min. "diaameter" in pixels
                                                        //                              - max. "diameter" in pixels
                                                        //                              - aspect ratio (min_d/max_d)
                                                        //                              - circularity ((4*PI*area)/(perimeter*perimeter)
                                                        //      [4+NBOFFEATURES]        - number of vertex positions for clockwise contour description    (=N1)
                                                        //      N1 pieces of pairs: x and y coordinates of vertex position (in image's coordinate system)
                                                        //      Remark: - the contour is closed - it is asumed, that the first and last vertex positions are adjacent
                                                        //              - between the vertex positions straight line segments are assumed, if they are not adjacent
                                                        // REM: the above teacher data array is dependent on the preprocessing steps and parameters (type of thresholding, its parameters0,
                                                        //      so they are applyed only, if these parameters are the same as were set at teaching. The teacher data file contains these parameters, and the program
                                                        //      checks at launching the neural network, if the teached and currently set conditions are the same.
        public static Boolean bTeacherEdit;             // if true, "Teachers" page of tabControl is invoked and the teachers are (can be) adjusted
        public Boolean bUsedFeatures_Setting;           // set to true, in order to avoid cyclic state retrieval
        public static int NumberOfTeachers;             // number of teachers - defined on the currently loaded image
        public static int TeacherIndex;                 // index of selected teacher

        // Neural network (NN)
        public int nbOfUnits_1;                         // nb. of Units in INPUT layer ( = number of wired features)
        public int nbOfUnits_2;                         // nb. of units in HIDDEN layer ( user parameter )
        public int nbOfUnits_3;                         // nb. of units in OUTPUT layer ( preswently: 2, 'our object' and 'not our object')
        public static double[] Weights = { };           // weights for neural network with the above unit-numbers
        public static bool bTeached;                    // true, if the NN has been teached with the current teachers and unit-settings

        // Zoom
        public static ZoomWindow zoomwindow;            // zoom window
        public static Point pos0;                       // cursor position (before conversion to image's coord. system)
        public static Point pos;                        // cursor position (after conversion: in image's coord. system)
        int halfWidth, halfHeight;                      // half size of zoomwindow - in image's system
        public static Boolean bZoomPanTimerStarted;     // true, if refreshing of zoom window is started

        // ZoomThr
        public static ZoomWindow zoomwindowThr;         // zoom (thresholded) window

        // misc
        public static ToolTip tt1;                      // tooltip
        public static int filterindex;                  // type selection for File Browser dialogue
        Point pos1;                                     // starting position of zoom window
        bool bPreProcessed;                             // true, if the input is preprocessed (thresholded)
        bool bProcessed;                                // true, if the thresholded image is processed (using NN)
        public static ContextMenu mnuContextMenu1;      // context menu (with "Add Teacher")
        public static ContextMenu mnuContextMenu2;      // context menu (with "Remove Teacher")
        public static bool bContextMenuInvoked;         // true., if context menu is invoked

        //---------------------------------------------------------------------------
        // ### general events ###

        public Form1()
        {
            InitializeComponent();

            // We need handler for MouseWheel
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);

            // Set constraints
            NBOFFEATURES = 9;                       // in the first version:    - R,G,B components (averaged in the segmented spot)
            //                                                                  - I (gray level: average of r,g,b components)
            //                                                                  - area (number of pixels inside spot)
            //                                                                  - min. "diameter" in pixels
            //                                                                  - max. "diameter" in pixels
            //                                                                  - aspect ratio (shape factor: ratio, where the largest diameter
            //                                                                    is the denominator, and the nominator is the ortogonal one,
            //                                                                  - circularity (ratio of area and square of perimeter)
            FEATURES0ARRAYLENGTH = 6 * 65536;           // the pixel format of Segmented_Bitmap is 16bpp, that is: the highest serial number for a spot
            //                                             can be 65536. Every spot is represented by 6 basic features here - others are computed later on.
            TEACHERSARRAYLENGTH = 10000000;             // large enough for realistic scene
            PROCESSBACKCOLOR = Color.LightGreen;
            SAVEBACKCOLOR = Color.LightBlue;
            DISABLEDCOLOR = Color.LightGray;
            DOWNSCALINGFACTOR = 4;                  // default - will be adjusted when loading a new image
            CROSS_SIZE = 20;
            SENSITIVITY = 100;
            DISTANCEINIT = 100000;

            // Load stored/default parameters (General settings)
            SettingsFile sf = new SettingsFile();
            button_ObjectColor.BackColor = sf.GetObjectColor();
            button_Zoom_Color.BackColor = sf.GetZoomedFieldColor();
            checkBox_ShowFilledFields.Checked = sf.GetShowFilledFields();
            filterindex = sf.GetFilterIndex();
            trackBar_Blending_Fields.Value = sf.GetBlendingFieldsRate();
            trackBar_Blending_Result.Value = sf.GetBlendingResultRate();
            checkBox_KeepPreProcessing.Checked = sf.GetKeepPreProcessing();
            checkBox_KeepProcessing.Checked = sf.GetKeepProcessing();
            bool bShowZoom = sf.GetShowZoom();
            bool bShowZoomThr = sf.GetShowZoomThr();
            checkBox_Show_Thresholded.Checked = sf.GetShowThresholded();

            // Load stored/default parameters (Preprocessing and teaching-related)
            TeacherFile tf = new TeacherFile();
            if (tf.GetOtsuThresholdingSelected())
                radioButton_Otsu_thresholding.Checked = true;
            else
                radioButton_Manual_thresholding.Checked = true;
            numericUpDown_Otsu.Value = tf.GetOtsuThresholdValue();
            numericUpDown_threshold.Value = tf.GetManualThresholdValue();
            button_Teacher_Our_Color.BackColor = tf.GetTeacherOurColor();
            button_Teacher_Other_Color.BackColor = tf.GetTeacherOtherColor();

            // Bitmaps
            Input_Bitmap = null;
            Input_D_Bitmap = null;
            Grayscale_Bitmap = null;
            Preprocessed_Bitmap = null;
            Preprocessed_D_Bitmap = null;
            Teachers_Bitmap = null;
            Teachers_D_Bitmap = null;
            Result_Bitmap = null;
            Result_D_Bitmap = null;
            bTeachersBitmapCreated = false;
            bInputBitmapLocked = false;
            bTeachersBitmapLocked = false;
            bResultBitmapLocked = false;
            ShowImageSize();

            // Features0
            ClearFeatures0();
            ClearSegmented();

            // Teachers
            bUsedFeatures_Setting = false;
            ClearTeachers();
            bTeached = false;
            bTeacherEdit = false;
            ShowTeacherNumber();
            GetUsedFeatureControls();
            FillListView_TeacherNames(false);
            checkBox_AddTeacher.Enabled = false;    // not allowed, till image is not loaded
            button_RenameSelected.Enabled = false;

            // Neural network
            // Load the weights (if already teached)
            WeightFile wf = new WeightFile();
            nbOfUnits_1 = nbOfUnits_2 = nbOfUnits_3 = 0;
            nbOfUnits_1 = wf.LoadNbOfUnits_1();
            if (nbOfUnits_1 == 0)
                nbOfUnits_1 = NBOFFEATURES;
            nbOfUnits_2 = wf.LoadNbOfUnits_2();
            nbOfUnits_3 = wf.LoadNbOfUnits_3();

            // check consistency
            int NumberOfNames = tf.GetNumberOfNames();
            if (nbOfUnits_3 != NumberOfNames)
            {
                nbOfUnits_3 = NumberOfNames;
                nbOfUnits_2 = nbOfUnits_3;
                SetupWeightArray();
                CImageProcessingUtils.InitNeuralNetwork(Weights,
                     nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
                wf.SaveSettings(nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
                wf.SaveWeights(Weights, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
            }

            SetupWeightArray();
            double confidence = wf.GetConfidence();
            numericUpDown_Confidence.Value = Convert.ToDecimal(confidence);
            bTeached = wf.LoadWeights(Weights, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
            textBox_inputunits.Text = Convert.ToString(nbOfUnits_1);
            numericUpDownNbofHiddenUnits.Value = Convert.ToDecimal(nbOfUnits_2);
            textBox_outputunits.Text = Convert.ToString(nbOfUnits_3);

            // Misc.
            tt1 = new ToolTip();
            Form1.CheckForIllegalCrossThreadCalls = false;
            EnableButtons(false);
            bPreProcessed = false;
            bProcessed = false;
            bContextMenuInvoked = false;
            mnuContextMenu1 = null;
            mnuContextMenu2 = null;

            // Zoom
            pos0 = new Point(-1, -1);
            pos = new Point(-1, -1);
            halfWidth = halfHeight = 0;
            ShowPosition(-1, -1);
            timer_zoom_pan.Interval = 100;      //  refresh of zoom viewr after zoom/pan is: 100 msec
            bZoomPanTimerStarted = false;

            // capture spec. characters, for fine adjusting the cursor's position
            this.KeyPreview = true;
            this.KeyPress +=
                new KeyPressEventHandler(Form1_KeyPress);
            textBox_ImageFileName.KeyPress +=
                new KeyPressEventHandler(textBox_ImageFileName_KeyPress);

            // Append the 'Version' to App's window title
            Assembly MyAsm = Assembly.Load("AutoSegment");
            AssemblyName aName = MyAsm.GetName();
            Version ver = aName.Version;
            this.Text += "   [Version: " + ver + " ]";
            Title0 = this.Text; // for later use...
            this.CenterToScreen();

            // Show Zoom window
            zoomwindow = new ZoomWindow();      // create and invoke the Zoom Window
            zoomwindow.Owner = this;
            zoomwindow.Text = " Input image (1:1)";
            if (bShowZoom)
            {
                checkBox_ShowZoom.Checked = true;
                zoomwindow.Show();
            }
            pos1.X = 10;                        // default pos.: upper-left corner of screen
            pos1.Y = 10;
            zoomwindow.Location = pos1;

            // Show ZoomThr window
            zoomwindowThr = new ZoomWindow();      // create and invoke the Zoom (thresholded) Window
            zoomwindowThr.Owner = this;
            zoomwindowThr.Text = " Thresholded input image (1:1)";
            if (bShowZoomThr)
            {
                checkBox_ShowZoomThr.Checked = true;
                zoomwindowThr.Show();
            }

            pos1.X = 10;                        // default pos.: upper-left corner of screen
            pos1.Y = 300;
            zoomwindowThr.Location = pos1;

            zoomwindow.zoomwnd2 = zoomwindowThr;
            zoomwindowThr.zoomwnd2 = zoomwindow;

        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            return; // Presently: fine adjustment of position by key pressing is not allowed, pressing the SHIFT key while moving is implemented instead

            /*
            if (pos.X == -1)
                return; // no Input_Bitmap, or the cursor is outside of input image

            // decide the step of cursor position in image's coordinate system
            int step = 1;
            switch (e.KeyChar)
            {
                // West-North
                case (char)55:      // '7'
                    pos.X -= step;
                    pos.Y -= step;
                    break;
                // North
                case (char)56:      // '8'
                    pos.Y -= step;
                    break;
                // East-North
                case (char)57:      // '9'
                    pos.X += step;
                    pos.Y -= step;
                    break;
                // East
                case (char)54:      // '6'
                    pos.X += step;
                    break;
                // East-South
                case (char)51:      // '3'
                    pos.X += step;
                    pos.Y += step;
                    break;
                // South
                case (char)50:      // '2'
                    pos.Y += step;
                    break;
                // West-South
                case (char)49:      // '1'
                    pos.X -= step;
                    pos.Y += step;
                    break;
                // West
                case (char)52:      // '4'
                    pos.X -= step;
                    break;
                default:            // (nothing to do)
                    return;
            }
            e.Handled = true;       // (handled)

            // refresh the zoom window
            ShowPosition(pos.X, pos.Y);
            ShowZoomedArea();
            ShowZoomedThrArea();

            // show the border of zoomed area (in 'overlay' color)
            if (!bZoomPanTimerStarted)
            {
                bZoomPanTimerStarted = true;
                timer_zoom_pan.Start();
            }
            */
        }

        void textBox_ImageFileName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (pos.X != -1)
            {
                e.Handled = true;
                return; // the cursor is over Input_Image - used for fine adjustment of cursor position
            }

        }

        //--------------------------------------------------------------------------------------

        // get user-given image file name
        private void textBox_ImageFileName_TextChanged(object sender, EventArgs e)
        {
            bPreProcessed = false;
            bProcessed = false;
            bTeached = false;

            // get rid of all previous data
            bool bAcceptable = false;
            bTeachersBitmapCreated = false;
            string fullfilename = textBox_ImageFileName.Text;
            textBox_ExecutionTime.Text = "";
            TeacherIndex = -1;

            // get rid of previous visualizations
            if (pictureBox_Image.Image != null)
            {
                pictureBox_Image.Image.Dispose();
                pictureBox_Image.Image = null;
            }

            Cursor = Cursors.WaitCursor;
            bAcceptable = LoadImageFile(fullfilename);  // load - and create bitmaps
            Cursor = Cursors.Default;

            textBox_ImageFileName.ForeColor = bAcceptable ? Color.Black : Color.Red;   // black, if accepted

            if (bAcceptable)
            {
                if (checkBox_KeepPreProcessing.Checked || checkBox_AddTeacher.Checked)
                    PreProcess();

                if (checkBox_KeepProcessing.Checked)
                    Process();
                else
                    ShowBitmap();

                // load all teacher data
                TeacherFile tf = new TeacherFile();
                tf.SetFeatureNumber(NBOFFEATURES);
                string filename = textBox_ImageFileName.Text;
                NumberOfTeachers = tf.GetNumberOfTeachers(filename);
                ClearTeachers();
                tf.GetTeachers(filename, Teachers);
                TeacherIndex = -1;
                ShowTeacherNumber();
                checkBox_AddTeacher.Enabled = true;

                RefreshNamesListView();

            }
            ShowImageSize();
            EnableButtons(bAcceptable);

        }

        public void RefreshNamesListView()
        {

            if (radioButton_CurrentTeachers.Checked)
                FillListView_TeacherNames(false);
            else
                FillListView_TeacherNames(true);

        }

        // invoke the file browser for selecting image file name
        private void button_BrowseImage_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter =
                "jpeg, png, bmp or tiff images|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff|jpeg images (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            dlg.Filter += "|png images (*.png)|*.png|bitmap images (*.bmp)|*.bmp|tiff images (*.tif, *.tiff)|*.tif;*.tiff|All files (*.*)|*.*";
            dlg.FilterIndex = filterindex;

            dlg.FileName = textBox_ImageFileName.Text; // start with prev. image file and filterindex
            if (dlg.ShowDialog() != DialogResult.OK)
                return;             // cancelled by the user

            filterindex = dlg.FilterIndex;
            SettingsFile sf = new SettingsFile();
            sf.SetFilterIndex(filterindex);

            textBox_ImageFileName.Text = dlg.FileName; // (REM: textBox's handler will process it)

        }

        //------------------------------------------------------------------------------------------------------




        // quit the program
        private void button_Exit_Click(object sender, EventArgs e)
        {
            timer_zoom_pan.Stop();
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        // invoke the file save browser, for saving the visualized image and overlay
        private void button_Save_Click(object sender, EventArgs e)
        {
            if (Input_Bitmap == null)
                return;

            // choose name and format for saving
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter =
                "bitmap images (*.bmp)|*.bmp|jpeg images (*.jpg,*.jpeg)|*.jpg;*.jpeg|png images (*.png)|*.png|All files (*.*)|*.*";

            // the default format is ".png"
            dlg.FilterIndex = 3;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;             // cancelled by user

            while (bInputBitmapLocked || bTeachersBitmapLocked)
            {
                // do nothing (other thread will unlock the locked bitmap)
            }

            // 24bpp bitmap needed for getting graphics object
            Bitmap Output_Bitmap;

            if (Input_Bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                Output_Bitmap = (Bitmap)Input_Bitmap.Clone();
            else
            {
                Output_Bitmap = new Bitmap(Input_Bitmap.Width, Input_Bitmap.Height, PixelFormat.Format24bppRgb);
                ImageProcessingUtils.CImageProcessingUtils.ConvertToRGB_24bpp(Input_Bitmap, Output_Bitmap);
            }


            using (Graphics g = Graphics.FromImage(Output_Bitmap))
            {
                PaintAddOnsOverImage(g, Output_Bitmap.Width, Output_Bitmap.Height); // paint add-ons as blended overlay
            }

            // Save the proper bitmap (REM: format is selected by filename-extension)
            Output_Bitmap.Save(dlg.FileName);
            Output_Bitmap.Dispose();

        }

        // paint event of picturebox
        private void pictureBox_Image_Paint(object sender, PaintEventArgs e)
        {
            // for painting additional object over the visualized bitmap (overlay)
            PaintAddOnsOverImage(e.Graphics, pictureBox_Image.Width, pictureBox_Image.Height);
        }

        //---------------------------------------------------------------------------
        // ## misc functions ##

        // paint the add-ons (overlay objects) over visualized image
        void PaintAddOnsOverImage(Graphics g, int output_width, int output_height)
        {
            if (Input_Bitmap == null || bInputBitmapLocked)
                return;
            bInputBitmapLocked = true;

            // 1. Show overlay of teacher's spots
            if (Teachers_Bitmap != null && !bTeachersBitmapLocked)
            {
                bTeachersBitmapLocked = true;
                ShowFieldsOverlay(g, output_width, output_height);
                bTeachersBitmapLocked = false;
            }

            // 2. Show overlay of Results (PLANT pixels)
            if (bProcessed && Result_Bitmap != null && !bResultBitmapLocked)
            {
                bResultBitmapLocked = true;
                ShowResultOverlay(g, output_width, output_height);
                bResultBitmapLocked = false;
            }

            // 3. Show the border of zoomed area (in 'overlay' color)
            ShowZoomWindowAsOverlay(g);

            bInputBitmapLocked = false;
        }

        // show teacher objects as overlayed spots
        public void ShowFieldsOverlay(Graphics g, int output_width, int output_height)
        {
            if (Teachers[0] == 0)
                return;

            Color OurTeacherColor = button_Teacher_Our_Color.BackColor;
            SolidBrush OurBrush = new SolidBrush(Color.FromArgb(trackBar_Blending_Fields.Value,
                    OurTeacherColor.R, OurTeacherColor.G, OurTeacherColor.B));
            Color OtherTeacherColor = button_Teacher_Other_Color.BackColor;
            SolidBrush OtherBrush = new SolidBrush(Color.FromArgb(trackBar_Blending_Fields.Value,
                    OtherTeacherColor.R, OtherTeacherColor.G, OtherTeacherColor.B));
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
            int NbofTeachers = (int)Teachers[0];
            if (checkBox_ShowFilledFields.Checked)
            {
                if (!bTeachersBitmapCreated)
                {
                    CreateTeachersBitmap();
                    ImageProcessingUtils.CImageProcessingUtils.DownscaleBitmap(Teachers_Bitmap, DOWNSCALINGFACTOR, Teachers_D_Bitmap);
                }

                // Blend the Mask to the visualized content
                float Alpha = (float)trackBar_Blending_Fields.Value / 255.0f;
                Size psize = new Size(output_width, output_height);
                Rectangle rect1 = new Rectangle(Point.Empty, psize);
                double ratio1 = (double)psize.Width / (double)Teachers_D_Bitmap.Width;
                int pheight = (int)((double)Teachers_D_Bitmap.Height * ratio1 + 0.5);
                if (pheight <= psize.Height)
                {
                    // fit-to-width
                    rect1.X = 0;
                    rect1.Y = (psize.Height - pheight) / 2;
                    rect1.Width = psize.Width;
                    rect1.Height = pheight;
                }
                else
                {
                    // fit-to-height
                    ratio1 = (double)psize.Height / (double)Teachers_D_Bitmap.Height;
                    int pwidth = (int)((double)Teachers_D_Bitmap.Width * ratio1 + 0.5);
                    rect1.X = (psize.Width - pwidth) / 2;
                    rect1.Y = 0;
                    rect1.Width = pwidth;
                    rect1.Height = psize.Height;
                }

                ImageAttributes attr = new ImageAttributes();
                float[][] ptsArray ={
                  new float[] {1, 0, 0, 0, 0},
                  new float[] {0, 1, 0, 0, 0},
                  new float[] {0, 0, 1, 0, 0},
                  new float[] {0, 0, 0, Alpha, 0},
                  new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                attr.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(Teachers_D_Bitmap, rect1,
                    0, 0, Teachers_D_Bitmap.Width, Teachers_D_Bitmap.Height, GraphicsUnit.Pixel, attr);

            }
            else
            {
                int index = 1;  // index of the first teacher
                TeacherFile tf = new TeacherFile();
                for (int ii = 0; ii < NbofTeachers; ii++)
                {
                    int name_sn = (int)Teachers[index];
                    string type = tf.GetType(name_sn - 1);
                    bool bOurObject = type == "OUR" ? true : false;
                    if ( bOurObject)
                        DrawPolygon(Teachers, index, Input_Bitmap.Width, Input_Bitmap.Height,
                                    output_width, output_height, OurBrush, true, g);
                    else
                        DrawPolygon(Teachers, index, Input_Bitmap.Width, Input_Bitmap.Height,
                                    output_width, output_height, OtherBrush, true, g);
                    int length = 3 + NBOFFEATURES;
                    length += 2 * (int)Teachers[index + length]+1;
                    index += length;
                }

            }
            OurBrush.Dispose();
            OtherBrush.Dispose();
        }

        // Show the borders of fields over the zoomed image
        void ShowFieldsZoomOverlay(Graphics g)
        {
            if (Teachers[0] == 0)
                return;

            int zoomWidth = zoomwindow.pictureBox_Zoom.Width;
            int zoomHeight = zoomwindow.pictureBox_Zoom.Height;
            halfWidth = zoomWidth / 2;
            halfHeight = zoomHeight / 2;

            Color OurTeacherColor = button_Teacher_Our_Color.BackColor;
            SolidBrush OurBrush = new SolidBrush(Color.FromArgb(trackBar_Blending_Fields.Value,
                    OurTeacherColor.R, OurTeacherColor.G, OurTeacherColor.B));
            Color OtherTeacherColor = button_Teacher_Other_Color.BackColor;
            SolidBrush OtherBrush = new SolidBrush(Color.FromArgb(trackBar_Blending_Fields.Value,
                    OtherTeacherColor.R, OtherTeacherColor.G, OtherTeacherColor.B));
            int NbofTeachers = (int)Teachers[0];
            int index = 1;


            for (int ii = 0; ii < NbofTeachers; ii++)
            {
                Point[] mask2 = { };
                int index2 = index + 3 + NBOFFEATURES;
                int length = (int)Teachers[index2];
                Array.Resize(ref mask2, length);
                for (int jj = 0; jj < length; jj++)
                {
                    mask2[jj].X = -pos.X + halfWidth + (int)Teachers[index2 + 2 * jj + 1];
                    mask2[jj].Y = -pos.Y + halfHeight + (int)Teachers[index2 + 2 * jj + 2];
                }
                if (length >= 2 && Teachers[index] != 0)
                {
                    switch ((int)Teachers[index])
                    {
                        case 0:
                            // undefined spot - nothing to do
                            break;
                        case 1:
                            Pen PenO = new Pen(OurBrush, 5);
                            g.DrawPolygon(PenO, mask2);
                            PenO.Dispose();
                            break;
                        case 2:
                            Pen PenNO = new Pen(OtherBrush, 5);
                            g.DrawPolygon(PenNO, mask2);
                            PenNO.Dispose();
                            break;
                    }

                }

                int length2 = 3 + NBOFFEATURES;
                length2 += 2 * (int)Teachers[index + length2] + 1;
                index += length2;
            }

            OurBrush.Dispose();
            OtherBrush.Dispose();

        }

        // create bitmaps with filled spots representing the teachers
        void CreateTeachersBitmap()
        {
            Color OurTeacherColor = button_Teacher_Our_Color.BackColor;
            Color OtherTeacherColor = button_Teacher_Other_Color.BackColor;
            using (Graphics g = Graphics.FromImage(Teachers_Bitmap))
            {
                SolidBrush OurBrush = new SolidBrush(Color.FromArgb(255, OurTeacherColor.R, OurTeacherColor.G, OurTeacherColor.B));
                SolidBrush OtherBrush = new SolidBrush(Color.FromArgb(255, OtherTeacherColor.R, OtherTeacherColor.G, OtherTeacherColor.B));
                SolidBrush BkgBrush = new SolidBrush(Color.FromArgb(255, 127, 127, 127));
                int NbofTeachers = (int)Teachers[0];

                // 1. clear the whole mask
                Point[] Corners = { };
                Array.Resize(ref Corners, 4);
                Corners[0].X = 0; Corners[0].Y = 0;
                Corners[1].X = Teachers_Bitmap.Width - 1; Corners[1].Y = 0;
                Corners[2].X = Teachers_Bitmap.Width - 1; Corners[2].Y = Teachers_Bitmap.Height - 1;
                Corners[3].X = 0; Corners[3].Y = Teachers_Bitmap.Height - 1;
                g.FillPolygon(BkgBrush, Corners);

                // 2. draw the fields
                int index = 1;  // index of the first teacher's spot
                TeacherFile tf = new TeacherFile();
                for (int ii = 0; ii < NbofTeachers; ii++)
                {
                    int name_sn = (int)Teachers[index];
                    string type = tf.GetType(name_sn - 1);
                    bool bOurObject = type == "OUR" ? true : false;
                    if (bOurObject)
                        FillPolygon(Teachers, index, Teachers_Bitmap.Width, Teachers_Bitmap.Height, OurBrush, g);
                    else
                        FillPolygon(Teachers, index, Teachers_Bitmap.Width, Teachers_Bitmap.Height, OtherBrush, g);
                    int length = 3 + NBOFFEATURES;
                    length += 2 * (int)Teachers[index + length] + 1;
                    index += length;
                }

                OurBrush.Dispose();
                OtherBrush.Dispose();
                BkgBrush.Dispose();
            }

            bTeachersBitmapCreated = true;

        }

        // draw polygon
        void DrawPolygon(double[] mask, int index, int bmp_width, int bmp_height, int pb_width, int pb_height,
            Brush brush, bool bClosed, Graphics g)
        {
            // transform the positions from image-relative coordinate system to
            // the coordinate system of visualized image
            double ratio_image = (double)bmp_width / (double)bmp_height;
            double ratio_PictureBox = (double)pb_width / (double)pb_height;
            double ratio;
            double shiftx, shifty;
            if (ratio_image > ratio_PictureBox)
            {
                // the pictureBox's height is bigger than image's one
                ratio = (double)pb_width / (double)bmp_width;
                shiftx = 0;
                shifty = (double)(pb_height - ratio * (double)bmp_height) / 2.0;
                ratio *= 0.995; // ???
            }
            else
            {
                // the pictureBox's width is bigger than image's one
                ratio = (double)pb_height / (double)bmp_height;
                shiftx = (double)(pb_width - ratio * (double)bmp_width) / 2.0;
                ratio *= 0.995; // ???
                shifty = 0;
            }

            // transform the vertex positions
            Point[] mask2 = { };
            int index2 = index + 3 + NBOFFEATURES;
            int length = (int)mask[index2];
            Array.Resize(ref mask2, length);
            for (int ii = 0; ii < length; ii++)
            {
                mask2[ii].X = (int)(shiftx + ratio * mask[index2 + 2 * ii + 1] + 0.5);
                mask2[ii].Y = (int)(shifty + ratio * mask[index2 + 2 * ii + 2] + 0.5);
            }

            // paint
            if (length >= 2)
            {
                Pen Pen = new Pen(brush, 3);
                if (bClosed)
                {
                    g.DrawPolygon(Pen, mask2);
                }
                else
                {
                    for (int ii = 0; ii < (length - 1); ii++)
                        g.DrawLine(Pen, mask2[ii], mask2[ii + 1]);
                }
                Pen.Dispose();

            }

        }

        // draw filled polygon
        void FillPolygon(double[] mask, int index, int bmp_width, int bmp_height, Brush brush, Graphics g)
        {

            // transform the vertex positions
            Point[] mask2 = { };
            int index2 = index + 3 + NBOFFEATURES;
            int length = (int)mask[index2];
            Array.Resize(ref mask2, length);
            for (int ii = 0; ii < length; ii++)
            {
                mask2[ii].X = (int)mask[index2 + 2 * ii + 1];
                mask2[ii].Y = (int)mask[index2 + 2 * ii + 2];
            }

            // paint
            if (length >= 3)
                g.FillPolygon(brush, mask2);

        }


        // load image file
        Boolean LoadImageFile(String fullfilename)
        {
            while (bInputBitmapLocked || bTeachersBitmapLocked)
            {
                // do nothing (other thread will unlock the locked bitmap)
            }
            bInputBitmapLocked = bTeachersBitmapLocked = bResultBitmapLocked = true;

            ClearBitmaps(); // destroy all bitmaps

            Boolean bAcceptable = false;

            try
            {
                Input_Bitmap = new Bitmap(fullfilename); // exception, if unsuccessful
                if (Input_Bitmap.PixelFormat != PixelFormat.Format1bppIndexed &&
                    Input_Bitmap.PixelFormat != PixelFormat.Format8bppIndexed &&
                    Input_Bitmap.PixelFormat != PixelFormat.Format16bppGrayScale &&
                    Input_Bitmap.PixelFormat != PixelFormat.Format24bppRgb &&
                    Input_Bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    MessageBox.Show("Input Image's format is not acceptable.\n (Pixelformat must be 1bpp, 8bpp, 16bpp, 24bpp or 32bpp.)");
                    Input_Bitmap.Dispose();
                    Input_Bitmap = null;
                    bAcceptable = false; // not accepted
                }
                else bAcceptable = true;
                bAcceptable = true;
            }
            catch (Exception x)
            {
                // no matter what is the problem - do not accept it
                bAcceptable = false;
            }

            if (bAcceptable)
            {
                // setup the DOWNSCALINGFACTOR - assuming, taking the screen resolution into consideration
                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                DOWNSCALINGFACTOR = Math.Max(Input_Bitmap.Width / resolution.Width, Input_Bitmap.Height / resolution.Height);
                DOWNSCALINGFACTOR = Math.Max(DOWNSCALINGFACTOR, 1);

                // create all the necessary bitmaps and arrays
                Grayscale_Bitmap = CreateGrayScaleBitmap(Input_Bitmap.Width, Input_Bitmap.Height);
                Preprocessed_Bitmap = CreateGrayScaleBitmap(Input_Bitmap.Width, Input_Bitmap.Height);
                Teachers_Bitmap = new Bitmap(Input_Bitmap.Width, Input_Bitmap.Height, PixelFormat.Format24bppRgb);
                ClearSegmented();

                int down_Width = Input_Bitmap.Width / DOWNSCALINGFACTOR;     // dimensions of downscaled bitmaps
                int down_Height = Input_Bitmap.Height / DOWNSCALINGFACTOR;
                Input_D_Bitmap = Input_Bitmap.PixelFormat == PixelFormat.Format8bppIndexed ?
                    CreateGrayScaleBitmap(down_Width, down_Height) : new Bitmap(down_Width, down_Height, Input_Bitmap.PixelFormat);
                Preprocessed_D_Bitmap = CreateGrayScaleBitmap(down_Width, down_Height);
                Teachers_D_Bitmap = new Bitmap(down_Width, down_Height, PixelFormat.Format24bppRgb);
                ImageProcessingUtils.CImageProcessingUtils.DownscaleBitmap(Input_Bitmap, DOWNSCALINGFACTOR, Input_D_Bitmap);
                Result_Bitmap = new Bitmap(Input_Bitmap.Width, Input_Bitmap.Height, PixelFormat.Format24bppRgb);
                Result_D_Bitmap = new Bitmap(down_Width, down_Height, PixelFormat.Format24bppRgb);

            }
            bInputBitmapLocked = bTeachersBitmapLocked = bResultBitmapLocked = false;

            return bAcceptable;
        }

        // Delete all necessary bitmaps
        void ClearBitmaps()
        {
            // get rid of original size images
            if (Input_Bitmap != null)
            {
                Input_Bitmap.Dispose(); Input_Bitmap = null;
            }
            if (Grayscale_Bitmap != null)
            {
                Grayscale_Bitmap.Dispose(); Grayscale_Bitmap = null;
            }
            if (Preprocessed_Bitmap != null)
            {
                Preprocessed_Bitmap.Dispose(); Preprocessed_Bitmap = null;
            }
            if (Teachers_Bitmap != null)
            {
                Teachers_Bitmap.Dispose(); Teachers_Bitmap = null;
            }

            // get rid of diminished images
            if (Input_D_Bitmap != null)
            {
                Input_D_Bitmap.Dispose(); Input_D_Bitmap = null;
            }
            if (Preprocessed_D_Bitmap != null)
            {
                Preprocessed_D_Bitmap.Dispose(); Preprocessed_D_Bitmap = null;
            }
            if (Teachers_D_Bitmap != null)
            {
                Teachers_D_Bitmap.Dispose(); Teachers_D_Bitmap = null;
            }
            if (Result_Bitmap != null)
            {
                Result_Bitmap.Dispose(); Result_Bitmap = null;
            }
            if (Result_D_Bitmap != null)
            {
                Result_D_Bitmap.Dispose(); Result_D_Bitmap = null;
            }
            ClearSegmented();
        }

        // Create 8bpp grayscale bitmap (with linear palette)
        Bitmap CreateGrayScaleBitmap(int width, int height)
        {
            Bitmap bitmap = null;

            if (width == 0 || height == 0)
                return bitmap; // no relevant data

            // create empty 8bpp grayscale bitmap
            bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // it's palette must be replaced by linear grayscale one
            ColorPalette tempPalette;
            Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            tempPalette = tempBmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                tempPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = tempPalette;
            tempBmp.Dispose();

            return bitmap;
        }


        // assign the desired image to picturebox, for displaying
        void ShowBitmap()
        {
            pictureBox_Image.Image = checkBox_Show_Thresholded.Checked ? Preprocessed_D_Bitmap : Input_D_Bitmap;

        }

        // show numerically the image size
        void ShowImageSize()
        {
            string str = Input_Bitmap == null ? "" :
                "( " + Convert.ToInt16(Input_Bitmap.Width) + ", " + Convert.ToInt16(Input_Bitmap.Height) + " ) ";
            if (Input_Bitmap != null)
            {
                if (Input_Bitmap.PixelFormat == PixelFormat.Format1bppIndexed)
                    str += "1bpp";
                else if (Input_Bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                    str += "8bpp";
                else if (Input_Bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                    str += "24bpp";
                else if (Input_Bitmap.PixelFormat == PixelFormat.Format32bppArgb)
                    str += "32bpp";
            }
            textBox_ImageSize.Text = str;
        }

        // enable/disable the buttons, depending on the loading/processing status
        // REM: also the button's color changes between grayed and "available"
        void EnableButtons(Boolean bEnable)
        {
            button_Process.BackColor = bEnable ? PROCESSBACKCOLOR : DISABLEDCOLOR;
            button_Process.Enabled = bEnable;
            button_Save.BackColor = bEnable ? SAVEBACKCOLOR : DISABLEDCOLOR;
            button_Save.Enabled = bEnable;
        }

        // convert position from window system to image system
        private Point WindowToImage(Point pos)
        {
            Point pt = new Point();

            // transform the positions from windows's coordinate system to
            // the coordinate system of image
            double ratio_image = (double)Input_Bitmap.Width / (double)Input_Bitmap.Height;
            double ratio_PictureBox = (double)pictureBox_Image.Width / (double)pictureBox_Image.Height;
            double ratio;
            double shiftx, shifty;
            if (ratio_image > ratio_PictureBox)
            {
                // the pictureBox's height is bigger than image's one
                ratio = (double)pictureBox_Image.Width / (double)(double)Input_Bitmap.Width;
                shiftx = 0;
                shifty = (double)(pictureBox_Image.Height - ratio * (double)Input_Bitmap.Height) / 2.0;
            }
            else
            {
                // the pictureBox's width is bigger than image's one
                ratio = (double)pictureBox_Image.Height / (double)(double)Input_Bitmap.Height;
                shiftx = (double)(pictureBox_Image.Width - ratio * (double)Input_Bitmap.Width) / 2.0;
                shifty = 0;
            }
            pt.X = (int)((double)(pos.X - shiftx) / ratio + 0.5);
            pt.Y = (int)((double)(pos.Y - shifty) / ratio + 0.5);

            return pt;
        }

        // convert position from image system to window system
        private Point ImageToWindow(Point pos)
        {
            Point pt = new Point();
            pt.X = pt.Y = 0;

            if (Input_Bitmap == null)
                return pt;

            double ratio_image = (double)Input_Bitmap.Width / (double)Input_Bitmap.Height;
            double ratio_PictureBox = (double)pictureBox_Image.Width / (double)pictureBox_Image.Height;
            double ratio;
            double shiftx, shifty;
            if (ratio_image > ratio_PictureBox)
            {
                // the pictureBox's height is bigger than image's one
                ratio = (double)pictureBox_Image.Width / (double)Input_Bitmap.Width;
                shiftx = 0;
                shifty = (double)(pictureBox_Image.Height - ratio * (double)Input_Bitmap.Height) / 2.0;
                ////         ratio *= 0.995; // ???
            }
            else
            {
                // the pictureBox's width is bigger than image's one
                ratio = (double)pictureBox_Image.Height / (double)Input_Bitmap.Height;
                shiftx = (double)(pictureBox_Image.Width - ratio * (double)Input_Bitmap.Width) / 2.0;
                ////         ratio *= 0.995; // ???
                shifty = 0;
            }

            pt.X = (int)(shiftx + ratio * pos.X + 0.5);
            pt.Y = (int)(shifty + ratio * pos.Y + 0.5);

            return pt;
        }

        // show cursor position numerically
        public void ShowPosition(int xpos, int ypos)
        {
            if (xpos < 0)
            {
                textBox_Position.Text = "";
                return;
            }
            textBox_Position.Text = "( " + Convert.ToString(xpos) + ", " + Convert.ToString(ypos) + " )";
            textBox_Position.Refresh();
        }

        // show the selected area of original image in zoom window
        public void ShowZoomedArea()
        {
            if (Input_Bitmap == null || !checkBox_ShowZoom.Checked)
                return;

            // show the selected area in zoom window
            int zoomWidth = zoomwindow.pictureBox_Zoom.Width;
            int zoomHeight = zoomwindow.pictureBox_Zoom.Height;
            halfWidth = zoomWidth / 2;
            halfHeight = zoomHeight / 2;
            Bitmap tempBitmap = null;
            tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);

            Graphics bmGraphics = Graphics.FromImage(tempBitmap);
            bmGraphics.Clear(Color.White);
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            bmGraphics.DrawImage(Input_Bitmap,
                    new Rectangle(0, 0, zoomWidth, zoomHeight),
                    new Rectangle(pos.X - halfWidth, pos.Y - halfHeight,
                    zoomWidth, zoomHeight), GraphicsUnit.Pixel);


            // draw the bordering lines of teacher's spots
            ShowFieldsZoomOverlay(bmGraphics);

            // draw cross in the middle - for picking up object
            Color crossbkgcolor = Color.FromArgb(255, 255 - button_Zoom_Color.BackColor.R, 255 - button_Zoom_Color.BackColor.G, 255 - button_Zoom_Color.BackColor.B);
            Pen CrossBkgPen = new Pen(crossbkgcolor, 4);
            bmGraphics.DrawLine(CrossBkgPen, halfWidth, halfHeight - CROSS_SIZE - 1, halfWidth, halfHeight + CROSS_SIZE + 1);
            bmGraphics.DrawLine(CrossBkgPen, halfWidth - CROSS_SIZE - 1, halfHeight, halfWidth + CROSS_SIZE + 1, halfHeight);

            Color crosscolor = button_Zoom_Color.BackColor;
            Pen CrossPen = new Pen(crosscolor, 2);
            bmGraphics.DrawLine(CrossPen, halfWidth, halfHeight - CROSS_SIZE, halfWidth, halfHeight + CROSS_SIZE);
            bmGraphics.DrawLine(CrossPen, halfWidth - CROSS_SIZE, halfHeight, halfWidth + CROSS_SIZE, halfHeight);

            zoomwindow.pictureBox_Zoom.Image = tempBitmap;

            CrossBkgPen.Dispose();
            CrossPen.Dispose();
            bmGraphics.Dispose();
            zoomwindow.pictureBox_Zoom.Refresh();
        }

        // show the selected area of thresholded grayscale image in zoomthr window
        public void ShowZoomedThrArea()
        {
            if (Preprocessed_Bitmap == null || !checkBox_ShowZoomThr.Checked)
                return;

            // show the selected area in zoom window
            int zoomWidth = zoomwindowThr.pictureBox_Zoom.Width;
            int zoomHeight = zoomwindowThr.pictureBox_Zoom.Height;
            halfWidth = zoomWidth / 2;
            halfHeight = zoomHeight / 2;
            Bitmap tempBitmap = null;
            tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);

            Graphics bmGraphics = Graphics.FromImage(tempBitmap);
            bmGraphics.Clear(Color.White);
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            bmGraphics.DrawImage(Preprocessed_Bitmap,
                    new Rectangle(0, 0, zoomWidth, zoomHeight),
                    new Rectangle(pos.X - halfWidth, pos.Y - halfHeight,
                    zoomWidth, zoomHeight), GraphicsUnit.Pixel);


            // draw the bordering lines of teacher's spots
            ShowFieldsZoomOverlay(bmGraphics);

            // draw cross in the middle - for picking up object
            Color crossbkgcolor = Color.FromArgb(255, 255 - button_Zoom_Color.BackColor.R, 255 - button_Zoom_Color.BackColor.G, 255 - button_Zoom_Color.BackColor.B);
            Pen CrossBkgPen = new Pen(crossbkgcolor, 4);
            bmGraphics.DrawLine(CrossBkgPen, halfWidth, halfHeight - CROSS_SIZE - 1, halfWidth, halfHeight + CROSS_SIZE + 1);
            bmGraphics.DrawLine(CrossBkgPen, halfWidth - CROSS_SIZE - 1, halfHeight, halfWidth + CROSS_SIZE + 1, halfHeight);

            Color crosscolor = button_Zoom_Color.BackColor;
            Pen CrossPen = new Pen(crosscolor, 2);
            bmGraphics.DrawLine(CrossPen, halfWidth, halfHeight - CROSS_SIZE, halfWidth, halfHeight + CROSS_SIZE);
            bmGraphics.DrawLine(CrossPen, halfWidth - CROSS_SIZE, halfHeight, halfWidth + CROSS_SIZE, halfHeight);

            zoomwindowThr.pictureBox_Zoom.Image = tempBitmap;

            CrossBkgPen.Dispose();
            CrossPen.Dispose();
            bmGraphics.Dispose();
            zoomwindowThr.pictureBox_Zoom.Refresh();
        }

        private void button_ObjectColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = false;
            cd.ShowHelp = true;
            cd.Color = button_Zoom_Color.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                button_ObjectColor.BackColor = cd.Color;
                SettingsFile sf = new SettingsFile();
                sf.SetObjectColor(cd.Color);
                pictureBox_Image.Invalidate();
            }
        }

        // adjust the colour of rectangle showing the zoomed area
        private void button_Zoom_Color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = false;
            cd.ShowHelp = true;
            cd.Color = button_Zoom_Color.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                button_Zoom_Color.BackColor = cd.Color;
                SettingsFile sf = new SettingsFile();
                sf.SetZoomedFieldColor(cd.Color);
                pictureBox_Image.Invalidate();
            }

        }

        //-------------------------------------------------------------------------------------------

        // show rectangle of zoom window over main window

        public void ShowZoomWindowAsOverlay(Graphics g)
        {
            if (!checkBox_ShowZoom.Checked || Input_Bitmap == null || pos.X < 0 || pos.X >= Input_Bitmap.Width || pos.Y < 0 || pos.Y >= Input_Bitmap.Height)
                return;

            int zoomWidth = zoomwindow.pictureBox_Zoom.Width;
            int zoomHeight = zoomwindow.pictureBox_Zoom.Height;
            halfWidth = zoomWidth / 2;
            halfHeight = zoomHeight / 2;

            Color pencolor = button_Zoom_Color.BackColor;
            Pen linePen = new Pen(pencolor, 3);
            Point LeftTop = new Point();
            Point RightBottom = new Point();

            LeftTop.X = pos.X - halfWidth;
            LeftTop.Y = pos.Y - halfHeight;
            RightBottom.X = pos.X + halfWidth;
            RightBottom.Y = pos.Y + halfHeight;

            LeftTop = ImageToWindow(LeftTop);
            RightBottom = ImageToWindow(RightBottom);

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;

            g.DrawLine(linePen, LeftTop.X, LeftTop.Y, RightBottom.X, LeftTop.Y);
            g.DrawLine(linePen, RightBottom.X, LeftTop.Y, RightBottom.X, RightBottom.Y);
            g.DrawLine(linePen, RightBottom.X, RightBottom.Y, LeftTop.X, RightBottom.Y);
            g.DrawLine(linePen, LeftTop.X, RightBottom.Y, LeftTop.X, LeftTop.Y);

            linePen.Dispose();

        }

        // execute the zooming timer's tick event
        private void timer_zoom_pan_Tick(object sender, EventArgs e)
        {
            timer_zoom_pan.Stop();

            pictureBox_Image.Invalidate();
            bZoomPanTimerStarted = false;
        }

        // Process the loaded image
        private void button_Process_Click(object sender, EventArgs e)
        {
            Process();
        }

        public void PreProcess()
        {

            if (Input_Bitmap == null)
                return;

            // 1. convert to grayscale (for spot segmentation - and pick up teachers
            Cursor = Cursors.WaitCursor;
            ImageProcessingUtils.CImageProcessingUtils.ConvertToGrayScale_8bpp(Input_Bitmap, Grayscale_Bitmap);

            // 2. create thresholded version of input by automatic (Otsu) or manual thresholding
            if (radioButton_Manual_thresholding.Checked)
            {
                int threshold = Convert.ToInt16(numericUpDown_threshold.Value);
                ImageProcessingUtils.CImageProcessingUtils.Threshold_8bpp(Grayscale_Bitmap, Preprocessed_Bitmap, threshold);
            }
            else
            {
                int destroyedband = Convert.ToInt16(numericUpDown_Otsu.Value);
                ImageProcessingUtils.CImageProcessingUtils.Otsu_8bpp(Grayscale_Bitmap, Preprocessed_Bitmap, destroyedband);
            }
            ImageProcessingUtils.CImageProcessingUtils.DownscaleBitmap(Preprocessed_Bitmap, DOWNSCALINGFACTOR, Preprocessed_D_Bitmap);

            // 3. segment the thresholded image, resulting in different codes for every separated spots.
            //    REM:  - in parallel, the most basic features are extracted in a list, for faster teaching and identification
            //          - the pixel format for segmented image as well as the size of feature list is limited to 16bits representation,
            //            as theoretically it is enough for all realistic situations
            ImageProcessingUtils.CImageProcessingUtils.SegmentImage_8bpp(Preprocessed_Bitmap, Segmented, Features0, FEATURES0ARRAYLENGTH);

            bPreProcessed = true;
            Cursor = Cursors.Default;

        }

        public void Process()
        {

            if (Input_Bitmap == null)
                return;

            textBox_ExecutionTime.Text = "";
            Cursor = Cursors.WaitCursor;

            DateTime d1 = DateTime.Now; // store the starting time of execution

            if (!bPreProcessed)
                PreProcess();

            // Create the vector of types (containing '0' for OTHER objects and '1' for OUR objects)
            TeacherFile tf = new TeacherFile();
            int nbofnames = tf.GetNumberOfNames();
            if (nbofnames == 0)
                return;

            int[] Types = { };
            Array.Resize(ref Types, nbofnames);
            for (int ii = 0; ii < nbofnames; ii++)
            {
                Types[ii] = (tf.GetType(ii) == "OUR") ? 1 : 0;
            }

            Color ourcolor = button_ObjectColor.BackColor;
            double confidence = Convert.ToDouble(numericUpDown_Confidence.Value);
            Boolean[] UsedFeatures = new Boolean[NBOFFEATURES];
            UsedFeatures[0] = tf.GetUsedFeatures_RComponent();
            UsedFeatures[1] = tf.GetUsedFeatures_GComponent();
            UsedFeatures[2] = tf.GetUsedFeatures_BComponent();
            UsedFeatures[3] = tf.GetUsedFeatures_Grayscale();
            UsedFeatures[4] = tf.GetUsedFeatures_Area();
            UsedFeatures[5] = tf.GetUsedFeatures_MinDiameter();
            UsedFeatures[6] = tf.GetUsedFeatures_MaxDiameter();
            UsedFeatures[7] = tf.GetUsedfeatures_Aspectratio();
            UsedFeatures[8] = tf.GetUsedfeatures_Circularity();

            // execute the recognition
            while (bInputBitmapLocked || bResultBitmapLocked)
            {
                // do nothing (other thread will unlock the locked bitmap)
            }
            bInputBitmapLocked = bResultBitmapLocked = true;
            ImageProcessingUtils.CImageProcessingUtils.RecognizeWithNN(Input_Bitmap, Result_Bitmap,
                NBOFFEATURES, UsedFeatures, Types,
                ourcolor.R, ourcolor.G, ourcolor.B,
                nbOfUnits_1, nbOfUnits_2, nbOfUnits_3, Weights, confidence,
                Segmented, Features0, FEATURES0ARRAYLENGTH);
            ImageProcessingUtils.CImageProcessingUtils.DownscaleBitmap(Result_Bitmap, DOWNSCALINGFACTOR, Result_D_Bitmap);
            bInputBitmapLocked = bResultBitmapLocked = false;

            DateTime d2 = DateTime.Now; // time when execution is finished

            // show the execution time (in seconds, with milliseconds preciseness)
            TimeSpan ts = d2 - d1;
            int secdiffs = ts.Seconds;
            int msecdiffs = ts.Milliseconds;
            textBox_ExecutionTime.Text = Convert.ToString(secdiffs) + "." + Convert.ToString(msecdiffs);
            bProcessed = true;
            Cursor = Cursors.Default;

            ShowBitmap();

        }


        //--------------------------------------------------------------------------------------------------------------------------------------

        // mouse click event of pictureBox
        private void pictureBox_Image_MouseClick(object sender, MouseEventArgs e)
        {
            if (Input_Bitmap == null)
                return;

            pos0.X = e.X;
            pos0.Y = e.Y;
            pos = WindowToImage(pos0);

            if (bTeacherEdit)
            {
                if (bContextMenuInvoked)
                {
                    bContextMenuInvoked = false;
                    return;
                }
                TeacherIndex = checkBox_AddTeacher.Checked == true ? -1 : GetIndexofClosestTeacher(SENSITIVITY);
                if (e.Button == MouseButtons.Left)
                {
                    // ...
                }
                else if (e.Button == MouseButtons.Right)
                {
                    bContextMenuInvoked = true;
                    if (mnuContextMenu1 != null)
                        mnuContextMenu1.Dispose();
                    if (mnuContextMenu2 != null)
                        mnuContextMenu2.Dispose();

                    if (checkBox_AddTeacher.Checked == true)
                    {
                        mnuContextMenu1 = new ContextMenu();
                        MenuItem mnuItemAddTeacher = new MenuItem("Add Teacher");
                        mnuItemAddTeacher.Click += new System.EventHandler(this.AddTeacher);
                        mnuContextMenu1.MenuItems.Add(mnuItemAddTeacher);
                        this.ContextMenu = mnuContextMenu1;
                        mnuContextMenu1.Show(pictureBox_Image, pos0);
                    }
                    else if (TeacherIndex != -1)
                    {
                        mnuContextMenu2 = new ContextMenu();
                        MenuItem mnuItemRemoveTeacher = new MenuItem("Remove Teacher");
                        mnuItemRemoveTeacher.Click += new System.EventHandler(this.RemoveTeacher);
                        mnuContextMenu2.MenuItems.Add(mnuItemRemoveTeacher);
                        this.ContextMenu = mnuContextMenu2;
                        mnuContextMenu2.Show(pictureBox_Image, pos0);
                    }
                    return;
                }
                pictureBox_Image.Invalidate();
            }

        }

        // mouse move event of pictureBox
        private void pictureBox_Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (Input_Bitmap == null)
                return;

            // regain the focus, if it is lost
            if (!pictureBox_Image.Focused)
                pictureBox_Image.Focus();

            // if "Shift" key is pressed, step in the image's coordinate system, and transform back to the screen
            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                // 1. decide the direction of moving
                int dir_x = (pos0.X == e.X) ? 0 : (pos0.X < e.X) ? 1 : -1;
                int dir_y = (pos0.Y == e.Y) ? 0 : (pos0.Y < e.Y) ? 1 : -1;

                // 2. step in image's coordinate system
                pos.X += dir_x;
                pos.Y += dir_y;

                // 3. transform back to window's coordinate system - and set cursor's position
                pos0 = ImageToWindow(pos);
                Point cursorpos = pictureBox_Image.PointToScreen(pos0);
                Cursor.Position = cursorpos;
            }
            else
            {
                // transform the cursor position to the image's coordinate system
                pos0.X = e.X;
                pos0.Y = e.Y;
                pos = WindowToImage(pos0);
            }

            if (pos.X < 0 || pos.X >= Input_Bitmap.Width || pos.Y < 0 || pos.Y >= Input_Bitmap.Height)
            {
                // outside
                zoomwindow.pictureBox_Zoom.Image = null;
                pos.X = -1;
                pos.Y = -1;
                ShowPosition(pos.X, pos.Y);
                pictureBox_Image.Invalidate();
                return;
            }

            if (bTeacherEdit)
            {
                // select the closest teacher's center position - if "Add New Teacher" is not checked
                if (checkBox_AddTeacher.Checked == true)
                    TeacherIndex = -1;
                else if (e.Button != MouseButtons.Left)
                    TeacherIndex = GetIndexofClosestTeacher(SENSITIVITY);   // keep prev. selection, if Left button pressed

                if (TeacherIndex != -1)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        SetTeacherPosition(TeacherIndex, pos);
                        SaveTeachers();
                    }
                    else
                    {
                        pos = GetTeacherPosition(TeacherIndex);

                        // highlight the 'name' in the Teacher's list
                        int index = GetIndexForTeacher(TeacherIndex);
                        int nameindexindatafile = (int)Teachers[index];
                        int nameindexinlistview = GetNameIndexInListView(nameindexindatafile);
                        listView_Names.Items[nameindexinlistview].Selected = true;
                    }
                }
                ShowTeacherNumber();
            }

            ShowPosition(pos.X, pos.Y);
            ShowZoomedArea();
            ShowZoomedThrArea();

            // show the border of zoomed area (in 'overlay' color)
            pictureBox_Image.Invalidate();
        }

        // mouse wheel event of pictureBox
        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            int value = trackBar_Blending_Fields.Value;
            value += e.Delta > 0 ? +5 : -5;
            value = Math.Max(0, value);
            value = Math.Min(value, trackBar_Blending_Fields.Maximum);
            trackBar_Blending_Fields.Value = value;

            pictureBox_Image.Invalidate();
            zoomwindow.pictureBox_Zoom.Invalidate();
        }

        // changing the blending percentage
        private void trackBar_Blending_Fields_Scroll(object sender, EventArgs e)
        {
            int Rate = trackBar_Blending_Fields.Value;
            SettingsFile sf = new SettingsFile();
            sf.SetBlendingFieldsRate(Rate);

            pictureBox_Image.Invalidate();
        }

        // mouse leave event of pictureBox
        private void pictureBox_Image_MouseLeave(object sender, EventArgs e)
        {
            if (bContextMenuInvoked)
                return;

            zoomwindow.pictureBox_Zoom.Image = null;
            pos.X = -1;
            pos.Y = -1;
            ShowPosition(pos.X, pos.Y);

            // show the border of zoomed area (in 'overlay' color)
            pictureBox_Image.Invalidate();
        }

        // show the overlay objects filled
        private void checkBox_ShowFilledFields_CheckedChanged(object sender, EventArgs e)
        {
            SettingsFile sf = new SettingsFile();
            sf.SetShowFilledFields(checkBox_ShowFilledFields.Checked);
            bTeachersBitmapCreated = false;

            pictureBox_Image.Invalidate();
        }

        private void checkBox_ShowZoom_CheckedChanged(object sender, EventArgs e)
        {

            bool bShowZoom = checkBox_ShowZoom.Checked;

            SettingsFile sf = new SettingsFile();
            sf.SetShowZoom(bShowZoom);

            if (bShowZoom)
                zoomwindow.Show();
            else
                zoomwindow.Hide();

        }

        private void checkBox_ShowZoomThr_CheckedChanged(object sender, EventArgs e)
        {

            bool bShowZoomThr = checkBox_ShowZoomThr.Checked;

            SettingsFile sf = new SettingsFile();
            sf.SetShowZoomThr(bShowZoomThr);

            if (bShowZoomThr)
                zoomwindowThr.Show();
            else
                zoomwindowThr.Hide();
        }

        // init the Features0 data vector
        public void ClearFeatures0()
        {
            Array.Resize(ref Features0, FEATURES0ARRAYLENGTH);
            for (int ii = 0; ii < FEATURES0ARRAYLENGTH; ii++)
                Features0[ii] = 0;
        }

        // init. the segmented data array
        public void ClearSegmented()
        {
            // get the size of array
            int nbofpixels = 0;
            if (Input_Bitmap != null)
                nbofpixels = Input_Bitmap.Width * Input_Bitmap.Height;

            // size/resize and init
            Array.Resize(ref Segmented, nbofpixels);
            for (int ii = 0; ii < nbofpixels; ii++)
                Segmented[ii] = 0;
        }

        // init Teachers (get rid of previous result objects)
        public void ClearTeachers()
        {
            Array.Resize(ref Teachers, TEACHERSARRAYLENGTH);
            for (int ii = 0; ii < TEACHERSARRAYLENGTH; ii++)
                Teachers[ii] = 0;
        }

        //--------------------------------------------------------------------------------
        private void SetNumberOfUnits()
        {
            TeacherFile tf = new TeacherFile();
            int nbofnames = tf.GetNumberOfNames();

            nbOfUnits_1 = NBOFFEATURES;
            nbOfUnits_2 = nbofnames;
            nbOfUnits_3 = nbofnames;

            textBox_inputunits.Text = Convert.ToString(nbOfUnits_1);
            numericUpDownNbofHiddenUnits.Value = Convert.ToDecimal(nbOfUnits_2);
            textBox_outputunits.Text = Convert.ToString(nbOfUnits_3);

        }

        private void numericUpDown_Confidence_ValueChanged(object sender, EventArgs e)
        {
            bProcessed = false;
            double confidence = Convert.ToDouble(numericUpDown_Confidence.Value);
            WeightFile wf = new WeightFile();
            wf.SetConfidence(confidence);

            ClearResultBitmap();

            if (checkBox_KeepProcessing.Checked)
                Process();
        }

        private void SetupWeightArray()
        {

            int NbofWeights = nbOfUnits_1 + nbOfUnits_1 * nbOfUnits_2 + nbOfUnits_2 * nbOfUnits_3;
            Array.Resize(ref Weights, NbofWeights);
            for (int ii = 0; ii < NbofWeights; ii++)
            {
                Weights[ii] = 0.0;
            }

        }

        private void button_Teach_Click(object sender, EventArgs e)
        {
            if (!bPreProcessed)
                PreProcess();

            bTeached = TeachNN();  // invoke the dialogue for teaching the neural network
            if (bTeached)
            {
                bProcessed = false;
                ClearResultBitmap();
            }

            WeightFile wf = new WeightFile();
            SetupWeightArray();
            bTeached = wf.LoadWeights(Weights, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
        }

        private bool TeachNN()
        {
            // Teach the neural network, using the extracted features
            Learn learn = new Learn(NBOFFEATURES);
            learn.confidence = Convert.ToDouble(numericUpDown_Confidence.Value);
            learn.factor = 0.2;
            learn.nbOfUnits_1 = nbOfUnits_1;
            learn.nbOfUnits_2 = nbOfUnits_2;
            learn.nbOfUnits_3 = nbOfUnits_3;

            learn.ShowDialog();

            // reload the weights for Neural network
            WeightFile wf = new WeightFile();
            wf.LoadWeights(Weights, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);

            return true;
        }

        private void radioButton_Otsu_thresholding_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Otsu_thresholding.Checked)
            {
                TeacherFile tf = new TeacherFile();
                tf.SetOtsuThresholdingSelected(true);

                if (checkBox_KeepPreProcessing.Checked)
                    PreProcess();
                if (checkBox_KeepProcessing.Checked)
                    Process();
                else
                    ShowBitmap();
            }
        }

        private void radioButton_Manual_thresholding_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Manual_thresholding.Checked)
            {
                TeacherFile tf = new TeacherFile();
                tf.SetOtsuThresholdingSelected(false);

                if (checkBox_KeepPreProcessing.Checked)
                    PreProcess();
                if (checkBox_KeepProcessing.Checked)
                    Process();
                else
                    ShowBitmap();
            }
        }

        private void numericUpDown_threshold_ValueChanged(object sender, EventArgs e)
        {
            int thresholdvalue = Convert.ToInt16(numericUpDown_threshold.Value);
            TeacherFile tf = new TeacherFile();
            tf.SetManualThresholdValue(thresholdvalue);

            if (checkBox_KeepPreProcessing.Checked)
                PreProcess();
            if (checkBox_KeepProcessing.Checked)
                Process();
            else
                ShowBitmap();
        }

        private void numericUpDown_Otsu_ValueChanged(object sender, EventArgs e)
        {
            int thresholdvalue = Convert.ToInt16(numericUpDown_Otsu.Value);
            TeacherFile tf = new TeacherFile();
            tf.SetOtsuThresholdValue(thresholdvalue);

            if (checkBox_KeepPreProcessing.Checked)
                PreProcess();
            if (checkBox_KeepProcessing.Checked)
                Process();
            else
                ShowBitmap();
        }

        private void checkBox_Show_Thresholded_CheckedChanged(object sender, EventArgs e)
        {
            bool bShowThresholded = checkBox_Show_Thresholded.Checked;

            SettingsFile sf = new SettingsFile();
            sf.SetShowThresholded(bShowThresholded);

            ShowBitmap();
        }


        private void ClearResultBitmap()
        {
            while (bResultBitmapLocked)
            {
                // do nothing (other thread will unlock the locked bitmap)
            }
            bResultBitmapLocked = true;
            ImageProcessingUtils.CImageProcessingUtils.CleanBitmap_24bpp(Result_Bitmap);
            bResultBitmapLocked = false;
        }

        private void checkBox_KeepProcessing_CheckedChanged(object sender, EventArgs e)
        {
            bool bKeepProcessing = checkBox_KeepProcessing.Checked;

            SettingsFile sf = new SettingsFile();
            sf.SetKeepProcessing(bKeepProcessing);

            if (bKeepProcessing)
                Process();
        }

        private void trackBar_Blending_Fields_MouseEnter(object sender, EventArgs e)
        {
            tt1.RemoveAll();
            tt1.SetToolTip(trackBar_Blending_Fields, "Blending with Teachers");
        }

        private void trackBar_Blending_Result_MouseEnter(object sender, EventArgs e)
        {
            tt1.RemoveAll();
            tt1.SetToolTip(trackBar_Blending_Result, "Blending with Result");
        }

        private void numericUpDownNbofHiddenUnits_ValueChanged(object sender, EventArgs e)
        {
            // store the number of units to Weights data file
            nbOfUnits_2 = Convert.ToInt16(numericUpDownNbofHiddenUnits.Value);
            WeightFile wf = new WeightFile();
            wf.SaveSettings(nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
        }


        private void trackBar_Blending_Result_Scroll(object sender, EventArgs e)
        {
            int Rate = trackBar_Blending_Result.Value;
            SettingsFile sf = new SettingsFile();
            sf.SetBlendingResultRate(Rate);

            pictureBox_Image.Invalidate();
        }

        //------------------------------------------------------------------------------------
        // ### Teacher ###

        private void radioButton_CurrentTeachers_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_CurrentTeachers.Checked)
            {
                FillListView_TeacherNames(false);
                pictureBox_Image.Invalidate();
            }
        }

        private void radioButton_AllTeachers_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AllTeachers.Checked)
            {
                FillListView_TeacherNames(true);
                pictureBox_Image.Invalidate();
            }
        }

        private void button_Teacher_Our_Color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = false;
            cd.ShowHelp = true;
            cd.Color = button_Teacher_Our_Color.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                button_Teacher_Our_Color.BackColor = cd.Color;
                bTeachersBitmapCreated = false;
                pictureBox_Image.Invalidate();
            }

            TeacherFile tf = new TeacherFile();
            tf.SetTeacherOurColor(cd.Color);
        }

        private void button_Teacher_Other_Color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = false;
            cd.ShowHelp = true;
            cd.Color = button_Teacher_Other_Color.BackColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                button_Teacher_Other_Color.BackColor = cd.Color;
                bTeachersBitmapCreated = false;
                pictureBox_Image.Invalidate();
            }

            TeacherFile tf = new TeacherFile();
            tf.SetTeacherOtherColor(cd.Color);
        }

        private void button_AddName_Click(object sender, EventArgs e)
        {
            string name_value = textBox_Name.Text;
            if (name_value == "")
            {
                MessageBox.Show("Name must be given!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // check, if this name is already registered?
            bool bFound = false;
            foreach (ListViewItem lvi in listView_Names.Items)
            {
                if (lvi.SubItems[0].Text == name_value)
                    bFound = true;
            }
            if (bFound)
            {
                MessageBox.Show("This name is already registered!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // new name - add to the ListView
            string type = radioButton_OurObject.Checked ? "OUR" : "OTHER";
            ListViewItem item = new ListViewItem(new string[] { name_value, "0", type });
            listView_Names.Items.Add(item);
            textBox_Name.Text = "";

            // add the new item to the File_Teacher.dat file
            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            tf.AddName(name_value, type);
            int nbofallteachers = tf.GetNumberOfTeachers();
            tf.GetTeachers(Teachers);

            int selecteditemindex = -1;
            if (listView_Names.SelectedItems.Count != 0 && listView_Names.SelectedIndices[0] != 0)
                selecteditemindex = listView_Names.SelectedIndices[0];
            if (radioButton_CurrentTeachers.Checked)
                FillListView_TeacherNames(false);
            else
                FillListView_TeacherNames(true);
            if (selecteditemindex != -1)
                listView_Names.Items[selecteditemindex].Selected = true;

            SetNumberOfUnits();

            bTeached = false;
            bProcessed = false;
            ClearResultBitmap();

        }

        private void button_RenameSelected_Click(object sender, EventArgs e)
        {
            // if no item is selected in ListView (or the "ALL" is selected), warning
            if (listView_Names.SelectedItems.Count == 0 || listView_Names.SelectedIndices[0] == 0)
            {
                MessageBox.Show("An existing item must be selected in ListView!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selecteditemindex = listView_Names.SelectedIndices[0];
            string newtype = radioButton_OurObject.Checked ? "OUR" : "OTHER";

            string newname = textBox_Name.Text;
            ListViewItem lvi2 = listView_Names.Items[selecteditemindex];
            string oldname = lvi2.SubItems[0].Text;
            if (newname != "" && newname != oldname)
            {
                // check, if the new name is already registered?
                bool bFound = false;
                foreach (ListViewItem lvi in listView_Names.Items)
                {
                    if (lvi.SubItems[0].Text == newname)
                        bFound = true;
                }
                if (bFound)
                {
                    MessageBox.Show("This name is already registered!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // execute the renaming
            if (newname == "")
                newname = oldname;  // REM: only the "type" will be rewritten
            lvi2.SubItems[0].Text = newname;
            lvi2.SubItems[2].Text = newtype;
            listView_Names.Items[selecteditemindex] = lvi2;
            int count = listView_Names.Items.Count;
            selecteditemindex = (count > selecteditemindex) ? selecteditemindex : count;
            listView_Names.Items[selecteditemindex].Selected = true;
            listView_Names.Select();

            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            tf.RenameName(oldname, newname, newtype);

            textBox_Name.Text = "";

            bTeached = false;
            bProcessed = false;
            ClearResultBitmap();
            pictureBox_Image.Invalidate();

        }

        private void button_DeleteSelected_Click(object sender, EventArgs e)
        {
            if (listView_Names.SelectedItems.Count == 0 || listView_Names.SelectedIndices[0] == 0)
                return; // (Nothing to delete (The first item "ALL" is fixed,  only one item can be deleted)

            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            int selecteditemindex = listView_Names.SelectedIndices[0];

            // confirm, if the user want's to delete the selected item
            DialogResult result = MessageBox.Show("Are you sure that you would like to delete the selected teacher data?",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ListViewItem lvi = listView_Names.Items[selecteditemindex];
                string name = lvi.SubItems[0].Text;
                tf.RemoveName(name);
                int nbofallteachers = tf.GetNumberOfTeachers();
                string filename = textBox_ImageFileName.Text;
                NumberOfTeachers = tf.GetNumberOfTeachers(filename);
                ClearTeachers();
                tf.GetTeachers(filename, Teachers);

                if (radioButton_CurrentTeachers.Checked)
                {
                    FillListView_TeacherNames(false);
                }
                else
                {
                    FillListView_TeacherNames(true);
                }
                int count = listView_Names.Items.Count;
                selecteditemindex = (count > selecteditemindex) ? selecteditemindex : count;
            }

            SetNumberOfUnits();

            SaveTeachers();
            ShowTeacherNumber();

            bTeached = false;
            bProcessed = false;
            ClearResultBitmap();
            pictureBox_Image.Invalidate();
        }

        private void button_DeleteAll_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure that you would like to discard all teacher data?",
                "Warning", buttons, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            // discard all teacher data
            ClearTeachers();
            NumberOfTeachers = 0;
            InitNamesListView();
            TeacherFile tf = new TeacherFile();
            tf.CleanAll();
            tf.SetFeatureNumber(NBOFFEATURES);
            int nbofallteachers = tf.GetNumberOfTeachers();
            tf.GetTeachers(Teachers);

            nbOfUnits_2 = 0;
            nbOfUnits_3 = 0;

            bTeached = false;
            bProcessed = false;
            ClearResultBitmap();

            ShowTeacherNumber();

            pictureBox_Image.Invalidate();

        }

        private void button_Previous_Click(object sender, EventArgs e)
        {
            if (TeacherIndex > 0)
                TeacherIndex--;

            pos = GetTeacherPosition(TeacherIndex); // necessary for positioning zoom-window's content

            ShowTeacherNumber();

            // highlight the 'name' in the Teacher's list
            int index = GetIndexForTeacher(TeacherIndex);
            int nameindexindatafile = (int)Teachers[index];
            int nameindexinlistview = GetNameIndexInListView(nameindexindatafile);
            listView_Names.Items[nameindexinlistview].Selected = true;

            Point pos1 = new Point();
            pos1.X = (int)Teachers[index + 1];
            pos1.Y = (int)Teachers[index + 2];
            ShowPosition(pos1.X, pos1.Y);

            ShowZoomedArea();
            ShowZoomedThrArea();

            pictureBox_Image.Focus();
            pictureBox_Image.Invalidate();
        }


        private void button_Next_Click(object sender, EventArgs e)
        {
            if (TeacherIndex < (NumberOfTeachers - 1))
                TeacherIndex++;

            pos = GetTeacherPosition(TeacherIndex); // necessary for positioning zoom-window's content

            ShowTeacherNumber();

            // highlight the 'name' in the Teacher's list
            int index = GetIndexForTeacher(TeacherIndex);
            int nameindexindatafile = (int)Teachers[index];
            int nameindexinlistview = GetNameIndexInListView(nameindexindatafile);
            listView_Names.Items[nameindexinlistview].Selected = true;

            Point pos1 = new Point();
            pos1.X = (int)Teachers[index + 1];
            pos1.Y = (int)Teachers[index + 2];
            ShowPosition(pos1.X, pos1.Y);

            ShowZoomedArea();
            ShowZoomedThrArea();

            pictureBox_Image.Focus();
            pictureBox_Image.Invalidate();
        }

        private void FillListView_TeacherNames(bool bAllTeacher)
        {
            InitNamesListView();
            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            int nbofnames = tf.GetNumberOfNames();
            if (nbofnames == 0)
                return;

            int[] counters = new int[nbofnames];
            for (int ii = 0; ii < nbofnames; ii++)
                counters[ii] = 0;

            // get the count of teachers
            if (bAllTeacher)
            {
                int nbofdatafiles = tf.GetNumberOfDataFiles();
                for (int jj = 0; jj < nbofdatafiles; jj++)
                {
                    string filename = tf.GetFileName(jj + 1);
                    if (filename == "")
                        continue;
                    int nbofteachers = tf.GetNumberOfTeachers(filename);
                    double[] Data = new double[100000];           // overallocated - no index check
                    for (int ii = 0; ii < nbofteachers; ii++)
                    {
                        tf.RetrieveTeacherData(filename, ii + 1, Data);
                        counters[(int)Data[0] - 1]++;
                    }
                }
            }
            else
            {
                string filename = textBox_ImageFileName.Text;
                int nbofteachers = tf.GetNumberOfTeachers(filename);
                double[] Data = new double[100000];       // overallocated - no index check
                for (int ii = 0; ii < nbofteachers; ii++)
                {
                    tf.RetrieveTeacherData(filename, ii + 1, Data);
                    counters[(int)Data[0] - 1]++;
                }
            }

            for (int ii = 0; ii < nbofnames; ii++)
            {
                string name = tf.GetName(ii + 1);
                string type = tf.GetType(ii);
                if (name != "")
                {
                    string count = Convert.ToString(counters[ii]);
                    ListViewItem item1 = new ListViewItem(new string[] { name, count, type });
                    listView_Names.Items.Add(item1);
                }
            }

            // show also the overall counter
            int counter_all = 0;
            for (int ii = 0; ii < nbofnames; ii++)
                counter_all += counters[ii];
            string count_all = Convert.ToString(counter_all);
            ListViewItem item2 = new ListViewItem(new string[] { "[ALL]", count_all });
            listView_Names.Items[0] = item2;

        }

        private void InitNamesListView()
        {
            // clear all items in the ListView
            listView_Names.Items.Clear();

            // add "ALL" as first (default) item
            ListViewItem item = new ListViewItem(new string[] { "[ALL]", "0" });
            listView_Names.Items.Add(item);

        }

        private Point GetTeacherPosition(int teacherindex)
        {
            Point pos = new Point(-1, -1);

            if (teacherindex >=0)
            {
                int index = GetIndexForTeacher(teacherindex);
                int posindex = index + 1;
                if (posindex < Teachers.Length)
                    pos = new Point((int)Teachers[posindex], (int)Teachers[posindex + 1]);
            }
            return pos;
        }

        private void SetTeacherPosition(int teacherindex, Point pos)
        {
            int index = GetIndexForTeacher(teacherindex);
            int posindex = index + 1;
            if (posindex < Teachers.Length)
            {
                Teachers[posindex] = pos.X;
                Teachers[posindex + 1] = pos.Y;
            }
        }

        private void checkBox_UsedFeatureChanged(object sender, EventArgs e)
        {
            if (bUsedFeatures_Setting)
                return;                     // (adjustment is activated)

            // store the state of usage of the possible features
            TeacherFile tf = new TeacherFile();
            tf.SetUsedFeatures_RComponent(checkBox_R_component.Checked);
            tf.SetUsedFeatures_GComponent(checkBox_G_component.Checked);
            tf.SetUsedFeatures_BComponent(checkBox_B_component.Checked);
            tf.SetUsedFeatures_Grayscale(checkBox_grayscale.Checked);
            tf.SetUsedFeatures_Area(checkBox_area.Checked);
            tf.SetUsedFeatures_MinDiameter(checkBox_min_diameter.Checked);
            tf.SetUsedFeatures_MaxDiameter(checkBox_max_diameter.Checked);
            tf.SetUsedfeatures_Aspectratio(checkBox_aspect_ratio.Checked);
            tf.SetUsedfeatures_Circularity(checkBox_circularity.Checked);

            bTeached = false;
            bProcessed = false;
        }

        private void GetUsedFeatureControls()
        {
            // load the stored state of using the possible features
            bUsedFeatures_Setting = true;
            TeacherFile tf = new TeacherFile();
            checkBox_R_component.Checked = tf.GetUsedFeatures_RComponent();
            checkBox_G_component.Checked = tf.GetUsedFeatures_GComponent();
            checkBox_B_component.Checked = tf.GetUsedFeatures_BComponent();
            checkBox_grayscale.Checked = tf.GetUsedFeatures_Grayscale();
            checkBox_area.Checked = tf.GetUsedFeatures_Area();
            checkBox_min_diameter.Checked = tf.GetUsedFeatures_MinDiameter();
            checkBox_max_diameter.Checked = tf.GetUsedFeatures_MaxDiameter();
            checkBox_aspect_ratio.Checked = tf.GetUsedfeatures_Aspectratio();
            checkBox_circularity.Checked = tf.GetUsedfeatures_Circularity();
            bUsedFeatures_Setting = false;
        }

        public void ShowTeacherNumber()
        {
            string message = "";
            if (NumberOfTeachers > 0)
            {
                if (TeacherIndex == -1)
                    message = Convert.ToString(NumberOfTeachers);
                else
                    message = Convert.ToString(TeacherIndex + 1) + "/" + Convert.ToString(NumberOfTeachers);
            }
            textBox_CurrentTeacher.Text = message;
            button_Previous.Enabled = (TeacherIndex <= 0) ? false : true;
            button_Next.Enabled = (NumberOfTeachers == 0 || TeacherIndex >= (NumberOfTeachers - 1)) ? false : true;
        }

        public int GetIndexofClosestTeacher(double sensitivity)
        {
            int outindex = -1;
            if (Teachers.Length == 0)
                return outindex;

            double mindistance = DISTANCEINIT;
            int nbofteachers = (int)Teachers[0];
            int index = 1;
            int ii;
            for (ii = 0; ii < nbofteachers; ii++)
            {
                int posindex = index + 1;
                double distance = DISTANCEINIT;
                if (posindex < Teachers.Length)
                {
                    distance = Math.Sqrt(Math.Pow((pos.X - Teachers[posindex]), 2.0) + Math.Pow((pos.Y - Teachers[posindex + 1]), 2.0));
                }
                if (distance < mindistance)
                {
                    mindistance = distance;
                    outindex = ii;
                }
                int length = 3 + NBOFFEATURES;
                length += 2 * (int)Teachers[index + length] + 1;
                index += length;
            }
            if (mindistance > sensitivity)
                outindex = -1;

            return outindex;
        }

        private void AddTeacher(object sender, System.EventArgs e)
        {
            if (Input_Bitmap == null)
                return;

            if (listView_Names.SelectedItems.Count == 0 || listView_Names.SelectedIndices[0] == 0)
            {
                MessageBox.Show("Name must be given and selected in the 'Teachers' list!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (TeacherIndex != -1)
                return;

            // add new teacher if under the cursor's position a segmented object is situated
            int spotSN = Segmented[pos.X + Input_Bitmap.Width * pos.Y];
            if ( spotSN==0 )
            {
                MessageBox.Show("Only segmented spot can be added to the list of teachers!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bTeachersBitmapCreated = false;

            // extract the features for the picked spot and store it as new teacher
            int newitem_index = GetIndexForNewTeacher();
            NumberOfTeachers++; 
            Teachers[0] = (double)NumberOfTeachers;
            int nameindex = listView_Names.SelectedIndices[0];    // serial number of 'name' - in listbox
            ListViewItem lvi = listView_Names.Items[nameindex];
            String name = lvi.SubItems[0].Text;
            TeacherFile tf = new TeacherFile();
            int nameindexindatafile = tf.GetNameIndex(name);
            Teachers[newitem_index] = nameindexindatafile;
            ImageProcessingUtils.CImageProcessingUtils.GetFeaturesOfSelectedTeacher(Input_Bitmap, Segmented, Features0,
                spotSN, Teachers, newitem_index);

            TeacherIndex = -1;
            ShowTeacherNumber();

            // increment the counter in the list, for the 'name' of added teacher
            int counter = Convert.ToUInt16(lvi.SubItems[1].Text);
            counter++;
            lvi.SubItems[1].Text = Convert.ToString(counter);
            listView_Names.Items[nameindex] = lvi;

            // increment the counter of ALL
            lvi = listView_Names.Items[0];
            int numberofALL = Convert.ToInt16(lvi.SubItems[1].Text);
            numberofALL++;
            lvi.SubItems[1].Text = Convert.ToString(numberofALL);
            listView_Names.Items[0] = lvi;

            bContextMenuInvoked = false;
            mnuContextMenu1.Dispose();

            // set the cursor's position in the main window
            Point cursorpos = pictureBox_Image.PointToScreen(pos0);
            Cursor.Position = cursorpos;

            SaveTeachers();

            if (!bZoomPanTimerStarted)
            {
                bZoomPanTimerStarted = true;
                timer_zoom_pan.Start();
            }

            bProcessed = false;
        }

        private void RemoveTeacher(object sender, System.EventArgs e)
        {
            NumberOfTeachers = (int)Teachers[0];
            if (TeacherIndex == -1 || NumberOfTeachers == 0)
                return;         // nothing to remove

            bTeachersBitmapCreated = false;

            bContextMenuInvoked = false;
            mnuContextMenu2.Dispose();

            // get rid of TeacherIndex'th teacher
            TeacherFile tf = new TeacherFile();
            string filename = textBox_ImageFileName.Text;
            int ID = tf.GetIDForDataFile(filename);
            String DataFileID = "DataFile_" + Convert.ToString(ID);
            tf.RemoveTeacher(filename, DataFileID, TeacherIndex+1);

            // reload the modified teacher data from file
            NumberOfTeachers = tf.GetNumberOfTeachers(filename);
            ClearTeachers();
            tf.GetTeachers(filename, Teachers);
            TeacherIndex = -1;
            ShowTeacherNumber();
            RefreshNamesListView();

            // set the cursor's position in the main window
            Point cursorpos = pictureBox_Image.PointToScreen(pos0);
            Cursor.Position = cursorpos;

            if (!bZoomPanTimerStarted)
            {
                bZoomPanTimerStarted = true;
                timer_zoom_pan.Start();
            }

            bProcessed = false;

        }

        public void SaveTeachers()
        {
            if (Teachers.Length == 0)
                return;

            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            string filename = textBox_ImageFileName.Text;
            tf.ClearTeachersOfCurrentImage(filename);
            int nbofteachers = (int)Teachers[0];
            for (int ii = 0; ii < nbofteachers; ii++)
            {
                int teacherindex = GetIndexForTeacher(ii);
                tf.StoreTeacherData(filename, Teachers, teacherindex);
            }

        }

        private int GetNameIndexInListView(int nameindexindatafile)
        {
            TeacherFile tf = new TeacherFile();
            string name = tf.GetName(nameindexindatafile);

            int count = listView_Names.Items.Count;
            bool bFound = false;
            int ii = 0;
            while (!bFound && ii < count)
            {
                ListViewItem lvi = listView_Names.Items[ii];
                string nameinlistview = lvi.SubItems[0].Text;
                if (name == nameinlistview)
                    bFound = true;
                else
                    ii++;
            }

            return ii;
        }

        private void tabControl_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pagename = tabControl_User.SelectedTab.Name;
            if ( pagename == "tabPage3")
            {
                // "Teachers" page is selected
                bTeacherEdit = true;
            }
            else
            {
                bTeacherEdit = false;
            }
            RefreshNamesListView();
            pictureBox_Image.Invalidate();
        }

        private bool IsItOurTeacherObject(int teacherindex)
        {
            int index = GetIndexForTeacher(teacherindex);

            int name_sn = (int)Teachers[index];

            TeacherFile tf = new TeacherFile();
            string type = tf.GetType(name_sn - 1);
            bool bOurObject = type == "OUR" ? true : false;

            return bOurObject;
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            // invoke the proper help-page, depending on the selected TAB-page
            string helpmessage = "";
            string pagename = tabControl_User.SelectedTab.Name;
            if (pagename == "tabPage1")
            {
                // "Preprocessing" page is selected
                helpmessage += "Preprocessing of the input image, resulting in a thresholded image.\n\n\n";

                helpmessage += "Otsu: an automatic thresholding method, finding the 'optimal' value.\n";
                helpmessage += "         The user can define band widths at lowest and highest pixel values.\n";
                helpmessage += "         where the counts are not taken into consideration.\n";
                helpmessage += "Manual thresholding: the user can give the explicit threshold value.\n\n";
            }
            else if (pagename == "tabPage2")
            {
                // "Features" page is selected
                helpmessage += "Selecting the features to be used:\n\n\n";

                helpmessage += "   Those features will be used, which are checked in the 'Used features' field.\n\n";
                helpmessage += "Remark:  The R,G and B components are computed in the teacher's spot.\n";
                helpmessage += "                 The grayscale value is their average.\n";
                helpmessage += "                 The area is the number of pixels within it's spot.\n";
                helpmessage += "                 The min. and max. diameters are computers over it's contour.\n";
                helpmessage += "                 The aspect ratio and circularity are commonly used shape factors.\n\n";
            }
            else if (pagename == "tabPage3")
            {
                // "Teachers" page is selected
                helpmessage += "Teacher creation and modification using mouse:\n\n\n";

                helpmessage += "Name: before adding new Teacher, a name must be given\n";
                helpmessage += "                     (Add, Rename, Delete buttons).\n\n";
                helpmessage += "Add New Teacher: Right click over desired position and press\n";
                helpmessage += "                     the 'Add Teacher'' context menupoint.\n";
                helpmessage += "Remove Teacher: right click over existing teacher and press\n";
                helpmessage += "                     the 'Remove Teacher' context menupoint.\n";
                helpmessage += "Remark:  For removing or editing existing ROI, the Add New Teacher\n";
                helpmessage += "                 checkbox must be unchecked.\n\n";
            }
            else if (pagename == "tabPage4")
            {
                // "NN" (neural network)
                helpmessage += "Using Neural Network for finding the desired content on the image:\n\n\n";

                helpmessage += "The Neural Network's decisions are based on the weights,\n";
                helpmessage += "that were created in the dialog, invoked by 'Teach!' button.\n";
                helpmessage += "\n";
                helpmessage += "Here only the number of hidden layer's units, and the\n";
                helpmessage += "threshold of accepting the decisions can be given by the user.\n";
                helpmessage += "\n";
                helpmessage += "Pressing the 'Process' button, the NN creates the result.\n\n";

            }
            helpmessage += "\n";

            Assembly MyAsm = Assembly.Load("AutoSegment");
            AssemblyName aName = MyAsm.GetName();
            Version ver = aName.Version;
            string title = "AutoSegment   [Version: " + ver + " ]  Help";

            MessageBox.Show(helpmessage, title);
        }

        private void listView_Names_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bValidItemSelected = (listView_Names.SelectedItems.Count==1 && listView_Names.SelectedIndices[0]>0 ) ? true:false;
            button_DeleteSelected.Enabled = bValidItemSelected;
            button_RenameSelected.Enabled = bValidItemSelected;

            pictureBox_Image.Focus();
        }

        private void button_PreProcess_Click(object sender, EventArgs e)
        {
            PreProcess();
            ShowBitmap();
        }

        private void checkBox_KeepPreProcessing_CheckedChanged(object sender, EventArgs e)
        {
            bool bKeepPreProcessing = checkBox_KeepPreProcessing.Checked;

            SettingsFile sf = new SettingsFile();
            sf.SetKeepPreProcessing(bKeepPreProcessing);

            if (bKeepPreProcessing)
            {
                PreProcess();
                ShowBitmap();
            }
        }

        private void checkBox_AddTeacher_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AddTeacher.Checked && !bPreProcessed)
            {
                // as no teacher can be added without having the preprocessed (thresholded) image, preprocessing must be executed
                PreProcess();
                ShowBitmap();
            }
        }

        public void ShowResultOverlay(Graphics g, int output_width, int output_height)
        {
            if (bProcessed && Result_D_Bitmap != null)
            {
                float Alpha = (float)trackBar_Blending_Result.Value / 100.0f;  // REM: 100 steps distinguished
                Size psize = new Size(output_width, output_height);
                Rectangle rect1 = new Rectangle(Point.Empty, psize);
                double ratio1 = (double)psize.Width / (double)Result_D_Bitmap.Width;
                int pheight = (int)((double)Result_D_Bitmap.Height * ratio1 + 0.5);
                if (pheight <= psize.Height)
                {
                    // fit-to-width
                    rect1.X = 0;
                    rect1.Y = (psize.Height - pheight) / 2;
                    rect1.Width = psize.Width;
                    rect1.Height = pheight;
                }
                else
                {
                    // fit-to-height
                    ratio1 = (double)psize.Height / (double)Result_D_Bitmap.Height;
                    int pwidth = (int)((double)Result_D_Bitmap.Width * ratio1 + 0.5);
                    rect1.X = (psize.Width - pwidth) / 2;
                    rect1.Y = 0;
                    rect1.Width = pwidth;
                    rect1.Height = psize.Height;
                }

                ImageAttributes attr = new ImageAttributes();
                float[][] ptsArray ={
                  new float[] {1, 0, 0, 0, 0},
                  new float[] {0, 1, 0, 0, 0},
                  new float[] {0, 0, 1, 0, 0},
                  new float[] {0, 0, 0, Alpha, 0},
                  new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                attr.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(Result_D_Bitmap, rect1,
                    0, 0, Result_D_Bitmap.Width, Result_D_Bitmap.Height, GraphicsUnit.Pixel, attr);
            }
        }

        public int GetIndexForTeacher( int teacherindex)
        {
            int index = 1;
            for (int ii = 0; ii < teacherindex; ii++)
            {
                int length = 3 + NBOFFEATURES;  // offset for number of vertex positions
                length += 2 * (int)Teachers[index + length] + 1;
                index += length;
            }

            return index;

        }

        public int GetIndexForNewTeacher()
        {
            int newitemindex = 1;
            int nbofexistingteachers = (int)Teachers[0];
            for ( int ii=0; ii< nbofexistingteachers; ii++)
            {
                int length = 3 + NBOFFEATURES;  // offset for number of vertex positions
                length += 2*(int)Teachers[newitemindex + length] + 1;
                newitemindex += length;
            }

            return newitemindex;
        }

    }
    //--------------------------------------------------------------------------------


}