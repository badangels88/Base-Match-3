using System.Diagnostics;

namespace Doxygen.Editor
{
    public class ProcessRunner
    {
        private Process _process;

        public bool ProcessIsActive() => _process != null && !_process.HasExited;

        public void StartInFolder(string startPath, string runFile, string[] arguments)
        {
            var dir = System.IO.Directory.CreateDirectory(startPath);
            var fullPathWorkingFolder = dir.FullName;
            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = fullPathWorkingFolder
            };

            SetupAndStart(runFile, arguments, processStartInfo);
        }

        private void SetupAndStart(string runFile, string[] arguments, ProcessStartInfo processStartInfo)
        {
            processStartInfo.FileName = runFile;
            processStartInfo.Arguments = arguments[0];
            _process = Process.Start(processStartInfo);
        }
    }
}