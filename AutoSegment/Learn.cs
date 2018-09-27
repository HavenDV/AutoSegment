using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using ImageProcessingUtils;

namespace AutoSegment

{
    public partial class Learn : Form
    {
        public int NbOfFileNames;       // number of image file names, used at teaching
        public int NbofTeachers;        // number of teacher data sets
        public int CurrentItem;         // index of currently processed item
        public int CurrentSession;      // in case of cyclic execution: serial number of executed cycle
        public Boolean bStarted;        // "true", if the learning process is activated.

        public static double[] Weights = { };   // weights of neural network
        public double confidence;       // confidence of accepting a NN decision
        public int nbOfUnits_1;         // nb of Units in INPUT layer
        public int nbOfUnits_2;         // nb. of Units in HIDDEN layer
        public int nbOfUnits_3;         // nb. of Units in OUTPUT layer
        public double factor;           // learning factor (small:precize - large:fast)

        // testing
        public static double[] errors;  // differences between expected and resulted decisions
        //                                  for all elements
        public double errorabs_sum;     // sum of absolute errors
        public int nbofprocteachers;    // number of processed teachers
        public double maxerror;         // max. (absolute value) of error

        // misc
        public int ERROR_SIZE;          // overestimated size of error buffer (not din.)
        public int NBOFFEATURES;        // number of features for decisions

