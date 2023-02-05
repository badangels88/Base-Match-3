using System.IO;
using UnityEngine;

namespace Doxygen.Editor
{
    public struct DoxySettings
    {
        public string FullDoxygenPath { get; set; }
        public string ActiveDocumentationFolder { get; set; }
        public string ProjecSourceFolder { get; set; }
        public string ProjectName { get; set; }
        public string ProjectBrief { get; set; }
        public string ProjectVersionNumber { get; set; }
        public string SettingsFolder { get; set; }
        public string SettingsFile { get; set; }

        public void Load()
        {
            this.SettingsFile = "DeveloperDoxyfile";
            this.SettingsFolder = $@"{Directory.GetCurrentDirectory()}\Assets\Doxygen\Editor\Resources";
            this.ProjecSourceFolder = $@"{Directory.GetCurrentDirectory()}\Assets\Scripts";
            this.ProjectName = Application.productName;
            this.ProjectVersionNumber = Application.version;
            this.ActiveDocumentationFolder = Directory.GetCurrentDirectory();
            this.FullDoxygenPath = @"C:\Program Files\doxygen\bin\doxygen.exe";
        }
    }
}