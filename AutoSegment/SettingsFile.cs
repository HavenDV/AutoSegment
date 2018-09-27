using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoSegment
{
    public class SettingsFile
    {

        //-----------------------------------------------------------------------
        public static string ProfileFileName;   // full path for 'File_Settings' data file

        static SettingsFile( )
        {
            ProfileFileName = Application.StartupPath + "\\File_Settings.dat";
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
        // Set (store) general parameters/settings

        public void SetObjectColor( Color color)
        {
            SetColor("ObjectColor", color);
        }

        public void SetZoomedFieldColor(Color color)
        {
            SetColor("ZoomedFieldColor", color);
        }

        public void SetFilterIndex(int filterindex)
        {
            String sz_filterindex = Convert.ToString(filterindex);
            WritePrivateProfileString("General", "FilterIndex", sz_filterindex, ProfileFileName);
        }

        public void SetColor( string colortype, Color color)
        {
            String sz_color = Convert.ToString(color.R) + "," + Convert.ToString(color.G) + "," + Convert.ToString(color.B);
            WritePrivateProfileString("General", colortype, sz_color, ProfileFileName);
        }

        public void SetShowFilledFields( bool bFilled )
        {
            String sz_filled = (bFilled) ? "true" : "false";
            WritePrivateProfileString("General", "ShowFilledFields", sz_filled, ProfileFileName);
        }

        public void SetBlendingFieldsRate(int blendingfieldsrate)
        {
            String sz_blendingfieldsrate = Convert.ToString(blendingfieldsrate);
            WritePrivateProfileString("General", "BlendingFieldsRate", sz_blendingfieldsrate, ProfileFileName);
        }

        public void SetBlendingResultRate(int blendingresultrate)
        {
            String sz_blendingresultrate = Convert.ToString(blendingresultrate);
            WritePrivateProfileString("General", "BlendingResultRate", sz_blendingresultrate, ProfileFileName);
        }

        public void SetKeepPreProcessing(bool bKeepPreProcessing)
        {
            String sz_KeepPreProcessing = (bKeepPreProcessing) ? "true" : "false";
            WritePrivateProfileString("General", "KeepPreProcessing", sz_KeepPreProcessing, ProfileFileName);
        }

        public void SetKeepProcessing(bool bKeepProcessing)
        {
            String sz_KeepProcessing = (bKeepProcessing) ? "true" : "false";
            WritePrivateProfileString("General", "KeepProcessing", sz_KeepProcessing, ProfileFileName);
        }

        public void SetShowZoom(bool bShowZoom)
        {
            String sz_ShowZoom = (bShowZoom) ? "true" : "false";
            WritePrivateProfileString("General", "ShowZoom", sz_ShowZoom, ProfileFileName);
        }

        public void SetShowZoomThr(bool bShowZoomThr)
        {
            String sz_ShowZoomThr = (bShowZoomThr) ? "true" : "false";
            WritePrivateProfileString("General", "ShowZoomThr", sz_ShowZoomThr, ProfileFileName);
        }

        public void SetShowThresholded(bool bShowThresholded)
        {
            String sz_ShowThresholded = (bShowThresholded) ? "true" : "false";
            WritePrivateProfileString("General", "ShowThresholded",  sz_ShowThresholded, ProfileFileName);
        }

        //-------------------------------------------------------------------------------
        // Get (retrieve) general parameters/settings

        public Color GetObjectColor()
        {
            return GetColor("ObjectColor");
        }

        public Color GetZoomedFieldColor()
        {
            return GetColor("ZoomedFieldColor");
        }

        public bool GetShowFilledFields()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "ShowFilledFields", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bShowFilledFields = text == "true" ? true : false;

            return bShowFilledFields;
        }

        public int GetFilterIndex()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "FilterIndex", "1", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int filterindex = Convert.ToInt16(text);

            return filterindex;
        }

        public int GetBlendingFieldsRate()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "BlendingFieldsRate", "127", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int blendingfieldsrate = Convert.ToInt16(text);

            return blendingfieldsrate;
        }

        public int GetBlendingResultRate()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "BlendingResultRate", "50", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            int blendingresultrate = Convert.ToInt16(text);

            return blendingresultrate;
        }

        public Color GetColor( string colortype)
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
            if (text=="" )
            {
                if (colortype == "ObjectColor")
                    text = "144,238,144";
                else if (colortype == "ZoomedFieldColor")
                    text = "255,255,0";
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

        public bool GetKeepPreProcessing()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "KeepPreProcessing", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bKeepPreProcessing = text == "true" ? true : false;

            return bKeepPreProcessing;
        }

        public bool GetKeepProcessing()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "KeepProcessing", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bKeepProcessing = text == "true" ? true : false;

            return bKeepProcessing;
        }

        public bool GetShowZoom()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "ShowZoom", "true", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bShowZoom = text == "true" ? true : false;

            return bShowZoom;
        }
        public bool GetShowZoomThr()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "ShowZoomThr", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bShowZoomThr = text == "true" ? true : false;

            return bShowZoomThr;
        }

        public bool GetShowThresholded()
        {
            String text;
            int buffer_length = 500;
            StringBuilder buffer = new StringBuilder(buffer_length);
            GetPrivateProfileString("General", "ShowThresholded", "false", buffer, buffer_length, ProfileFileName);
            text = buffer.ToString();
            bool bShowThresholded = text == "true" ? true : false;

            return bShowThresholded;
        }

        //--------------------------------------------------------------------------------


        }
}
