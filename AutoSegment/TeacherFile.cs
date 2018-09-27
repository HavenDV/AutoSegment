using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoSegment
{
    public class TeacherFile
    {

        //-----------------------------------------------------------------------
        public static string ProfileFileName;   // full path for 'File_Teacher' data file

        public int NBOFFEATURES;


        static TeacherFile( )
        {
            ProfileFileName = Application.StartupPath + "\\File_Teacher.dat";
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


        //-------------------------------------------------------------------------------
        // Set preprocessing-related parameters/settings

        public void SetOtsuThresholdingSelected(bool bOtsu)
        {
            String sz_OtsuThresholding = (bOtsu) ? "true" : "false";
            WritePrivateProfileString("Preprocessing", "OtsuthresholdingSelected", sz_OtsuThresholding, ProfileFileName);
        }

        public void SetManualThresholdValue(int thresholdvalue)
        {
            String sz_manualthresholdvalue = Convert.ToString(thresholdvalue);
            WritePrivateProfileString("Preprocessing", "ManualThresholdvalue", sz_manualthresholdvalue, ProfileFileName);
        }

        public void SetOtsuThresholdValue(int thresholdvalue)
        {
            String sz_otsuthresholdvalue = Convert.ToString(thresholdvalue);
            WritePrivateProfileString("Preprocessing", "OtsuThresholdValue", sz_otsuthresholdvalue, ProfileFileName);
        }

        //-------------------------------------------------------------------------------
        // Get (retrieve) preprocessing-related parameters/settings
        public bool GetOtsuThresholdingSelected()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("Preprocessing", "OtsuthresholdingSelected", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bOtsuThresholdingSelected = text == "true" ? true : false;

            return bOtsuThresholdingSelected;

        }

        public int GetManualThresholdValue()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("Preprocessing", "ManualThresholdvalue", "180", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int manualthresholdvalue = Convert.ToInt16(text);

            return manualthresholdvalue;
        }

        public int GetOtsuThresholdValue()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("Preprocessing", "OtsuThresholdValue", "15", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int otsuthresholdvalue = Convert.ToInt16(text);

            return otsuthresholdvalue;
        }

        //------------------------------------------------------------------------------
        // Set the usage of features
        public void SetUsedFeatures_RComponent(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "RComponent", sz_Used, ProfileFileName);
        }

        public void SetUsedFeatures_GComponent(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "GComponent", sz_Used, ProfileFileName);
        }

        public void SetUsedFeatures_BComponent(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "BComponent", sz_Used, ProfileFileName);
        }

        public void SetUsedFeatures_Grayscale(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "Grayscale", sz_Used, ProfileFileName);
        }

        public void SetUsedFeatures_Area(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "Area", sz_Used, ProfileFileName);
        }

        public void SetUsedFeatures_MinDiameter(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "MinDiameter", sz_Used, ProfileFileName);
        }

        public void SetUsedFeatures_MaxDiameter(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "MaxDiameter", sz_Used, ProfileFileName);
        }

        public void SetUsedfeatures_Aspectratio(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "AspectRatio", sz_Used, ProfileFileName);
        }

        public void SetUsedfeatures_Circularity(bool bUsed)
        {
            String sz_Used = (bUsed) ? "true" : "false";
            WritePrivateProfileString("UsedFeatures", "Circularity", sz_Used, ProfileFileName);
        }

        //------------------------------------------------------------------------------
        // get the usage of features

        public bool GetUsedFeatures_RComponent()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "RComponent", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bRComponentUsed = text == "true" ? true : false;

            return bRComponentUsed;

        }

        public bool GetUsedFeatures_GComponent()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "GComponent", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bGComponentUsed = text == "true" ? true : false;

            return bGComponentUsed;

        }

        public bool GetUsedFeatures_BComponent()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "BComponent", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bBComponentUsed = text == "true" ? true : false;

            return bBComponentUsed;

        }

        public bool GetUsedFeatures_Grayscale()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "Grayscale", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bGrayscaleUsed = text == "true" ? true : false;

            return bGrayscaleUsed;

        }

        public bool GetUsedFeatures_Area()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "Area", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bAreaUsed = text == "true" ? true : false;

            return bAreaUsed;

        }

        public bool GetUsedFeatures_MinDiameter()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "MinDiameter", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bMinDiameterUsed = text == "true" ? true : false;

            return bMinDiameterUsed;

        }

        public bool GetUsedFeatures_MaxDiameter()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "MaxDiameter", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bMaxDiameterUsed = text == "true" ? true : false;

            return bMaxDiameterUsed;

        }

        public bool GetUsedfeatures_Aspectratio()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "AspectRatio", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bAspectRatioUsed = text == "true" ? true : false;

            return bAspectRatioUsed;

        }

        public bool GetUsedfeatures_Circularity()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("UsedFeatures", "Circularity", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bCircularityUsed = text == "true" ? true : false;

            return bCircularityUsed;

        }


        // -----------------------------------------------------------------------------

        public void SetFeatureNumber(int NbOfFeatures)
        {
            NBOFFEATURES = NbOfFeatures;

            String sz_nboffeatures = Convert.ToString(NbOfFeatures);
            WritePrivateProfileString("General", "NbOfFeatures", sz_nboffeatures, ProfileFileName);
        }

        public int GetNumberOfFeatures()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("General", "NbOfFeatures", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nboffeatures = Convert.ToInt16(text);

            return nboffeatures;
        }

        //--------------------------------------------------------------------------------
        // colors


        public void SetTeacherOurColor(Color color)
        {
            SetColor("TeacherOurColor", color);
        }

        public void SetTeacherOtherColor(Color color)
        {
            SetColor("TeacherOtherColor", color);
        }

        public void SetColor(string colortype, Color color)
        {
            String sz_color = Convert.ToString(color.R) + "," + Convert.ToString(color.G) + "," + Convert.ToString(color.B);
            WritePrivateProfileString("General", colortype, sz_color, ProfileFileName);
        }

        public Color GetTeacherOurColor()
        {
            return GetColor("TeacherOurColor");
        }
        public Color GetTeacherOtherColor()
        {
            return GetColor("TeacherOtherColor");
        }

        public Color GetColor(string colortype)
        {
            Color color = new Color();
            int red = 0;
            int green = 0;
            int blue = 0;

            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", colortype, "", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            // set default, if necessary
            if (text == "")
            {
                if (colortype == "TeacherOurColor")
                    text = "255,255,0";
                else if (colortype == "TeacherOtherColor")
                    text = "135,206,235";
                else
                    text = "0,0,0";
            }
            int length = text.Length;
            int index = 0;
            string currentitem = "";
            while (index < length && text[index] != ',')
            {
                currentitem += text[index];
                index++;
            }
            index++;
            red = Convert.ToInt16(currentitem);
            currentitem = "";
            while (index < length && text[index] != ',')
            {
                currentitem += text[index];
                index++;
            }
            index++;
            green = Convert.ToInt16(currentitem);
            currentitem = "";
            while (index < length && text[index] != ',')
            {
                currentitem += text[index];
                index++;
            }
            blue = Convert.ToInt16(currentitem);

            color = Color.FromArgb(255, red, green, blue);

            return color;
        }


        //--------------------------------------------------------------------------------

        public void CleanAll()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("DataFiles", "NbOfDataFiles", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofdatafiles = Convert.ToInt32(text);

            for (int ii = 0; ii < nbofdatafiles; ii++)
            {
                String DataFileID = "DataFile_" + Convert.ToString(ii+1);
                WritePrivateProfileString(DataFileID, null, null, ProfileFileName);
                WritePrivateProfileString("DataFiles", DataFileID, null, ProfileFileName);
            }
            WritePrivateProfileString("DataFiles", "NbOfDataFiles", "0", ProfileFileName);

            int nbofnames = GetNumberOfNames();
            for (int ii = 0; ii < nbofnames; ii++)
            {
                String NameID = "Name_" + Convert.ToString(ii + 1);
                WritePrivateProfileString("Names", NameID, null, ProfileFileName);
            }
            WritePrivateProfileString("Names", "NbOfNames", "0", ProfileFileName);
        }

        public void ClearTeachersOfCurrentImage(String filename)
        {
            int ID = GetIDForDataFile(filename);
            if (ID != -1)
            {
                String DataFileID = "DataFile_" + Convert.ToString(ID);
                WritePrivateProfileString(DataFileID, null, null, ProfileFileName);
            }
        }
        public bool StoreTeacherData(String FileName, double[] teachers, int index)
        {
            int ID = GetIDForDataFile(FileName);
            String DataFileID = "DataFile_" + Convert.ToString(ID);

            String TeacherString = "";
            int length = 3 + NBOFFEATURES;
            length += 2 * (int)teachers[index + length]+1;
            for ( int ii=0; ii<length; ii++)
            {
                if (ii > 0)
                    TeacherString += ",";
                TeacherString += Convert.ToString(teachers[index+ii]);
            }

            int TeacherID = GetNextTeacherID(FileName);
            String NbOfTeachers = Convert.ToString(TeacherID);
            WritePrivateProfileString(DataFileID, "NbOfTeachers", NbOfTeachers, ProfileFileName);
            String TeacherIDx = "Teacher_" + Convert.ToString(TeacherID);

            WritePrivateProfileString(DataFileID, TeacherIDx, TeacherString, ProfileFileName);

            return true;
        }

        //----------------------------------------------------------------------------------------------------------------------

        public int GetNumberOfNames()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("Names", "NbOfNames", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofnames = Convert.ToInt16(text);

            return nbofnames;
        }

        public String GetName(int index)
        {
            String Name = "";
            String NameeIDx = "Name_" + Convert.ToString(index);
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("Names", NameeIDx, "", buffer, buffer_length, ProfileFileName);
            Name = buffer.ToString();
            int index2 = Name.IndexOf(",");
            if (index2 > 0)
            {
                Name = Name.Remove(index2);
            }

            return Name;
        }

        public String GetType(int index)
        {
            String Type = "";
            String NameeIDx = "Name_" + Convert.ToString(index+1);
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("Names", NameeIDx, "", buffer, buffer_length, ProfileFileName);
            Type = buffer.ToString();
            Type = Type.Substring(Type.IndexOf(",") + 1);

            return Type;
        }

        public int GetNameIndex( string name)
        {
            int index = 0;
            int nbofnames = GetNumberOfNames();
            bool bFound = false;
            while (!bFound && index <= nbofnames)
            {
                string currentname = GetName(index);
                if (currentname == name)
                    bFound = true;
                else
                    index++;
            }

            return bFound ? index:(-1);
        }

        public int GetNameIndexForPosition(String FileName, int xpos, int ypos)
        {
            int NameID = -1;
            int ID = GetIDForDataFile(FileName);
            String DataFileID = "DataFile_" + Convert.ToString(ID);
            int TeacherID = GetTeacherID(FileName, xpos, ypos);
            if (TeacherID == -1)
                return NameID;
            double [] Data = new double[4+ NBOFFEATURES];
            RetrieveTeacherData(FileName, TeacherID, Data);
            return (int)Data[0];
        }

        public void RemoveName(string name)
        {
            int index = GetNameIndex(name);
            if (index == -1)
                return; // not found

            String NameIDx = "Name_" + Convert.ToString(index);
            WritePrivateProfileString("Names", NameIDx, "", ProfileFileName);

            // Remove all related data
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("DataFiles", "NbOfDataFiles", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofdatafiles = Convert.ToInt32(text);
            for (int ii = 0; ii < nbofdatafiles; ii++)
            {
                String DataFileIDx = "DataFile_" + Convert.ToString(ii + 1);
                String FileName = GetFileName(ii+1);
                GetPrivateProfileString(DataFileIDx, "NbOfTeachers", "0", buffer, buffer_length, ProfileFileName);
                text = buffer.ToString();
                int NbofTeachers = Convert.ToInt32(text);
                double[] Data = new double[10000];   // overallocated - no index check
                for ( int jj=0; jj< NbofTeachers; jj++)
                {
                    RetrieveTeacherData(FileName, jj + 1, Data);
                    if (Data[0] == index)
                    {
                        RemoveTeacher(FileName, DataFileIDx, jj + 1);
                        NbofTeachers--; 
                    }
                }

            }
        }

        public void RenameName(string oldname, string newname, string newtype)
        {
            int index = GetNameIndex(oldname);
            if (index == -1)
                return; // not found

            String NameIDx = "Name_" + Convert.ToString(index);
            string newnamestring = newname + "," + newtype;
            WritePrivateProfileString("Names", NameIDx, newnamestring, ProfileFileName);
        }

        public void AddName(string name, string type)
        {
            // REM: it is already checked (in ListView), that the 'name' is not yet registered
            int nbofnames = GetNumberOfNames();
            bool bFound = false;
            int index = 0;
            while (!bFound && index < nbofnames)
            {
                string currentname = GetName(index+1);
                if (currentname == "")
                    bFound = true;
                else
                    index++;
            }
            if (!bFound)
            {
                // new item is appended with the next unused index
                nbofnames++;
                string newnumber = Convert.ToString(nbofnames);
                WritePrivateProfileString("Names", "NbOfNames", newnumber, ProfileFileName);
            }

            String NameIDx = "Name_" + Convert.ToString(index + 1);
            string namestring = name + "," + type;
            WritePrivateProfileString("Names", NameIDx, namestring, ProfileFileName);

        }

        //-----------------------------------------------------------
        public int GetNumberOfDataFiles()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("DataFiles", "NbOfDataFiles", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofdatafiles = Convert.ToInt32(text);

            return nbofdatafiles;
        }
        public int GetNextDataFileID()
        {
            int nbofdatafiles = GetNumberOfDataFiles();

            return nbofdatafiles + 1;
        }

        public int GetIDForDataFile(String FileName)
        {
            if (FileName == "")
                return -1;

            // Check, if there are already techer data for 'FileName' file?
            int NextDataFileID = GetNextDataFileID();
            // Boolean bFound = false;
            for (int i = 1; i < NextDataFileID; i++)
            {
                String FileName1 = GetFileName(i);
                if (FileName == FileName1)
                    return i;   // (found)
            }

            // use the next (empty) ID
            String NbOfDataFileString = Convert.ToString(NextDataFileID);
            WritePrivateProfileString("DataFiles", "NbOfDataFiles", NbOfDataFileString, ProfileFileName);
            String DataFileID = "DataFile_" + Convert.ToString(NextDataFileID);
            WritePrivateProfileString("DataFiles", DataFileID, FileName, ProfileFileName);
            return NextDataFileID;
        }

        public int GetNextTeacherID(String FileName)
        {
            int ID = GetIDForDataFile(FileName);
            String DataFileID = "DataFile_" + Convert.ToString(ID);

            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString(DataFileID, "NbOfTeachers", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofteachers = Convert.ToInt32(text);

            return nbofteachers + 1;
        }

        public int GetTeacherID(String FileName, int pos_x, int pos_y)
        {
            int nbofteachers = GetNumberOfTeachers(FileName);
            double[] Data = new double[100000];           // overallocated - no indexing check
            for (int ii = 0; ii < nbofteachers; ii++)
            {
                RetrieveTeacherData(FileName, ii + 1, Data);
                Point pos = new Point((int)Data[1], (int)Data[2]);
                if (pos.X == pos_x && pos.Y == pos_y)
                    return ii+1;
            }

            return -1;
        }

        public String GetFileName(int DataFileID)
        {

            String FileName = "";
            String DataFileIDx = "DataFile_" + Convert.ToString(DataFileID);
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("DataFiles", DataFileIDx, "", buffer, buffer_length, ProfileFileName);
            FileName = buffer.ToString();

            return FileName;
        }

        public int GetNumberOfTeachers()
        {
            int NbofTeachers = 0;

            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("DataFiles", "NbOfDataFiles", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofdatafiles = Convert.ToInt32(text);

            for (int ii = 0; ii < nbofdatafiles; ii++)
            {
                String DataFileID = "DataFile_" + Convert.ToString(ii + 1);
                GetPrivateProfileString(DataFileID, "NbOfTeachers", "0", buffer, buffer_length, ProfileFileName);
                text = buffer.ToString();
                NbofTeachers += Convert.ToInt32(text);
            }

            return NbofTeachers;
        }

        //--------------------------------------------------------------------------------------------------------------------

        public void RemoveTeacher(String FileName, String DataFileID, int TeacherID)
        {

            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString(DataFileID, "NbOfTeachers", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofteachers = Convert.ToInt32(text);

            // delete the current item
            String TeacherIDx = "Teacher_" + Convert.ToString(TeacherID);
            WritePrivateProfileString(DataFileID, TeacherIDx, null, ProfileFileName);
            // shift ahead the following items
            for ( int ii= TeacherID+1; ii<= nbofteachers; ii++)
            {
                TeacherIDx = "Teacher_" + Convert.ToString(ii);
                GetPrivateProfileString(DataFileID, TeacherIDx, "", buffer, buffer_length, ProfileFileName);
                text = buffer.ToString();
                TeacherIDx = "Teacher_" + Convert.ToString(ii-1);
                WritePrivateProfileString(DataFileID, TeacherIDx, text, ProfileFileName);
            }
            if (nbofteachers > 1)
            {
                // remove the last item
                TeacherIDx = "Teacher_" + Convert.ToString(nbofteachers);
                WritePrivateProfileString(DataFileID, TeacherIDx, null, ProfileFileName);
            }

            String NbOfTeachers = Convert.ToString(nbofteachers-1);
            WritePrivateProfileString(DataFileID, "NbOfTeachers", NbOfTeachers, ProfileFileName);

        }

        //----------------------------------------------------------------------------------------------------------
        public int GetNumberOfTeachers(String FileName)
        {
            int nbofteachers = 0;
            if (FileName == "")
                return nbofteachers;

            int ID = GetIDForDataFile(FileName);
            String DataFileID = "DataFile_" + Convert.ToString(ID);

            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString(DataFileID, "NbOfTeachers", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            nbofteachers = Convert.ToInt16(text);

            return nbofteachers;
        }

        public void GetTeachers(String filename, double[] Teachers)
        {
            // get all teachers - defined on the current imagefile
            String text;
            int buffer_length = 100000;
            StringBuilder buffer = new StringBuilder(buffer_length);

            int ID = GetIDForDataFile(filename);
            String DataFileID = "DataFile_" + Convert.ToString(ID);
            int nbofteachers = GetNumberOfTeachers(filename);
            int outindex = 1;
            Teachers[0] = nbofteachers;
            for ( int ii=0; ii< nbofteachers; ii++)
            {
                String TeacherIDx = "Teacher_" + Convert.ToString(ii+1);
                GetPrivateProfileString(DataFileID, TeacherIDx, "", buffer, buffer_length, ProfileFileName);
                text = buffer.ToString();
                int length = text.Length; 
                int index = 0;
                while (index < length)
                {
                    string currentitem = "";
                    while (index < length && text[index] != ',')
                    {
                        currentitem += text[index];
                        index++;
                    }
                    index++;
                    Teachers[outindex] = Convert.ToDouble(currentitem);
                    outindex++;
                }
            }
        }
        public void GetTeachers( double [] Teachers)
        {
            // get all teachers - defined on all image files
            String text;
            int buffer_length = 100000;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString("DataFiles", "NbOfDataFiles", "0", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int nbofdatafiles = Convert.ToInt32(text);
            int outindex = 0;

            for (int ii = 0; ii < nbofdatafiles; ii++)
            {
                String DataFileID = "DataFile_" + Convert.ToString(ii + 1);
                GetPrivateProfileString(DataFileID, "NbOfTeachers", "0", buffer, buffer_length, ProfileFileName);
                text = buffer.ToString();
                int nbofteachers = Convert.ToInt16(text);
                for (int jj = 1; jj <= nbofteachers; jj++)
                {
                    String TeacherIDx = "Teacher_" + Convert.ToString(jj);
                    GetPrivateProfileString(DataFileID, TeacherIDx, "", buffer, buffer_length, ProfileFileName);
                    text = buffer.ToString();
                    int length = text.Length;

                    int index = 0;
                    while (index < length)
                    {
                        string currentitem = "";
                        while (index < length && text[index] != ',')
                        {
                            currentitem += text[index];
                            index++;
                        }
                        index++;
                        Teachers[outindex] = Convert.ToDouble(currentitem);
                        outindex++;
                    }
                }
            }

        }

        public void RetrieveTeacherData(String FileName, int TeacherID, double[] Data)
        {
            int ID = GetIDForDataFile(FileName);
            String DataFileID = "DataFile_" + Convert.ToString(ID);
            String TeacherIDx = "Teacher_" + Convert.ToString(TeacherID);

            String text;
            int buffer_length = 100000;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString(DataFileID, TeacherIDx, "", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int length = text.Length;
            if (length == 0)
                return;

            int index = 0;
            int outindex = 0;
            while (index < length)
            {
                string currentitem = "";
                while (index < length && text[index] != ',')
                {
                    currentitem += text[index];
                    index++;
                }
                index++;
                Data[outindex] = Convert.ToDouble(currentitem);
                outindex++;
            }
        }

        public int RetrieveTeacherFeatures(String FileName, int TeacherID, double[] Data, int startindex, int NBOFFEATURES)
        {
            int ID = GetIDForDataFile(FileName);
            String DataFileID = "DataFile_" + Convert.ToString(ID);
            String TeacherIDx = "Teacher_" + Convert.ToString(TeacherID);

            String text;
            int buffer_length = 100000;
            StringBuilder buffer = new StringBuilder(buffer_length);

            GetPrivateProfileString(DataFileID, TeacherIDx, "", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int length = text.Length;
            if (length == 0)
                return -1;      // (not a valid teacher)

            int index = 0;
            int outindex = 0;
            int expected_decision = 0;
            int outindex_max = startindex + NBOFFEATURES;
            while ((index < length) && (outindex<outindex_max))
            {
                string currentitem = "";
                while (index < length && text[index] != ',')
                {
                    currentitem += text[index];
                    index++;
                }
                index++;
                if ( outindex==0)
                    expected_decision = Convert.ToInt16(currentitem);
                if ( outindex>= startindex)
                    Data[outindex - startindex] = Convert.ToDouble(currentitem);
                outindex++;
            }

            return expected_decision;
        }

    }
}
