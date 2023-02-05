using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Doxygen.Editor
{
    public class DoxyFile
    {
        public static bool UpdateDoxyFile(DoxySettings doxySettings)
        {
            if (DoxygenFileExist(doxySettings))
            {
                string doxyContent = ReadDoxyfile(doxySettings);
                doxyContent = EditDoxyString(doxyContent, doxySettings);
                BuildFile(doxyContent, doxySettings);
                return true;
            }
            else
                return false;
        }

        public static bool DoxygenFileExist(DoxySettings doxySettings) =>
                File.Exists(@$"{doxySettings.SettingsFolder}\{doxySettings.SettingsFile}");

        private static string ReadDoxyfile(DoxySettings doxySettings)
        {
            string doxyFileName = @$"{doxySettings.SettingsFolder}\{doxySettings.SettingsFile}";
            try
            {
                if (File.Exists(doxyFileName))
                {
                    using (StreamReader sr = new(doxyFileName))
                    {
                        string line = sr.ReadToEnd();
                        return line;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (FileNotFoundException ex)
            {
                Debug.Log(ex);
            }
            return "";
        }

        private static string EditDoxyString(string doxyContent, DoxySettings doxySettings)
        {
            if (doxyContent != null)
            {
                doxyContent = Regex.Replace(doxyContent, @"PROJECT_NUMBER =[A-Za-z0-9_. ]*",             "PROJECT_NUMBER = " + doxySettings.ProjectVersionNumber);
                doxyContent = Regex.Replace(doxyContent, @"PROJECT_BRIEF  =[A-Za-z0-9_. " + "\"" + "]*", "PROJECT_BRIEF  = " + "\"" + doxySettings.ProjectBrief + "\"");
                doxyContent = Regex.Replace(doxyContent, @"PROJECT_NAME   =[A-Za-z0-9_. " + "\"" + "]*", "PROJECT_NAME   = " + "\"" + doxySettings.ProjectName + "\"");
                return doxyContent;
            }
            else
            {
                Debug.Log("Could not build file due to null input");
                return null;
            }
        }

        private static void BuildFile(string doxyContent, DoxySettings doxySettings)
        {
            if (doxyContent != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(doxyContent);
                StreamWriter NewDoxyfile = new(@$"{doxySettings.SettingsFolder}\{doxySettings.SettingsFile}");

                NewDoxyfile.Write(sb.ToString());
                NewDoxyfile.Close();
            }
            else
                Debug.Log("Could not build file due to null input");
        }
    }
}