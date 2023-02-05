using System.IO;
using UnityEditor;
using UnityEngine;

namespace Doxygen.Editor
{
    public class DoxyWindow : EditorWindow
    {
        private string msgToUser = "";
        private DoxySettings doxySettings = new DoxySettings();
        private Vector2 scroll = new Vector2();
        private ProcessRunner curentProcess = null;

        private void OnEnable() => doxySettings.Load();

        [MenuItem("Window/Doxygen documentation tool")]
        private static void ShowWindow() => EditorWindow.GetWindow(typeof(DoxyWindow), false, "DoxygenTool");

        private void OnGUI()
        {
            this.scroll = GUILayout.BeginScrollView(scroll);
            DisplayMsgToUser();
            GenerateViewEditGUI();
            GUILayout.EndScrollView();
        }

        private void DisplayMsgToUser()
        {
            if (!string.IsNullOrWhiteSpace(msgToUser))
            {
                GUIStyle myStyle = new GUIStyle();
                myStyle.normal.textColor = Color.red;
                myStyle.wordWrap = true;
                GUILayout.Label(msgToUser, myStyle);
            }
        }

        private void GenerateViewEditGUI()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Generate documentation"))
                TryGenerateDocumentation();
            GUILayout.Space(10);
            GUI.enabled = ExistsDocumentation();
            if (GUILayout.Button("Open documentation"))
                OpenDocumentation();
            GUI.enabled = true;
            if (GUILayout.Button("Edit Settings"))
                EditDoxygen();
            GUILayout.EndVertical();
        }

        private bool ExistsDocumentation()
        {
            string path = $@"{doxySettings.ActiveDocumentationFolder}\Wiki\index.html";
            return File.Exists(path);
        }

        private void OpenDocumentation()
        {
            string path = $@"{doxySettings.ActiveDocumentationFolder}\Wiki\index.html";
            if (File.Exists(path))
            {
                msgToUser = "";
                System.Diagnostics.Process.Start(path);
            }
        }

        private void EditDoxygen()
        {
            var arg = @$"{doxySettings.SettingsFolder}\{doxySettings.SettingsFile}";
            var doxyWizard = doxySettings.FullDoxygenPath.Replace("doxygen.exe", "doxywizard.exe");
            doxyWizard = doxyWizard.Replace('/', '\\');

            if (File.Exists(doxyWizard) && File.Exists(arg))
            {
                msgToUser = "";
                System.Diagnostics.Process.Start("\"" + doxyWizard + "\"", "\"" + arg + "\"");
            }
            else
            {
                msgToUser = "";
                if (!File.Exists(doxyWizard))
                    msgToUser = "doxywizard.exe is not found at:" + doxyWizard;
                if (!File.Exists(arg))
                    msgToUser += "Did not find the doxyfile at:" + arg;
            }
            DisplayMsgToUser();
        }

        private void TryGenerateDocumentation()
        {
            if ((curentProcess == null || !(curentProcess.ProcessIsActive())))
            {
                DoxyFile.UpdateDoxyFile(doxySettings);
                GenerateDocumentation();
            }
            else
                msgToUser = "Can't generate documentation. One process is already running.";
        }

        private void GenerateDocumentation()
        {
            if (Directory.Exists($@"{doxySettings.ActiveDocumentationFolder}\Wiki"))
                Directory.Delete($@"{doxySettings.ActiveDocumentationFolder}\Wiki", true);
            string settingsFolder = $"\"{doxySettings.SettingsFolder}\\{doxySettings.SettingsFile}\"";
            string doxygenPath = doxySettings.FullDoxygenPath.Replace('/', '\\');

            if (DoxyFilesInPlace())
            {
                curentProcess = new ProcessRunner();
                curentProcess.StartInFolder(doxySettings.ActiveDocumentationFolder, doxygenPath, new string[] { settingsFolder });
            }
        }

        private bool DoxyFilesInPlace()
        {
            msgToUser = "";
            bool allFine = true;

            if (!File.Exists(@$"{doxySettings.SettingsFolder}\{doxySettings.SettingsFile}"))
            {
                allFine = false;
                msgToUser += "\nDeveloperDoxyfile is missing at: " + @$"{doxySettings.SettingsFolder}\{doxySettings.SettingsFile}";
            }
            if (!File.Exists(doxySettings.FullDoxygenPath))
            {
                allFine = false;
                msgToUser += "\ndoxygen.exe is missing at: " + doxySettings.FullDoxygenPath;
            }
            if (allFine == false)
            {
                msgToUser = "Can not make documentation:" + msgToUser;
                return false;
            }
            else
            {
                msgToUser = "";
                return true;
            }
        }
    }
}