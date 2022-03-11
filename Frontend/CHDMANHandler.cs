using System.IO;
using System.Reflection;
using DRomsMUtils;

namespace Frontend
{
    /// <summary>
    /// chdman args from https://i12bretro.github.io/tutorials/0323.html
    /// </summary>
    public class CHDMANHandler
    {
        private const string CHDMANRelativeFilePath = @"chdman\chdman.exe";

        private readonly string CHDMANFullFilePath;

        public CHDMANHandler()
        {
            CHDMANFullFilePath = FileUtilities.CombinePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CHDMANRelativeFilePath);
        }

        public string ConvertToCHD(string cueFilePath)
        {
            var chdOutputDirectory = FileUtilities.GetDirectoryFullname(cueFilePath);
            var chdOutputFilename = $"{FileUtilities.GetFileNameWithoutExtension(cueFilePath)}.chd";
            var chdOutputFilePath = FileUtilities.CombinePath(chdOutputDirectory, chdOutputFilename);

            var arguments = $"createcd -i \"{cueFilePath}\" -o \"{chdOutputFilePath}\"";

            /*
            StartProcess(arguments);
            */

            return chdOutputFilePath;
        }

        private void StartProcess(string arguments)
        {
            var fileReaderProcess = new System.Diagnostics.Process()
            {
                EnableRaisingEvents = false,
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = CHDMANFullFilePath,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    // Arguments = $"\"{fileTypes}\" \"{searchLocationPath}\" \"{valuesCombined}\" \"{inverseSearch}\""
                    Arguments = arguments
                }
            };

            // Debug.Log(fileReaderProcess.StartInfo.Arguments);

            // fileReaderProcess.OutputDataReceived += Process_OutputDataReceived;
            fileReaderProcess.Start();
            fileReaderProcess.BeginOutputReadLine();
            fileReaderProcess.WaitForExit();
            // fileReaderProcess.OutputDataReceived -= Process_OutputDataReceived;

            // fileReaderProcess = null;
        }
    }
}