        public Learn( int NbOfFeatures)
        {
            NBOFFEATURES = NbOfFeatures;

            InitializeComponent();

            ERROR_SIZE = 1000;           // incredible big...unreal to overload
            Array.Resize(ref errors, ERROR_SIZE);
            errorabs_sum = 0.0;
            nbofprocteachers = 0;
            maxerror = 0.0;

        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if (bStarted)
                timer1.Stop();

            // Save the weights to data files
            Cursor = Cursors.WaitCursor;
            WeightFile wf = new WeightFile();
            wf.SetConfidence(confidence);
            wf.SaveSettings( nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
            wf.SaveWeights(Weights, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
            Cursor = Cursors.Default;

            Close();
        }

        private void Learn_Shown(object sender, EventArgs e)
        {

            // show the current number of teacher data
            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            NbOfFileNames = tf.GetNextDataFileID()-1;
            label_NbofFiles.Text = "Number of used image files:  " + Convert.ToString(NbOfFileNames);
            NbofTeachers = tf.GetNumberOfTeachers();
            label_NbofTeachers.Text = "Number of defined teachers:  " + Convert.ToString(NbofTeachers);
            numericUpDown_Convergence.Value = Convert.ToDecimal( factor );

            int NbofWeights = nbOfUnits_1 + nbOfUnits_1 * nbOfUnits_2 + nbOfUnits_2 * nbOfUnits_3;
            Array.Resize(ref Weights, NbofWeights);
            for (int ii = 0; ii < NbofWeights; ii++)
            {
                Weights[ii] = 0.0;
            }

            // Load the weights from data file
            WeightFile wf = new WeightFile();
            Cursor = Cursors.WaitCursor;
            bool bSuccess;
            bSuccess = wf.LoadWeights(Weights, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
            Cursor = Cursors.Default;
            if (!bSuccess )
            {
                MessageBox.Show("'Weights_File' data file is missing or injured - start working with random weights.");
                InitializeWeights();
            }

            timer1.Interval = 1;  // let the teaching timer's interval: 1 msec
            bStarted = false;
        }

        private void numericUpDown_Convergence_ValueChanged(object sender, EventArgs e)
        {
            factor = Convert.ToDouble(numericUpDown_Convergence.Value);
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show("Are you sure, that the current weights can be discarded?",
                "Warning", buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // init. the Weights
                InitializeWeights();
            }

            WeightFile wf = new WeightFile();
            wf.Clean();

            label_Processed.Text = "Processed:          ";
            label_OverallError.Text = "Overall:        ";
            label_MaxError.Text = "Maximum:        ";
        }

        private void button_Learn_Click(object sender, EventArgs e)
        {
            if (bStarted)
            {
                // stop the cyclic learning
                timer1.Stop();
                bStarted = false;
                SetButtonsState();
                return;
            }

            InitWeightsIfNecessary();   // execute clear/random init, if no previous result

            // start the teaching process
            int nbofitems = NbOfFileNames;
            string nbofitemsstr = Convert.ToString(nbofitems);
            label_Processed.Text = "Processed: 0/" + nbofitemsstr;
            label_OverallError.Text = "Overall:        ";
            label_MaxError.Text = "Maximum:        ";
            CurrentItem = 0;
            CurrentSession = 0;
            errorabs_sum = 0;
            nbofprocteachers = 0;
            maxerror = 0.0;
            bStarted = true;
            SetButtonsState();

            timer1.Start();
        }

        private void InitWeightsIfNecessary()
        {
            // check, if there are nonzero members of Weights vector. If all of them zero,
            // init with random numbers.
            int NbofWeights = nbOfUnits_1 + nbOfUnits_1 * nbOfUnits_2 + nbOfUnits_2 * nbOfUnits_3;

            for (int ii = 0; ii < NbofWeights; ii++)
            {
                if (Weights[ii] != 0.0)
                    return;
                InitializeWeights();
            }

        }

        //----------------------------------------------------------------------------------------------
        // ### Learning ###

        // timer for learning
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            int nbofitems = NbOfFileNames;
            if (CurrentItem == nbofitems)
            {
                // show the result
                string str = nbofprocteachers== 0 ? "0" :  Convert.ToString(errorabs_sum / (double)nbofprocteachers);
                str = OwnFormat(str);
                label_OverallError.Text = "Overall: " + str;
                str = Convert.ToString(maxerror);
                str = OwnFormat(str);
                label_MaxError.Text = "Maximum: " + str;

                double errorlimit = Convert.ToDouble(numericUpDown_MaxError.Value);
                if (maxerror > errorlimit)
                {
                    CurrentSession++;
                    CurrentItem = 0;
                    errorabs_sum = 0;
                    nbofprocteachers = 0;
                    maxerror = 0.0;
                    timer1.Start();
                    return;
                }

                bStarted = false;
                SetButtonsState();

                return;
            }

            // execute the teaching (using the current image's data elements)
            int nbofvalidelements = LearnNextImage(CurrentItem, errors);

            // cumulate the errors
            for (int ii = 0; ii < nbofvalidelements; ii++)
            {
                double abserror = errors[ii] > 0.0 ? errors[ii] : -errors[ii];
                errorabs_sum += abserror;
                if (abserror > maxerror)
                    maxerror = abserror;
            }
            nbofprocteachers += nbofvalidelements;

            string nbofitemsstr = Convert.ToString(nbofitems);
            label_Processed.Text = "Processed: " + Convert.ToString(CurrentItem+1) + "/" + nbofitemsstr;
            label_Processed.Text += "     [ " + Convert.ToString(CurrentSession + 1) + " ]";


            CurrentItem++;
            timer1.Start();
        }

        private void InitializeWeights()
        {
            // init. the weights
            CImageProcessingUtils.InitNeuralNetwork(Weights,
                nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
        }

        private int LearnNextImage( int CurrentItem, double[] errors )
        {
            int nbofvalidelements = 0;

            TeacherFile tf = new TeacherFile();
            tf.SetFeatureNumber(NBOFFEATURES);
            double[] CurrentFeatures = new double[NBOFFEATURES];
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

            double[] Decisions = new double[nbOfUnits_3];

            string filename = tf.GetFileName(CurrentItem + 1);
            int nbofteachers = tf.GetNumberOfTeachers(filename);
            if (nbofteachers==0 )
                return nbofvalidelements;

            int decision = 0;
            int errorindex = 0;

            for ( int ii=0; ii< nbofteachers; ii++)
            {
                int ExpectedDecision = tf.RetrieveTeacherFeatures(filename, ii + 1, CurrentFeatures, 3, NBOFFEATURES);
                if (ExpectedDecision == -1)
                    continue;

                for (int jj = 0; jj < NBOFFEATURES; jj++)
                {
                    if (UsedFeatures[jj] == false)
                        CurrentFeatures[jj] = 0.0;  // (discard the extracted value)
                }

                decision = CImageProcessingUtils.TeachNeuralNetwork(CurrentFeatures, Weights,
                    nbOfUnits_1, nbOfUnits_2, nbOfUnits_3, factor, ExpectedDecision, Decisions);
                Decisions[ExpectedDecision - 1] -= 1.0;  // this element of 'Decisions' vector is the correct answer
                for (int jj = 0; jj < nbOfUnits_3; jj++)
                {
                    errors[errorindex] = Decisions[jj];
                    nbofvalidelements++;
                    errorindex++;
                }
            }

            return nbofvalidelements;
        }

        string OwnFormat(string str)
        {
            // allow only six digit after point
            int pos = str.IndexOf('.');
            if (pos == -1)
                return str;

            int length = (pos + 7) < str.Length ? (pos + 7) : str.Length;
            str = str.Substring(0, length);
            if (str[str.Length - 1] == '.')
                str += "0";
            return str;
        }

        void SetButtonsState()
        {
            button_Clear.Enabled = !bStarted;
            button_Close.Enabled = !bStarted;
            button_Learn.BackColor = bStarted ? Color.Pink : Color.PaleGreen;
            button_Learn.Text = bStarted ? "Stop!" : "Learn!";
        }

    }
}
