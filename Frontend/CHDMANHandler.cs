using System.Reflection;
using System.Threading.Tasks;
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
            CHDMANFullFilePath = FileUtilities.CombinePath(FileUtilities.GetDirectory(Assembly.GetExecutingAssembly().Location), CHDMANRelativeFilePath);
        }

        public string ConvertToCHD(string cueFilePath, string chdOutputDirectory)
        {
            // Make sure the output directory exists
            FileUtilities.CreateDirectory(chdOutputDirectory);

            var chdOutputFilename = $"{FileUtilities.GetFileNameWithoutExtension(cueFilePath)}.chd";
            var chdOutputFilePath = FileUtilities.CombinePath(chdOutputDirectory, chdOutputFilename);

            // Delete the output file it it already exists
            FileUtilities.DeleteFile(chdOutputFilePath);

            var arguments = $"createcd -i \"{cueFilePath}\" -o \"{chdOutputFilePath}\" -f";

            StartProcess(arguments);

            return chdOutputFilePath;
        }

        public string ConvertToCueBin(string chdFilePath, string outputDirectory)
        {
            // Make sure the output directory exists
            FileUtilities.CreateDirectory(outputDirectory);

            var cueOutputFilename = $"{FileUtilities.GetFileNameWithoutExtension(chdFilePath)}.cue";
            var cueOutputFilePath = FileUtilities.CombinePath(outputDirectory, cueOutputFilename);

            // Delete the output file it it already exists
            FileUtilities.DeleteFile(cueOutputFilePath);

            var arguments = $"extractcd -i \"{chdFilePath}\" -o \"{cueOutputFilePath}\" -f";

            StartProcess(arguments);

            return cueOutputFilename;
        }

        public string CombineMultipleBinsIntoOne(string cueFilePath, string outputDirectory)
        {
            // First convert the cue/bin to chd
            var chdFilePath = ConvertToCHD(cueFilePath, outputDirectory);

            // Then convert the chd back to cue/bin 
            var newCueFilePath = ConvertToCueBin(chdFilePath, outputDirectory);

            // Remove the chd file since we're now done with it
            FileUtilities.DeleteFile(chdFilePath);

            return newCueFilePath;
        }

        private void StartProcess(string arguments)
        {
            Logger.Log($"{CHDMANFullFilePath} {arguments}");

            var fileReaderProcess = new System.Diagnostics.Process()
            {
                EnableRaisingEvents = false,
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = CHDMANFullFilePath,
                    UseShellExecute = false,
                    //RedirectStandardError = true,
                    //RedirectStandardInput = true,
                    //RedirectStandardOutput = true,
                    // CreateNoWindow = true,
                    CreateNoWindow = false,
                    // Arguments = $"\"{fileTypes}\" \"{searchLocationPath}\" \"{valuesCombined}\" \"{inverseSearch}\""
                    Arguments = arguments
                }
            };

            // Debug.Log(fileReaderProcess.StartInfo.Arguments);

            fileReaderProcess.OutputDataReceived += FileReaderProcess_OutputDataReceived;
            fileReaderProcess.Start();
            // fileReaderProcess.BeginOutputReadLine();
            fileReaderProcess.WaitForExit();
            fileReaderProcess.OutputDataReceived -= FileReaderProcess_OutputDataReceived;

            /*
            var stdout = fileReaderProcess.StandardOutput;
            while (!stdout.EndOfStream)
            {
                Logger.Log(stdout.ReadLine());
            }
            */
        }

        private void FileReaderProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Logger.Log(e.Data);
        }
    }
}