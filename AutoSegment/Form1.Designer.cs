namespace AutoSegment
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_Exit = new System.Windows.Forms.Button();
            this.groupBox_Images = new System.Windows.Forms.GroupBox();
            this.label_FileName = new System.Windows.Forms.Label();
            this.button_BrowseImage = new System.Windows.Forms.Button();
            this.textBox_ImageFileName = new System.Windows.Forms.TextBox();
            this.textBox_ImageSize = new System.Windows.Forms.TextBox();
            this.pictureBox_Image = new System.Windows.Forms.PictureBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.trackBar_Blending_Fields = new System.Windows.Forms.TrackBar();
            this.label_Position1 = new System.Windows.Forms.Label();
            this.textBox_Position = new System.Windows.Forms.TextBox();
            this.button_Zoom_Color = new System.Windows.Forms.Button();
            this.timer_zoom_pan = new System.Windows.Forms.Timer(this.components);
            this.groupBox_Show = new System.Windows.Forms.GroupBox();
            this.checkBox_Show_Thresholded = new System.Windows.Forms.CheckBox();
            this.groupBox_Colors = new System.Windows.Forms.GroupBox();
            this.checkBox_ShowZoomThr = new System.Windows.Forms.CheckBox();
            this.button_ZoomThr_Dummy = new System.Windows.Forms.Button();
            this.label_object = new System.Windows.Forms.Label();
            this.button_ObjectColor = new System.Windows.Forms.Button();
            this.checkBox_ShowZoom = new System.Windows.Forms.CheckBox();
            this.label_ZoomedFieldColor = new System.Windows.Forms.Label();
            this.checkBox_ShowFilledFields = new System.Windows.Forms.CheckBox();
            this.textBox_ExecutionTime = new System.Windows.Forms.TextBox();
            this.label_ProcessingTime = new System.Windows.Forms.Label();
            this.tabControl_User = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_KeepPreProcessing = new System.Windows.Forms.CheckBox();
            this.button_PreProcess = new System.Windows.Forms.Button();
            this.groupBox_Thresholding = new System.Windows.Forms.GroupBox();
            this.numericUpDown_Otsu = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_threshold = new System.Windows.Forms.NumericUpDown();
            this.radioButton_Manual_thresholding = new System.Windows.Forms.RadioButton();
            this.radioButton_Otsu_thresholding = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox_UsedFeatures = new System.Windows.Forms.GroupBox();
            this.checkBox_circularity = new System.Windows.Forms.CheckBox();
            this.checkBox_aspect_ratio = new System.Windows.Forms.CheckBox();
            this.checkBox_max_diameter = new System.Windows.Forms.CheckBox();
            this.checkBox_min_diameter = new System.Windows.Forms.CheckBox();
            this.checkBox_grayscale = new System.Windows.Forms.CheckBox();
            this.checkBox_area = new System.Windows.Forms.CheckBox();
            this.checkBox_B_component = new System.Windows.Forms.CheckBox();
            this.checkBox_G_component = new System.Windows.Forms.CheckBox();
            this.checkBox_R_component = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox_CurrentTeacher = new System.Windows.Forms.TextBox();
            this.button_Next = new System.Windows.Forms.Button();
            this.button_Previous = new System.Windows.Forms.Button();
            this.checkBox_AddTeacher = new System.Windows.Forms.CheckBox();
            this.groupBox_TeacherType = new System.Windows.Forms.GroupBox();
            this.button_Teacher_Other_Color = new System.Windows.Forms.Button();
            this.radioButton_NotOurObject = new System.Windows.Forms.RadioButton();
            this.label_Name = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.button_AddName = new System.Windows.Forms.Button();
            this.radioButton_OurObject = new System.Windows.Forms.RadioButton();
            this.button_DeleteAll = new System.Windows.Forms.Button();
            this.button_DeleteSelected = new System.Windows.Forms.Button();
            this.button_RenameSelected = new System.Windows.Forms.Button();
            this.button_Teacher_Our_Color = new System.Windows.Forms.Button();
            this.groupBox_Counts = new System.Windows.Forms.GroupBox();
            this.radioButton_AllTeachers = new System.Windows.Forms.RadioButton();
            this.radioButton_CurrentTeachers = new System.Windows.Forms.RadioButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button_Teach = new System.Windows.Forms.Button();
            this.label_Threshold1 = new System.Windows.Forms.Label();
            this.numericUpDown_Confidence = new System.Windows.Forms.NumericUpDown();
            this.label_Threshold2 = new System.Windows.Forms.Label();
            this.groupBox_Layers = new System.Windows.Forms.GroupBox();
            this.textBox_outputunits = new System.Windows.Forms.TextBox();
            this.label_outputunits = new System.Windows.Forms.Label();
            this.numericUpDownNbofHiddenUnits = new System.Windows.Forms.NumericUpDown();
            this.label_hiddenunits = new System.Windows.Forms.Label();
            this.textBox_inputunits = new System.Windows.Forms.TextBox();
            this.label_inputunits = new System.Windows.Forms.Label();
            this.label_Position2 = new System.Windows.Forms.Label();
            this.button_Process = new System.Windows.Forms.Button();
            this.checkBox_KeepProcessing = new System.Windows.Forms.CheckBox();
            this.trackBar_Blending_Result = new System.Windows.Forms.TrackBar();
            this.label_NamesList = new System.Windows.Forms.Label();
            this.listView_Names = new System.Windows.Forms.ListView();
            this.columnHeader_NameValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_Help = new System.Windows.Forms.Button();
            this.groupBox_Images.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Blending_Fields)).BeginInit();
            this.groupBox_Show.SuspendLayout();
            this.groupBox_Colors.SuspendLayout();
            this.tabControl_User.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox_Thresholding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Otsu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox_UsedFeatures.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox_TeacherType.SuspendLayout();
            this.groupBox_Counts.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Confidence)).BeginInit();
            this.groupBox_Layers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNbofHiddenUnits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Blending_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Exit
            // 
            this.button_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Exit.Location = new System.Drawing.Point(1166, 594);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(40, 22);
            this.button_Exit.TabIndex = 0;
            this.button_Exit.Text = "Exit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // groupBox_Images
            // 
            this.groupBox_Images.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Images.Controls.Add(this.label_FileName);
            this.groupBox_Images.Controls.Add(this.button_BrowseImage);
            this.groupBox_Images.Controls.Add(this.textBox_ImageFileName);
            this.groupBox_Images.Controls.Add(this.textBox_ImageSize);
            this.groupBox_Images.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Images.Name = "groupBox_Images";
            this.groupBox_Images.Size = new System.Drawing.Size(1194, 54);
            this.groupBox_Images.TabIndex = 1;
            this.groupBox_Images.TabStop = false;
            this.groupBox_Images.Text = "Image File";
            // 
            // label_FileName
            // 
            this.label_FileName.AutoSize = true;
            this.label_FileName.Location = new System.Drawing.Point(16, 24);
            this.label_FileName.Name = "label_FileName";
            this.label_FileName.Size = new System.Drawing.Size(57, 13);
            this.label_FileName.TabIndex = 5;
            this.label_FileName.Text = "File Name:";
            // 
            // button_BrowseImage
            // 
            this.button_BrowseImage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_BrowseImage.Location = new System.Drawing.Point(1123, 16);
            this.button_BrowseImage.Name = "button_BrowseImage";
            this.button_BrowseImage.Size = new System.Drawing.Size(65, 28);
            this.button_BrowseImage.TabIndex = 4;
            this.button_BrowseImage.Text = "Browse";
            this.button_BrowseImage.UseVisualStyleBackColor = true;
            this.button_BrowseImage.Click += new System.EventHandler(this.button_BrowseImage_Click);
            // 
            // textBox_ImageFileName
            // 
            this.textBox_ImageFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ImageFileName.Location = new System.Drawing.Point(79, 20);
            this.textBox_ImageFileName.Name = "textBox_ImageFileName";
            this.textBox_ImageFileName.Size = new System.Drawing.Size(897, 20);
            this.textBox_ImageFileName.TabIndex = 2;
            this.textBox_ImageFileName.TextChanged += new System.EventHandler(this.textBox_ImageFileName_TextChanged);
            // 
            // textBox_ImageSize
            // 
            this.textBox_ImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ImageSize.Enabled = false;
            this.textBox_ImageSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ImageSize.Location = new System.Drawing.Point(982, 19);
            this.textBox_ImageSize.Name = "textBox_ImageSize";
            this.textBox_ImageSize.Size = new System.Drawing.Size(135, 20);
            this.textBox_ImageSize.TabIndex = 41;
            this.textBox_ImageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox_Image
            // 
            this.pictureBox_Image.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Image.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox_Image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Image.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox_Image.Location = new System.Drawing.Point(311, 72);
            this.pictureBox_Image.Name = "pictureBox_Image";
            this.pictureBox_Image.Size = new System.Drawing.Size(677, 504);
            this.pictureBox_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Image.TabIndex = 2;
            this.pictureBox_Image.TabStop = false;
            this.pictureBox_Image.WaitOnLoad = true;
            this.pictureBox_Image.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Image_Paint);
            this.pictureBox_Image.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_Image_MouseClick);
            this.pictureBox_Image.MouseLeave += new System.EventHandler(this.pictureBox_Image_MouseLeave);
            this.pictureBox_Image.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_Image_MouseMove);
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(1123, 594);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(40, 22);
            this.button_Save.TabIndex = 12;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // trackBar_Blending_Fields
            // 
            this.trackBar_Blending_Fields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_Blending_Fields.Location = new System.Drawing.Point(311, 587);
            this.trackBar_Blending_Fields.Maximum = 255;
            this.trackBar_Blending_Fields.Name = "trackBar_Blending_Fields";
            this.trackBar_Blending_Fields.Size = new System.Drawing.Size(384, 45);
            this.trackBar_Blending_Fields.TabIndex = 15;
            this.trackBar_Blending_Fields.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_Blending_Fields.Value = 127;
            this.trackBar_Blending_Fields.Scroll += new System.EventHandler(this.trackBar_Blending_Fields_Scroll);
            this.trackBar_Blending_Fields.MouseEnter += new System.EventHandler(this.trackBar_Blending_Fields_MouseEnter);
            // 
            // label_Position1
            // 
            this.label_Position1.AutoSize = true;
            this.label_Position1.Location = new System.Drawing.Point(32, 547);
            this.label_Position1.Name = "label_Position1";
            this.label_Position1.Size = new System.Drawing.Size(36, 13);
            this.label_Position1.TabIndex = 17;
            this.label_Position1.Text = "cursor";
            // 
            // textBox_Position
            // 
            this.textBox_Position.Enabled = false;
            this.textBox_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Position.Location = new System.Drawing.Point(25, 563);
            this.textBox_Position.Name = "textBox_Position";
            this.textBox_Position.Size = new System.Drawing.Size(92, 20);
            this.textBox_Position.TabIndex = 19;
            this.textBox_Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Zoom_Color
            // 
            this.button_Zoom_Color.BackColor = System.Drawing.Color.Yellow;
            this.button_Zoom_Color.Location = new System.Drawing.Point(102, 52);
            this.button_Zoom_Color.Name = "button_Zoom_Color";
            this.button_Zoom_Color.Size = new System.Drawing.Size(75, 23);
            this.button_Zoom_Color.TabIndex = 28;
            this.button_Zoom_Color.UseVisualStyleBackColor = false;
            this.button_Zoom_Color.Click += new System.EventHandler(this.button_Zoom_Color_Click);
            // 
            // timer_zoom_pan
            // 
            this.timer_zoom_pan.Tick += new System.EventHandler(this.timer_zoom_pan_Tick);
            // 
            // groupBox_Show
            // 
            this.groupBox_Show.Controls.Add(this.checkBox_Show_Thresholded);
            this.groupBox_Show.Controls.Add(this.groupBox_Colors);
            this.groupBox_Show.Location = new System.Drawing.Point(12, 393);
            this.groupBox_Show.Name = "groupBox_Show";
            this.groupBox_Show.Size = new System.Drawing.Size(289, 151);
            this.groupBox_Show.TabIndex = 33;
            this.groupBox_Show.TabStop = false;
            this.groupBox_Show.Text = "Show";
            // 
            // checkBox_Show_Thresholded
            // 
            this.checkBox_Show_Thresholded.AutoSize = true;
            this.checkBox_Show_Thresholded.Location = new System.Drawing.Point(80, 21);
            this.checkBox_Show_Thresholded.Name = "checkBox_Show_Thresholded";
            this.checkBox_Show_Thresholded.Size = new System.Drawing.Size(111, 17);
            this.checkBox_Show_Thresholded.TabIndex = 51;
            this.checkBox_Show_Thresholded.Text = "Show thresholded";
            this.checkBox_Show_Thresholded.UseVisualStyleBackColor = true;
            this.checkBox_Show_Thresholded.CheckedChanged += new System.EventHandler(this.checkBox_Show_Thresholded_CheckedChanged);
            // 
            // groupBox_Colors
            // 
            this.groupBox_Colors.Controls.Add(this.checkBox_ShowZoomThr);
            this.groupBox_Colors.Controls.Add(this.button_ZoomThr_Dummy);
            this.groupBox_Colors.Controls.Add(this.label_object);
            this.groupBox_Colors.Controls.Add(this.button_ObjectColor);
            this.groupBox_Colors.Controls.Add(this.checkBox_ShowZoom);
            this.groupBox_Colors.Controls.Add(this.label_ZoomedFieldColor);
            this.groupBox_Colors.Controls.Add(this.button_Zoom_Color);
            this.groupBox_Colors.Location = new System.Drawing.Point(12, 47);
            this.groupBox_Colors.Name = "groupBox_Colors";
            this.groupBox_Colors.Size = new System.Drawing.Size(271, 91);
            this.groupBox_Colors.TabIndex = 43;
            this.groupBox_Colors.TabStop = false;
            this.groupBox_Colors.Text = "Colors";
            // 
            // checkBox_ShowZoomThr
            // 
            this.checkBox_ShowZoomThr.AutoSize = true;
            this.checkBox_ShowZoomThr.Location = new System.Drawing.Point(201, 57);
            this.checkBox_ShowZoomThr.Name = "checkBox_ShowZoomThr";
            this.checkBox_ShowZoomThr.Size = new System.Drawing.Size(15, 14);
            this.checkBox_ShowZoomThr.TabIndex = 57;
            this.checkBox_ShowZoomThr.UseVisualStyleBackColor = true;
            this.checkBox_ShowZoomThr.CheckedChanged += new System.EventHandler(this.checkBox_ShowZoomThr_CheckedChanged);
            // 
            // button_ZoomThr_Dummy
            // 
            this.button_ZoomThr_Dummy.BackColor = System.Drawing.Color.Silver;
            this.button_ZoomThr_Dummy.Enabled = false;
            this.button_ZoomThr_Dummy.Location = new System.Drawing.Point(184, 52);
            this.button_ZoomThr_Dummy.Name = "button_ZoomThr_Dummy";
            this.button_ZoomThr_Dummy.Size = new System.Drawing.Size(75, 23);
            this.button_ZoomThr_Dummy.TabIndex = 56;
            this.button_ZoomThr_Dummy.UseVisualStyleBackColor = false;
            // 
            // label_object
            // 
            this.label_object.AutoSize = true;
            this.label_object.Location = new System.Drawing.Point(36, 23);
            this.label_object.Name = "label_object";
            this.label_object.Size = new System.Drawing.Size(57, 13);
            this.label_object.TabIndex = 53;
            this.label_object.Text = "our object:";
            // 
            // button_ObjectColor
            // 
            this.button_ObjectColor.BackColor = System.Drawing.Color.Lime;
            this.button_ObjectColor.Location = new System.Drawing.Point(102, 18);
            this.button_ObjectColor.Name = "button_ObjectColor";
            this.button_ObjectColor.Size = new System.Drawing.Size(75, 23);
            this.button_ObjectColor.TabIndex = 52;
            this.button_ObjectColor.UseVisualStyleBackColor = false;
            this.button_ObjectColor.Click += new System.EventHandler(this.button_ObjectColor_Click);
            // 
            // checkBox_ShowZoom
            // 
            this.checkBox_ShowZoom.AutoSize = true;
            this.checkBox_ShowZoom.Location = new System.Drawing.Point(118, 57);
            this.checkBox_ShowZoom.Name = "checkBox_ShowZoom";
            this.checkBox_ShowZoom.Size = new System.Drawing.Size(15, 14);
            this.checkBox_ShowZoom.TabIndex = 51;
            this.checkBox_ShowZoom.UseVisualStyleBackColor = true;
            this.checkBox_ShowZoom.CheckedChanged += new System.EventHandler(this.checkBox_ShowZoom_CheckedChanged);
            // 
            // label_ZoomedFieldColor
            // 
            this.label_ZoomedFieldColor.AutoSize = true;
            this.label_ZoomedFieldColor.Location = new System.Drawing.Point(27, 57);
            this.label_ZoomedFieldColor.Name = "label_ZoomedFieldColor";
            this.label_ZoomedFieldColor.Size = new System.Drawing.Size(69, 13);
            this.label_ZoomedFieldColor.TabIndex = 43;
            this.label_ZoomedFieldColor.Text = "zoomed field:";
            // 
            // checkBox_ShowFilledFields
            // 
            this.checkBox_ShowFilledFields.AutoSize = true;
            this.checkBox_ShowFilledFields.Location = new System.Drawing.Point(229, 252);
            this.checkBox_ShowFilledFields.Name = "checkBox_ShowFilledFields";
            this.checkBox_ShowFilledFields.Size = new System.Drawing.Size(47, 17);
            this.checkBox_ShowFilledFields.TabIndex = 44;
            this.checkBox_ShowFilledFields.Text = "filled";
            this.checkBox_ShowFilledFields.UseVisualStyleBackColor = true;
            this.checkBox_ShowFilledFields.CheckedChanged += new System.EventHandler(this.checkBox_ShowFilledFields_CheckedChanged);
            // 
            // textBox_ExecutionTime
            // 
            this.textBox_ExecutionTime.Enabled = false;
            this.textBox_ExecutionTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ExecutionTime.Location = new System.Drawing.Point(225, 563);
            this.textBox_ExecutionTime.Name = "textBox_ExecutionTime";
            this.textBox_ExecutionTime.Size = new System.Drawing.Size(75, 20);
            this.textBox_ExecutionTime.TabIndex = 34;
            this.textBox_ExecutionTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_ProcessingTime
            // 
            this.label_ProcessingTime.AutoSize = true;
            this.label_ProcessingTime.Location = new System.Drawing.Point(192, 547);
            this.label_ProcessingTime.Name = "label_ProcessingTime";
            this.label_ProcessingTime.Size = new System.Drawing.Size(109, 13);
            this.label_ProcessingTime.TabIndex = 36;
            this.label_ProcessingTime.Text = "processing time (sec):";
            // 
            // tabControl_User
            // 
            this.tabControl_User.Controls.Add(this.tabPage1);
            this.tabControl_User.Controls.Add(this.tabPage2);
            this.tabControl_User.Controls.Add(this.tabPage3);
            this.tabControl_User.Controls.Add(this.tabPage4);
            this.tabControl_User.Location = new System.Drawing.Point(12, 72);
            this.tabControl_User.Name = "tabControl_User";
            this.tabControl_User.SelectedIndex = 0;
            this.tabControl_User.Size = new System.Drawing.Size(293, 315);
            this.tabControl_User.TabIndex = 48;
            this.tabControl_User.SelectedIndexChanged += new System.EventHandler(this.tabControl_User_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox_KeepPreProcessing);
            this.tabPage1.Controls.Add(this.button_PreProcess);
            this.tabPage1.Controls.Add(this.groupBox_Thresholding);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(285, 289);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PreProcessing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_KeepPreProcessing
            // 
            this.checkBox_KeepPreProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_KeepPreProcessing.AutoSize = true;
            this.checkBox_KeepPreProcessing.Checked = true;
            this.checkBox_KeepPreProcessing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_KeepPreProcessing.Location = new System.Drawing.Point(155, 164);
            this.checkBox_KeepPreProcessing.Name = "checkBox_KeepPreProcessing";
            this.checkBox_KeepPreProcessing.Size = new System.Drawing.Size(15, 14);
            this.checkBox_KeepPreProcessing.TabIndex = 52;
            this.checkBox_KeepPreProcessing.UseVisualStyleBackColor = true;
            this.checkBox_KeepPreProcessing.CheckedChanged += new System.EventHandler(this.checkBox_KeepPreProcessing_CheckedChanged);
            // 
            // button_PreProcess
            // 
            this.button_PreProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_PreProcess.BackColor = System.Drawing.Color.LimeGreen;
            this.button_PreProcess.Location = new System.Drawing.Point(146, 159);
            this.button_PreProcess.Name = "button_PreProcess";
            this.button_PreProcess.Size = new System.Drawing.Size(104, 22);
            this.button_PreProcess.TabIndex = 51;
            this.button_PreProcess.Text = "     Preprocess";
            this.button_PreProcess.UseVisualStyleBackColor = false;
            this.button_PreProcess.Click += new System.EventHandler(this.button_PreProcess_Click);
            // 
            // groupBox_Thresholding
            // 
            this.groupBox_Thresholding.Controls.Add(this.numericUpDown_Otsu);
            this.groupBox_Thresholding.Controls.Add(this.numericUpDown_threshold);
            this.groupBox_Thresholding.Controls.Add(this.radioButton_Manual_thresholding);
            this.groupBox_Thresholding.Controls.Add(this.radioButton_Otsu_thresholding);
            this.groupBox_Thresholding.Location = new System.Drawing.Point(9, 15);
            this.groupBox_Thresholding.Name = "groupBox_Thresholding";
            this.groupBox_Thresholding.Size = new System.Drawing.Size(263, 96);
            this.groupBox_Thresholding.TabIndex = 1;
            this.groupBox_Thresholding.TabStop = false;
            this.groupBox_Thresholding.Text = "Thresholding";
            // 
            // numericUpDown_Otsu
            // 
            this.numericUpDown_Otsu.Location = new System.Drawing.Point(184, 26);
            this.numericUpDown_Otsu.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown_Otsu.Name = "numericUpDown_Otsu";
            this.numericUpDown_Otsu.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown_Otsu.TabIndex = 3;
            this.numericUpDown_Otsu.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_Otsu.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown_Otsu.ValueChanged += new System.EventHandler(this.numericUpDown_Otsu_ValueChanged);
            // 
            // numericUpDown_threshold
            // 
            this.numericUpDown_threshold.Location = new System.Drawing.Point(184, 54);
            this.numericUpDown_threshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_threshold.Name = "numericUpDown_threshold";
            this.numericUpDown_threshold.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown_threshold.TabIndex = 2;
            this.numericUpDown_threshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_threshold.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown_threshold.ValueChanged += new System.EventHandler(this.numericUpDown_threshold_ValueChanged);
            // 
            // radioButton_Manual_thresholding
            // 
            this.radioButton_Manual_thresholding.AutoSize = true;
            this.radioButton_Manual_thresholding.Location = new System.Drawing.Point(19, 55);
            this.radioButton_Manual_thresholding.Name = "radioButton_Manual_thresholding";
            this.radioButton_Manual_thresholding.Size = new System.Drawing.Size(123, 17);
            this.radioButton_Manual_thresholding.TabIndex = 1;
            this.radioButton_Manual_thresholding.Text = "Manual thresholding:";
            this.radioButton_Manual_thresholding.UseVisualStyleBackColor = true;
            this.radioButton_Manual_thresholding.CheckedChanged += new System.EventHandler(this.radioButton_Manual_thresholding_CheckedChanged);
            // 
            // radioButton_Otsu_thresholding
            // 
            this.radioButton_Otsu_thresholding.AutoSize = true;
            this.radioButton_Otsu_thresholding.Checked = true;
            this.radioButton_Otsu_thresholding.Location = new System.Drawing.Point(19, 26);
            this.radioButton_Otsu_thresholding.Name = "radioButton_Otsu_thresholding";
            this.radioButton_Otsu_thresholding.Size = new System.Drawing.Size(162, 17);
            this.radioButton_Otsu_thresholding.TabIndex = 0;
            this.radioButton_Otsu_thresholding.TabStop = true;
            this.radioButton_Otsu_thresholding.Text = "Otsu (automatic) thresholding";
            this.radioButton_Otsu_thresholding.UseVisualStyleBackColor = true;
            this.radioButton_Otsu_thresholding.CheckedChanged += new System.EventHandler(this.radioButton_Otsu_thresholding_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox_UsedFeatures);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(285, 289);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Features";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox_UsedFeatures
            // 
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_circularity);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_aspect_ratio);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_max_diameter);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_min_diameter);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_grayscale);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_area);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_B_component);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_G_component);
            this.groupBox_UsedFeatures.Controls.Add(this.checkBox_R_component);
            this.groupBox_UsedFeatures.Location = new System.Drawing.Point(9, 17);
            this.groupBox_UsedFeatures.Name = "groupBox_UsedFeatures";
            this.groupBox_UsedFeatures.Size = new System.Drawing.Size(264, 248);
            this.groupBox_UsedFeatures.TabIndex = 0;
            this.groupBox_UsedFeatures.TabStop = false;
            this.groupBox_UsedFeatures.Text = "Used Features";
            // 
            // checkBox_circularity
            // 
            this.checkBox_circularity.AutoSize = true;
            this.checkBox_circularity.Location = new System.Drawing.Point(51, 214);
            this.checkBox_circularity.Name = "checkBox_circularity";
            this.checkBox_circularity.Size = new System.Drawing.Size(70, 17);
            this.checkBox_circularity.TabIndex = 15;
            this.checkBox_circularity.Text = "circularity";
            this.checkBox_circularity.UseVisualStyleBackColor = true;
            this.checkBox_circularity.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_aspect_ratio
            // 
            this.checkBox_aspect_ratio.AutoSize = true;
            this.checkBox_aspect_ratio.Location = new System.Drawing.Point(51, 191);
            this.checkBox_aspect_ratio.Name = "checkBox_aspect_ratio";
            this.checkBox_aspect_ratio.Size = new System.Drawing.Size(81, 17);
            this.checkBox_aspect_ratio.TabIndex = 14;
            this.checkBox_aspect_ratio.Text = "aspect ratio";
            this.checkBox_aspect_ratio.UseVisualStyleBackColor = true;
            this.checkBox_aspect_ratio.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_max_diameter
            // 
            this.checkBox_max_diameter.AutoSize = true;
            this.checkBox_max_diameter.Location = new System.Drawing.Point(51, 168);
            this.checkBox_max_diameter.Name = "checkBox_max_diameter";
            this.checkBox_max_diameter.Size = new System.Drawing.Size(124, 17);
            this.checkBox_max_diameter.TabIndex = 13;
            this.checkBox_max_diameter.Text = "maximum of diameter";
            this.checkBox_max_diameter.UseVisualStyleBackColor = true;
            this.checkBox_max_diameter.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_min_diameter
            // 
            this.checkBox_min_diameter.AutoSize = true;
            this.checkBox_min_diameter.Location = new System.Drawing.Point(51, 145);
            this.checkBox_min_diameter.Name = "checkBox_min_diameter";
            this.checkBox_min_diameter.Size = new System.Drawing.Size(121, 17);
            this.checkBox_min_diameter.TabIndex = 12;
            this.checkBox_min_diameter.Text = "minimum of diameter";
            this.checkBox_min_diameter.UseVisualStyleBackColor = true;
            this.checkBox_min_diameter.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_grayscale
            // 
            this.checkBox_grayscale.AutoSize = true;
            this.checkBox_grayscale.Location = new System.Drawing.Point(51, 99);
            this.checkBox_grayscale.Name = "checkBox_grayscale";
            this.checkBox_grayscale.Size = new System.Drawing.Size(176, 17);
            this.checkBox_grayscale.TabIndex = 11;
            this.checkBox_grayscale.Text = "average of (R,G,B) components";
            this.checkBox_grayscale.UseVisualStyleBackColor = true;
            this.checkBox_grayscale.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_area
            // 
            this.checkBox_area.AutoSize = true;
            this.checkBox_area.Location = new System.Drawing.Point(51, 122);
            this.checkBox_area.Name = "checkBox_area";
            this.checkBox_area.Size = new System.Drawing.Size(47, 17);
            this.checkBox_area.TabIndex = 10;
            this.checkBox_area.Text = "area";
            this.checkBox_area.UseVisualStyleBackColor = true;
            this.checkBox_area.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_B_component
            // 
            this.checkBox_B_component.AutoSize = true;
            this.checkBox_B_component.Location = new System.Drawing.Point(51, 76);
            this.checkBox_B_component.Name = "checkBox_B_component";
            this.checkBox_B_component.Size = new System.Drawing.Size(154, 17);
            this.checkBox_B_component.TabIndex = 9;
            this.checkBox_B_component.Text = "B component of pixel value";
            this.checkBox_B_component.UseVisualStyleBackColor = true;
            this.checkBox_B_component.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_G_component
            // 
            this.checkBox_G_component.AutoSize = true;
            this.checkBox_G_component.Location = new System.Drawing.Point(51, 53);
            this.checkBox_G_component.Name = "checkBox_G_component";
            this.checkBox_G_component.Size = new System.Drawing.Size(155, 17);
            this.checkBox_G_component.TabIndex = 8;
            this.checkBox_G_component.Text = "G component of pixel value";
            this.checkBox_G_component.UseVisualStyleBackColor = true;
            this.checkBox_G_component.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // checkBox_R_component
            // 
            this.checkBox_R_component.AutoSize = true;
            this.checkBox_R_component.Location = new System.Drawing.Point(50, 30);
            this.checkBox_R_component.Name = "checkBox_R_component";
            this.checkBox_R_component.Size = new System.Drawing.Size(155, 17);
            this.checkBox_R_component.TabIndex = 7;
            this.checkBox_R_component.Text = "R component of pixel value";
            this.checkBox_R_component.UseVisualStyleBackColor = true;
            this.checkBox_R_component.CheckedChanged += new System.EventHandler(this.checkBox_UsedFeatureChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox_CurrentTeacher);
            this.tabPage3.Controls.Add(this.button_Next);
            this.tabPage3.Controls.Add(this.button_Previous);
            this.tabPage3.Controls.Add(this.checkBox_AddTeacher);
            this.tabPage3.Controls.Add(this.groupBox_TeacherType);
            this.tabPage3.Controls.Add(this.groupBox_Counts);
            this.tabPage3.Controls.Add(this.checkBox_ShowFilledFields);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(285, 289);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Teachers";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox_CurrentTeacher
            // 
            this.textBox_CurrentTeacher.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_CurrentTeacher.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_CurrentTeacher.Location = new System.Drawing.Point(136, 248);
            this.textBox_CurrentTeacher.Name = "textBox_CurrentTeacher";
            this.textBox_CurrentTeacher.ReadOnly = true;
            this.textBox_CurrentTeacher.Size = new System.Drawing.Size(44, 26);
            this.textBox_CurrentTeacher.TabIndex = 41;
            this.textBox_CurrentTeacher.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Next
            // 
            this.button_Next.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_Next.Location = new System.Drawing.Point(180, 249);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(31, 23);
            this.button_Next.TabIndex = 43;
            this.button_Next.Text = ">";
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // button_Previous
            // 
            this.button_Previous.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_Previous.Location = new System.Drawing.Point(104, 248);
            this.button_Previous.Name = "button_Previous";
            this.button_Previous.Size = new System.Drawing.Size(31, 23);
            this.button_Previous.TabIndex = 42;
            this.button_Previous.Text = "<";
            this.button_Previous.UseVisualStyleBackColor = true;
            this.button_Previous.Click += new System.EventHandler(this.button_Previous_Click);
            // 
            // checkBox_AddTeacher
            // 
            this.checkBox_AddTeacher.AutoSize = true;
            this.checkBox_AddTeacher.Enabled = false;
            this.checkBox_AddTeacher.Location = new System.Drawing.Point(9, 254);
            this.checkBox_AddTeacher.Name = "checkBox_AddTeacher";
            this.checkBox_AddTeacher.Size = new System.Drawing.Size(88, 17);
            this.checkBox_AddTeacher.TabIndex = 40;
            this.checkBox_AddTeacher.Text = "Add Teacher";
            this.checkBox_AddTeacher.UseVisualStyleBackColor = true;
            this.checkBox_AddTeacher.CheckedChanged += new System.EventHandler(this.checkBox_AddTeacher_CheckedChanged);
            // 
            // groupBox_TeacherType
            // 
            this.groupBox_TeacherType.Controls.Add(this.button_Teacher_Other_Color);
            this.groupBox_TeacherType.Controls.Add(this.radioButton_NotOurObject);
            this.groupBox_TeacherType.Controls.Add(this.label_Name);
            this.groupBox_TeacherType.Controls.Add(this.textBox_Name);
            this.groupBox_TeacherType.Controls.Add(this.button_AddName);
            this.groupBox_TeacherType.Controls.Add(this.radioButton_OurObject);
            this.groupBox_TeacherType.Controls.Add(this.button_DeleteAll);
            this.groupBox_TeacherType.Controls.Add(this.button_DeleteSelected);
            this.groupBox_TeacherType.Controls.Add(this.button_RenameSelected);
            this.groupBox_TeacherType.Controls.Add(this.button_Teacher_Our_Color);
            this.groupBox_TeacherType.Location = new System.Drawing.Point(9, 72);
            this.groupBox_TeacherType.Name = "groupBox_TeacherType";
            this.groupBox_TeacherType.Size = new System.Drawing.Size(267, 156);
            this.groupBox_TeacherType.TabIndex = 39;
            this.groupBox_TeacherType.TabStop = false;
            this.groupBox_TeacherType.Text = "Type";
            // 
            // button_Teacher_Other_Color
            // 
            this.button_Teacher_Other_Color.BackColor = System.Drawing.Color.SkyBlue;
            this.button_Teacher_Other_Color.Location = new System.Drawing.Point(205, 48);
            this.button_Teacher_Other_Color.Name = "button_Teacher_Other_Color";
            this.button_Teacher_Other_Color.Size = new System.Drawing.Size(49, 23);
            this.button_Teacher_Other_Color.TabIndex = 34;
            this.button_Teacher_Other_Color.UseVisualStyleBackColor = false;
            this.button_Teacher_Other_Color.Click += new System.EventHandler(this.button_Teacher_Other_Color_Click);
            // 
            // radioButton_NotOurObject
            // 
            this.radioButton_NotOurObject.AutoSize = true;
            this.radioButton_NotOurObject.Location = new System.Drawing.Point(133, 50);
            this.radioButton_NotOurObject.Name = "radioButton_NotOurObject";
            this.radioButton_NotOurObject.Size = new System.Drawing.Size(63, 17);
            this.radioButton_NotOurObject.TabIndex = 33;
            this.radioButton_NotOurObject.Text = "OTHER";
            this.radioButton_NotOurObject.UseVisualStyleBackColor = true;
            // 
            // label_Name
            // 
            this.label_Name.AutoSize = true;
            this.label_Name.Location = new System.Drawing.Point(11, 21);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(38, 13);
            this.label_Name.TabIndex = 31;
            this.label_Name.Text = "Name:";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(52, 18);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(203, 20);
            this.textBox_Name.TabIndex = 30;
            this.textBox_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_AddName
            // 
            this.button_AddName.Location = new System.Drawing.Point(15, 83);
            this.button_AddName.Name = "button_AddName";
            this.button_AddName.Size = new System.Drawing.Size(105, 23);
            this.button_AddName.TabIndex = 32;
            this.button_AddName.Text = "Add";
            this.button_AddName.UseVisualStyleBackColor = true;
            this.button_AddName.Click += new System.EventHandler(this.button_AddName_Click);
            // 
            // radioButton_OurObject
            // 
            this.radioButton_OurObject.AutoSize = true;
            this.radioButton_OurObject.BackColor = System.Drawing.Color.Transparent;
            this.radioButton_OurObject.Checked = true;
            this.radioButton_OurObject.Location = new System.Drawing.Point(10, 51);
            this.radioButton_OurObject.Name = "radioButton_OurObject";
            this.radioButton_OurObject.Size = new System.Drawing.Size(49, 17);
            this.radioButton_OurObject.TabIndex = 32;
            this.radioButton_OurObject.TabStop = true;
            this.radioButton_OurObject.Text = "OUR";
            this.radioButton_OurObject.UseVisualStyleBackColor = false;
            // 
            // button_DeleteAll
            // 
            this.button_DeleteAll.Location = new System.Drawing.Point(144, 121);
            this.button_DeleteAll.Name = "button_DeleteAll";
            this.button_DeleteAll.Size = new System.Drawing.Size(108, 23);
            this.button_DeleteAll.TabIndex = 35;
            this.button_DeleteAll.Text = "Delete All";
            this.button_DeleteAll.UseVisualStyleBackColor = true;
            this.button_DeleteAll.Click += new System.EventHandler(this.button_DeleteAll_Click);
            // 
            // button_DeleteSelected
            // 
            this.button_DeleteSelected.Enabled = false;
            this.button_DeleteSelected.Location = new System.Drawing.Point(15, 121);
            this.button_DeleteSelected.Name = "button_DeleteSelected";
            this.button_DeleteSelected.Size = new System.Drawing.Size(105, 23);
            this.button_DeleteSelected.TabIndex = 34;
            this.button_DeleteSelected.Text = "Delete Selected";
            this.button_DeleteSelected.UseVisualStyleBackColor = true;
            this.button_DeleteSelected.Click += new System.EventHandler(this.button_DeleteSelected_Click);
            // 
            // button_RenameSelected
            // 
            this.button_RenameSelected.Enabled = false;
            this.button_RenameSelected.Location = new System.Drawing.Point(144, 83);
            this.button_RenameSelected.Name = "button_RenameSelected";
            this.button_RenameSelected.Size = new System.Drawing.Size(108, 23);
            this.button_RenameSelected.TabIndex = 33;
            this.button_RenameSelected.Text = "Rename Selected";
            this.button_RenameSelected.UseVisualStyleBackColor = true;
            this.button_RenameSelected.Click += new System.EventHandler(this.button_RenameSelected_Click);
            // 
            // button_Teacher_Our_Color
            // 
            this.button_Teacher_Our_Color.BackColor = System.Drawing.Color.Yellow;
            this.button_Teacher_Our_Color.Location = new System.Drawing.Point(71, 49);
            this.button_Teacher_Our_Color.Name = "button_Teacher_Our_Color";
            this.button_Teacher_Our_Color.Size = new System.Drawing.Size(49, 23);
            this.button_Teacher_Our_Color.TabIndex = 16;
            this.button_Teacher_Our_Color.UseVisualStyleBackColor = false;
            this.button_Teacher_Our_Color.Click += new System.EventHandler(this.button_Teacher_Our_Color_Click);
            // 
            // groupBox_Counts
            // 
            this.groupBox_Counts.Controls.Add(this.radioButton_AllTeachers);
            this.groupBox_Counts.Controls.Add(this.radioButton_CurrentTeachers);
            this.groupBox_Counts.Location = new System.Drawing.Point(9, 11);
            this.groupBox_Counts.Name = "groupBox_Counts";
            this.groupBox_Counts.Size = new System.Drawing.Size(267, 52);
            this.groupBox_Counts.TabIndex = 38;
            this.groupBox_Counts.TabStop = false;
            this.groupBox_Counts.Text = "Show counts in list of Teachers";
            // 
            // radioButton_AllTeachers
            // 
            this.radioButton_AllTeachers.AutoSize = true;
            this.radioButton_AllTeachers.Location = new System.Drawing.Point(153, 21);
            this.radioButton_AllTeachers.Name = "radioButton_AllTeachers";
            this.radioButton_AllTeachers.Size = new System.Drawing.Size(86, 17);
            this.radioButton_AllTeachers.TabIndex = 21;
            this.radioButton_AllTeachers.Text = "on all images";
            this.radioButton_AllTeachers.UseVisualStyleBackColor = true;
            this.radioButton_AllTeachers.CheckedChanged += new System.EventHandler(this.radioButton_AllTeachers_CheckedChanged);
            // 
            // radioButton_CurrentTeachers
            // 
            this.radioButton_CurrentTeachers.AutoSize = true;
            this.radioButton_CurrentTeachers.Checked = true;
            this.radioButton_CurrentTeachers.Location = new System.Drawing.Point(28, 21);
            this.radioButton_CurrentTeachers.Name = "radioButton_CurrentTeachers";
            this.radioButton_CurrentTeachers.Size = new System.Drawing.Size(107, 17);
            this.radioButton_CurrentTeachers.TabIndex = 20;
            this.radioButton_CurrentTeachers.TabStop = true;
            this.radioButton_CurrentTeachers.Text = " on current image";
            this.radioButton_CurrentTeachers.UseVisualStyleBackColor = true;
            this.radioButton_CurrentTeachers.CheckedChanged += new System.EventHandler(this.radioButton_CurrentTeachers_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button_Teach);
            this.tabPage4.Controls.Add(this.label_Threshold1);
            this.tabPage4.Controls.Add(this.numericUpDown_Confidence);
            this.tabPage4.Controls.Add(this.label_Threshold2);
            this.tabPage4.Controls.Add(this.groupBox_Layers);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(285, 289);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Neural Network";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button_Teach
            // 
            this.button_Teach.BackColor = System.Drawing.Color.Transparent;
            this.button_Teach.Location = new System.Drawing.Point(159, 240);
            this.button_Teach.Name = "button_Teach";
            this.button_Teach.Size = new System.Drawing.Size(101, 23);
            this.button_Teach.TabIndex = 37;
            this.button_Teach.Text = "Teach !";
            this.button_Teach.UseVisualStyleBackColor = false;
            this.button_Teach.Click += new System.EventHandler(this.button_Teach_Click);
            // 
            // label_Threshold1
            // 
            this.label_Threshold1.AutoSize = true;
            this.label_Threshold1.Location = new System.Drawing.Point(47, 174);
            this.label_Threshold1.Name = "label_Threshold1";
            this.label_Threshold1.Size = new System.Drawing.Size(116, 13);
            this.label_Threshold1.TabIndex = 35;
            this.label_Threshold1.Text = "Threshold of accepting";
            // 
            // numericUpDown_Confidence
            // 
            this.numericUpDown_Confidence.DecimalPlaces = 1;
            this.numericUpDown_Confidence.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_Confidence.Location = new System.Drawing.Point(176, 185);
            this.numericUpDown_Confidence.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDown_Confidence.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_Confidence.Name = "numericUpDown_Confidence";
            this.numericUpDown_Confidence.Size = new System.Drawing.Size(59, 20);
            this.numericUpDown_Confidence.TabIndex = 34;
            this.numericUpDown_Confidence.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_Confidence.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            this.numericUpDown_Confidence.ValueChanged += new System.EventHandler(this.numericUpDown_Confidence_ValueChanged);
            // 
            // label_Threshold2
            // 
            this.label_Threshold2.AutoSize = true;
            this.label_Threshold2.Location = new System.Drawing.Point(54, 198);
            this.label_Threshold2.Name = "label_Threshold2";
            this.label_Threshold2.Size = new System.Drawing.Size(109, 13);
            this.label_Threshold2.TabIndex = 36;
            this.label_Threshold2.Text = "the decision (0.1-1.0):";
            // 
            // groupBox_Layers
            // 
            this.groupBox_Layers.Controls.Add(this.textBox_outputunits);
            this.groupBox_Layers.Controls.Add(this.label_outputunits);
            this.groupBox_Layers.Controls.Add(this.numericUpDownNbofHiddenUnits);
            this.groupBox_Layers.Controls.Add(this.label_hiddenunits);
            this.groupBox_Layers.Controls.Add(this.textBox_inputunits);
            this.groupBox_Layers.Controls.Add(this.label_inputunits);
            this.groupBox_Layers.Location = new System.Drawing.Point(15, 16);
            this.groupBox_Layers.Name = "groupBox_Layers";
            this.groupBox_Layers.Size = new System.Drawing.Size(248, 134);
            this.groupBox_Layers.TabIndex = 33;
            this.groupBox_Layers.TabStop = false;
            this.groupBox_Layers.Text = "Layers";
            // 
            // textBox_outputunits
            // 
            this.textBox_outputunits.Enabled = false;
            this.textBox_outputunits.Location = new System.Drawing.Point(161, 96);
            this.textBox_outputunits.Name = "textBox_outputunits";
            this.textBox_outputunits.Size = new System.Drawing.Size(43, 20);
            this.textBox_outputunits.TabIndex = 33;
            this.textBox_outputunits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_outputunits
            // 
            this.label_outputunits.AutoSize = true;
            this.label_outputunits.Location = new System.Drawing.Point(32, 99);
            this.label_outputunits.Name = "label_outputunits";
            this.label_outputunits.Size = new System.Drawing.Size(117, 13);
            this.label_outputunits.TabIndex = 24;
            this.label_outputunits.Text = "Number of output units:";
            // 
            // numericUpDownNbofHiddenUnits
            // 
            this.numericUpDownNbofHiddenUnits.Location = new System.Drawing.Point(161, 59);
            this.numericUpDownNbofHiddenUnits.Name = "numericUpDownNbofHiddenUnits";
            this.numericUpDownNbofHiddenUnits.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownNbofHiddenUnits.TabIndex = 24;
            this.numericUpDownNbofHiddenUnits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownNbofHiddenUnits.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownNbofHiddenUnits.ValueChanged += new System.EventHandler(this.numericUpDownNbofHiddenUnits_ValueChanged);
            // 
            // label_hiddenunits
            // 
            this.label_hiddenunits.AutoSize = true;
            this.label_hiddenunits.Location = new System.Drawing.Point(32, 62);
            this.label_hiddenunits.Name = "label_hiddenunits";
            this.label_hiddenunits.Size = new System.Drawing.Size(119, 13);
            this.label_hiddenunits.TabIndex = 22;
            this.label_hiddenunits.Text = "Number of hidden units:";
            // 
            // textBox_inputunits
            // 
            this.textBox_inputunits.Enabled = false;
            this.textBox_inputunits.Location = new System.Drawing.Point(161, 22);
            this.textBox_inputunits.Name = "textBox_inputunits";
            this.textBox_inputunits.Size = new System.Drawing.Size(43, 20);
            this.textBox_inputunits.TabIndex = 32;
            this.textBox_inputunits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_inputunits
            // 
            this.label_inputunits.AutoSize = true;
            this.label_inputunits.Location = new System.Drawing.Point(41, 25);
            this.label_inputunits.Name = "label_inputunits";
            this.label_inputunits.Size = new System.Drawing.Size(110, 13);
            this.label_inputunits.TabIndex = 23;
            this.label_inputunits.Text = "Number of input units:";
            // 
            // label_Position2
            // 
            this.label_Position2.AutoSize = true;
            this.label_Position2.Location = new System.Drawing.Point(68, 547);
            this.label_Position2.Name = "label_Position2";
            this.label_Position2.Size = new System.Drawing.Size(46, 13);
            this.label_Position2.TabIndex = 49;
            this.label_Position2.Text = "position:";
            // 
            // button_Process
            // 
            this.button_Process.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Process.BackColor = System.Drawing.Color.LimeGreen;
            this.button_Process.Location = new System.Drawing.Point(1052, 594);
            this.button_Process.Name = "button_Process";
            this.button_Process.Size = new System.Drawing.Size(68, 22);
            this.button_Process.TabIndex = 50;
            this.button_Process.Text = "     Process";
            this.button_Process.UseVisualStyleBackColor = false;
            this.button_Process.Click += new System.EventHandler(this.button_Process_Click);
            // 
            // checkBox_KeepProcessing
            // 
            this.checkBox_KeepProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_KeepProcessing.AutoSize = true;
            this.checkBox_KeepProcessing.Location = new System.Drawing.Point(1058, 598);
            this.checkBox_KeepProcessing.Name = "checkBox_KeepProcessing";
            this.checkBox_KeepProcessing.Size = new System.Drawing.Size(15, 14);
            this.checkBox_KeepProcessing.TabIndex = 51;
            this.checkBox_KeepProcessing.UseVisualStyleBackColor = true;
            this.checkBox_KeepProcessing.CheckedChanged += new System.EventHandler(this.checkBox_KeepProcessing_CheckedChanged);
            // 
            // trackBar_Blending_Result
            // 
            this.trackBar_Blending_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_Blending_Result.Location = new System.Drawing.Point(697, 587);
            this.trackBar_Blending_Result.Maximum = 100;
            this.trackBar_Blending_Result.Name = "trackBar_Blending_Result";
            this.trackBar_Blending_Result.Size = new System.Drawing.Size(289, 45);
            this.trackBar_Blending_Result.TabIndex = 52;
            this.trackBar_Blending_Result.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_Blending_Result.Value = 50;
            this.trackBar_Blending_Result.Scroll += new System.EventHandler(this.trackBar_Blending_Result_Scroll);
            this.trackBar_Blending_Result.MouseEnter += new System.EventHandler(this.trackBar_Blending_Result_MouseEnter);
            // 
            // label_NamesList
            // 
            this.label_NamesList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_NamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NamesList.Location = new System.Drawing.Point(1033, 72);
            this.label_NamesList.Name = "label_NamesList";
            this.label_NamesList.Size = new System.Drawing.Size(149, 24);
            this.label_NamesList.TabIndex = 53;
            this.label_NamesList.Text = "Teachers";
            this.label_NamesList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listView_Names
            // 
            this.listView_Names.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Names.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_NameValue,
            this.columnHeader_Count,
            this.columnHeader_Type});
            this.listView_Names.FullRowSelect = true;
            this.listView_Names.GridLines = true;
            this.listView_Names.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_Names.HideSelection = false;
            this.listView_Names.Location = new System.Drawing.Point(999, 107);
            this.listView_Names.MultiSelect = false;
            this.listView_Names.Name = "listView_Names";
            this.listView_Names.Size = new System.Drawing.Size(207, 469);
            this.listView_Names.TabIndex = 54;
            this.listView_Names.UseCompatibleStateImageBehavior = false;
            this.listView_Names.View = System.Windows.Forms.View.Details;
            this.listView_Names.SelectedIndexChanged += new System.EventHandler(this.listView_Names_SelectedIndexChanged);
            // 
            // columnHeader_NameValue
            // 
            this.columnHeader_NameValue.Text = "Name:";
            this.columnHeader_NameValue.Width = 90;
            // 
            // columnHeader_Count
            // 
            this.columnHeader_Count.Text = "Count:";
            // 
            // columnHeader_Type
            // 
            this.columnHeader_Type.Text = "Type:";
            this.columnHeader_Type.Width = 50;
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.BackColor = System.Drawing.Color.Gold;
            this.button_Help.Location = new System.Drawing.Point(1001, 594);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(49, 22);
            this.button_Help.TabIndex = 55;
            this.button_Help.Text = "Help";
            this.button_Help.UseVisualStyleBackColor = false;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1218, 630);
            this.Controls.Add(this.button_Help);
            this.Controls.Add(this.listView_Names);
            this.Controls.Add(this.label_NamesList);
            this.Controls.Add(this.textBox_ExecutionTime);
            this.Controls.Add(this.label_ProcessingTime);
            this.Controls.Add(this.trackBar_Blending_Result);
            this.Controls.Add(this.checkBox_KeepProcessing);
            this.Controls.Add(this.button_Process);
            this.Controls.Add(this.label_Position2);
            this.Controls.Add(this.tabControl_User);
            this.Controls.Add(this.groupBox_Show);
            this.Controls.Add(this.pictureBox_Image);
            this.Controls.Add(this.textBox_Position);
            this.Controls.Add(this.label_Position1);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.groupBox_Images);
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.trackBar_Blending_Fields);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(1059, 664);
            this.Name = "Form1";
            this.Text = "AutoSegment - Development Environment   ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox_Images.ResumeLayout(false);
            this.groupBox_Images.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Blending_Fields)).EndInit();
            this.groupBox_Show.ResumeLayout(false);
            this.groupBox_Show.PerformLayout();
            this.groupBox_Colors.ResumeLayout(false);
            this.groupBox_Colors.PerformLayout();
            this.tabControl_User.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox_Thresholding.ResumeLayout(false);
            this.groupBox_Thresholding.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Otsu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox_UsedFeatures.ResumeLayout(false);
            this.groupBox_UsedFeatures.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox_TeacherType.ResumeLayout(false);
            this.groupBox_TeacherType.PerformLayout();
            this.groupBox_Counts.ResumeLayout(false);
            this.groupBox_Counts.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Confidence)).EndInit();
            this.groupBox_Layers.ResumeLayout(false);
            this.groupBox_Layers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNbofHiddenUnits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Blending_Result)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.GroupBox groupBox_Images;
        private System.Windows.Forms.TextBox textBox_ImageFileName;
        private System.Windows.Forms.Button button_BrowseImage;
        private System.Windows.Forms.PictureBox pictureBox_Image;
        private System.Windows.Forms.Label label_FileName;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.TrackBar trackBar_Blending_Fields;
        private System.Windows.Forms.Label label_Position1;
        private System.Windows.Forms.TextBox textBox_Position;
        private System.Windows.Forms.Button button_Zoom_Color;
        private System.Windows.Forms.Timer timer_zoom_pan;
        private System.Windows.Forms.GroupBox groupBox_Show;
        private System.Windows.Forms.TextBox textBox_ExecutionTime;
        private System.Windows.Forms.Label label_ProcessingTime;
        private System.Windows.Forms.TextBox textBox_ImageSize;
        private System.Windows.Forms.GroupBox groupBox_Colors;
        private System.Windows.Forms.Label label_ZoomedFieldColor;
        private System.Windows.Forms.CheckBox checkBox_ShowFilledFields;
        private System.Windows.Forms.CheckBox checkBox_ShowZoom;
        private System.Windows.Forms.Label label_object;
        private System.Windows.Forms.Button button_ObjectColor;
        private System.Windows.Forms.TabControl tabControl_User;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button_Teach;
        private System.Windows.Forms.Label label_Threshold1;
        private System.Windows.Forms.NumericUpDown numericUpDown_Confidence;
        private System.Windows.Forms.Label label_Threshold2;
        private System.Windows.Forms.GroupBox groupBox_Layers;
        private System.Windows.Forms.TextBox textBox_outputunits;
        private System.Windows.Forms.Label label_outputunits;
        private System.Windows.Forms.NumericUpDown numericUpDownNbofHiddenUnits;
        private System.Windows.Forms.Label label_hiddenunits;
        private System.Windows.Forms.TextBox textBox_inputunits;
        private System.Windows.Forms.Label label_inputunits;
        private System.Windows.Forms.GroupBox groupBox_UsedFeatures;
        private System.Windows.Forms.CheckBox checkBox_Show_Thresholded;
        private System.Windows.Forms.GroupBox groupBox_Thresholding;
        private System.Windows.Forms.RadioButton radioButton_Manual_thresholding;
        private System.Windows.Forms.RadioButton radioButton_Otsu_thresholding;
        private System.Windows.Forms.NumericUpDown numericUpDown_threshold;
        private System.Windows.Forms.NumericUpDown numericUpDown_Otsu;
        private System.Windows.Forms.Button button_ZoomThr_Dummy;
        private System.Windows.Forms.CheckBox checkBox_ShowZoomThr;
        private System.Windows.Forms.Label label_Position2;
        private System.Windows.Forms.Button button_Process;
        private System.Windows.Forms.CheckBox checkBox_KeepProcessing;
        private System.Windows.Forms.TrackBar trackBar_Blending_Result;
        private System.Windows.Forms.TextBox textBox_CurrentTeacher;
        private System.Windows.Forms.Button button_Next;
        private System.Windows.Forms.Button button_Previous;
        private System.Windows.Forms.CheckBox checkBox_AddTeacher;
        private System.Windows.Forms.GroupBox groupBox_TeacherType;
        private System.Windows.Forms.Button button_Teacher_Other_Color;
        private System.Windows.Forms.RadioButton radioButton_NotOurObject;
        private System.Windows.Forms.Label label_Name;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.Button button_AddName;
        private System.Windows.Forms.RadioButton radioButton_OurObject;
        private System.Windows.Forms.Button button_DeleteAll;
        private System.Windows.Forms.Button button_DeleteSelected;
        private System.Windows.Forms.Button button_RenameSelected;
        private System.Windows.Forms.Button button_Teacher_Our_Color;
        private System.Windows.Forms.GroupBox groupBox_Counts;
        private System.Windows.Forms.RadioButton radioButton_AllTeachers;
        private System.Windows.Forms.RadioButton radioButton_CurrentTeachers;
        private System.Windows.Forms.Label label_NamesList;
        private System.Windows.Forms.ListView listView_Names;
        private System.Windows.Forms.ColumnHeader columnHeader_NameValue;
        private System.Windows.Forms.ColumnHeader columnHeader_Count;
        private System.Windows.Forms.ColumnHeader columnHeader_Type;
        private System.Windows.Forms.Button button_Help;
        private System.Windows.Forms.CheckBox checkBox_circularity;
        private System.Windows.Forms.CheckBox checkBox_aspect_ratio;
        private System.Windows.Forms.CheckBox checkBox_max_diameter;
        private System.Windows.Forms.CheckBox checkBox_min_diameter;
        private System.Windows.Forms.CheckBox checkBox_grayscale;
        private System.Windows.Forms.CheckBox checkBox_area;
        private System.Windows.Forms.CheckBox checkBox_B_component;
        private System.Windows.Forms.CheckBox checkBox_G_component;
        private System.Windows.Forms.CheckBox checkBox_R_component;
        private System.Windows.Forms.Button button_PreProcess;
        private System.Windows.Forms.CheckBox checkBox_KeepPreProcessing;
    }
}

