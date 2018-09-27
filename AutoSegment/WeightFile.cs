using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoSegment
{
    public class WeightFile
    {

        //-----------------------------------------------------------------------
        public static string File_Weights;   // full path for 'File_Weights' data file

        public static int PSIZE;           // size of packages at storing 'weights' 


        static WeightFile()
        {
            File_Weights = Application.StartupPath + "\\File_Weights.dat";

            PSIZE = 1000;
        }

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA",
             CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(
            string sectionName,
            string keyName,
            string defaultValue,
            StringBuilder returnbuffer,
            Int32 bufferSize,
            string fileName);
        [DllImport("kernel32", EntryPoint = "WritePrivateProfileStringA",
            CharSet = CharSet.Ansi)]
        public static extern bool WritePrivateProfileString(
            string sectionName,
            string keyName,
            string buffer,
            string fileName);

        // -----------------------------------------------------------------------------

        public void Clean()
        {
            String ProfileFileName = File_Weights;
            WritePrivateProfileString("Data", null, null, ProfileFileName);
        }

        public void SaveSettings( int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3)
        {
            String ProfileFileName = File_Weights;
            String Field_ID = "NeuralNetwork";

            WritePrivateProfileString(Field_ID, "nbOfUnits_1", Convert.ToString(nbOfUnits_1), ProfileFileName);
            WritePrivateProfileString(Field_ID, "nbOfUnits_2", Convert.ToString(nbOfUnits_2), ProfileFileName);
            WritePrivateProfileString(Field_ID, "nbOfUnits_3", Convert.ToString(nbOfUnits_3), ProfileFileName);
        }

        public void SaveWeights(double[] Weights, int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3)
        {
            String ProfileFileName = File_Weights;

            // split the weights into packages, with length: PSIZE=1000 values), as the Private File handling
            // system has some limit in the size of fields.
            String Field_ID = "Data";
            int NbofWeights = nbOfUnits_1 + nbOfUnits_1 * nbOfUnits_2 + nbOfUnits_2 * nbOfUnits_3;
            int package_sn = 0;
            while (NbofWeights > 0)
            {
                // save the next package
                String Package_ID = "Weights_" + Convert.ToString(package_sn);
                int nbofweights = (PSIZE < NbofWeights) ? PSIZE : NbofWeights;
                int start_index = package_sn * PSIZE;
                int endindex = start_index + nbofweights;
                String weights = "";
                for (int ii = start_index; ii < endindex; ii++)
                {
                    weights += Convert.ToString(Weights[ii]);
                    if (ii != (endindex - 1))
                        weights += ",";
                }
                WritePrivateProfileString(Field_ID, Package_ID, weights, ProfileFileName);
                package_sn++;
                NbofWeights -= nbofweights;
            }
        }

        public int LoadNbOfUnits_1()
        {
            String ProfileFileName = File_Weights;
            String text;
            int buffer_length = 200;
            StringBuilder buffer = new StringBuilder(buffer_length);
            String Field_ID = "NeuralNetwork";
            GetPrivateProfileString(Field_ID, "nbOfUnits_1", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbOfUnits_1 = Convert.ToInt16(text);
            return nbOfUnits_1;
        }

        public int LoadNbOfUnits_2()
        {
            String ProfileFileName = File_Weights;
            String text;
            int buffer_length = 200;
            StringBuilder buffer = new StringBuilder(buffer_length);
            String Field_ID = "NeuralNetwork";
            GetPrivateProfileString(Field_ID, "nbOfUnits_2", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbOfUnits_2 = Convert.ToInt16(text);
            return nbOfUnits_2;
        }

        public int LoadNbOfUnits_3()
        {
            String ProfileFileName = File_Weights;
            String text;
            int buffer_length = 200;
            StringBuilder buffer = new StringBuilder(buffer_length);
            String Field_ID = "NeuralNetwork";
            GetPrivateProfileString(Field_ID, "nbOfUnits_3", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbOfUnits_3 = Convert.ToInt16(text);
            return nbOfUnits_3;
        }

        public void SetConfidence( double confidence)
        {
            String ProfileFileName = File_Weights;
            String Field_ID = "NeuralNetwork";

            WritePrivateProfileString(Field_ID, "Confidence", Convert.ToString(confidence), ProfileFileName);
        }

        public double GetConfidence()
        {
            String ProfileFileName = File_Weights;
            String text;
            int buffer_length = 100000; // extremely over-estimated
            StringBuilder buffer = new StringBuilder(buffer_length);

            String Field_ID = "NeuralNetwork";
            GetPrivateProfileString(Field_ID, "Confidence", "0.6", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            double confidence = Convert.ToDouble(text);

            return confidence;
        }

        public Boolean LoadWeights(double[] Weights, int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3)
        {
            String ProfileFileName = File_Weights;
            String text;
            int buffer_length = 100000; // extremely over-estimated
            StringBuilder buffer = new StringBuilder(buffer_length);

            String Field_ID = "NeuralNetwork";
            GetPrivateProfileString(Field_ID, "nbOfUnits_1", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            if (nbOfUnits_1 != Convert.ToInt16(text))
                return false;
            GetPrivateProfileString(Field_ID, "nbOfUnits_2", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            if (nbOfUnits_2 != Convert.ToInt16(text))
                return false;
            GetPrivateProfileString(Field_ID, "nbOfUnits_3", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            if (nbOfUnits_3 != Convert.ToInt16(text))
                return false;

            // Read the weights in packages - because they are splitted (due to Private profile file handling...)
            Field_ID = "Data";
            int NbofWeights = nbOfUnits_1 + nbOfUnits_1 * nbOfUnits_2 + nbOfUnits_2 * nbOfUnits_3;
            int package_sn = 0;
            int index = 0;
            while (index < NbofWeights)
            {
                // load the next package
                String Package_ID = "Weights_" + Convert.ToString(package_sn);
                int nbofweights = (PSIZE < NbofWeights) ? PSIZE : NbofWeights;
                int start_index = package_sn * PSIZE;
                int endindex = start_index + nbofweights;
                int charnb = GetPrivateProfileString(Field_ID, Package_ID, "0", buffer, buffer_length, ProfileFileName);
                if (charnb == 0)
                {
                    return true;
                }
                text = buffer.ToString();
                int length = text.Length;
                int ii=0;
                while (ii < length)
                {
                    string currentitem = "";
                    while (ii < length && text[ii] != ',')
                    {
                        currentitem += text[ii];
                        ii++;
                    }
                    ii++;    // jump over the separator
                    try
                    {
                        Weights[index] = Convert.ToDouble(currentitem);
                    }
                    catch (Exception x)
                    {
                        return false;   // something went wrong...
                    }
                    index++;
                }
                package_sn++;
            }

            return true;
        }
    }
}
