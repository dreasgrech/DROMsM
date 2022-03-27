using System;
using System.Reflection;
using DRomsMUtils;
using Frontend;

namespace DROMsM
{
    /// <summary>
    /// chdman args from https://i12bretro.github.io/tutorials/0323.html
    /// </summary>
    public class CHDMANHandler
    {
        private const string CHDMANFilename = @"chdman.exe";
        // private const string CHDMANRelativeFilePath = @"chdman\chdman.exe";
        private const string CHDMANRelativeFilePath = @"chdman\" + CHDMANFilename;

        private readonly string CHDMANFullFilePath;

        private string currentOperationCHDMANDirectoryName;
        private string currentOperationCHDMANFilePath;

        private bool runningOperation;

        public CHDMANHandler()
        {
            CHDMANFullFilePath = FileUtilities.CombinePath(FileUtilities.GetDirectory(Assembly.GetExecutingAssembly().Location), CHDMANRelativeFilePath);
        }

        public bool StartOperation(string workingDirectory)
        {
            try
            {
                if (runningOperation)
                {
                    return false;
                }

                // Make a copy of chdman to our working directory
                var destinationFilepath = FileUtilities.CombinePath(workingDirectory, CHDMANFilename);

                Logger.Log($"Starting chdman operation.  Copying chdman from {CHDMANFullFilePath} to {destinationFilepath}");

                var fileCopied = FileUtilities.CopyFile(CHDMANFullFilePath, destinationFilepath, true);
                if (!fileCopied)
                {
                    Logger.LogError($"Unable to start chdman operation.  Could not copy chdman from {CHDMANFullFilePath} to {destinationFilepath}");
                    return false;
                }

                currentOperationCHDMANFilePath = destinationFilepath;
                currentOperationCHDMANDirectoryName = FileUtilities.GetNameOfDirectory(FileUtilities.GetDirectory(destinationFilepath));

                runningOperation = true;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool StopOperation()
        {
            try
            {
                if (!runningOperation)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(currentOperationCHDMANFilePath))
                {
                    return false;
                }

                Logger.Log($"Stopping chdman operation.  Deleting {currentOperationCHDMANFilePath}");

                // Delete the chdman we created when we started the operation
                var fileDeleted = FileUtilities.DeleteFile(currentOperationCHDMANFilePath);
                if (!fileDeleted)
                {
                    return false;
                }

                currentOperationCHDMANFilePath = null;
                currentOperationCHDMANDirectoryName = null;

                runningOperation = false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string ConvertToCHD(string cueFilePath, string chdOutputDirectory)
        {
            if (!runningOperation)
            {
                return null;
            }

            /****************************/
            return RunCHDMan("createcd", cueFilePath, chdOutputDirectory, "chd");
            /****************************/

            /*
            var cueFilePathDirectory = FileUtilities.GetDirectory(cueFilePath);

            var currentOperationInRootDirectory =
                string.Equals(FileUtilities.GetNameOfDirectory(cueFilePathDirectory), currentOperationCHDMANDirectoryName) ||
                string.Equals(FileUtilities.GetNameOfDirectory(chdOutputDirectory), currentOperationCHDMANDirectoryName);

            // if (FileUtilities.PathExceedsWindowsMaxFilePathLength(cueFilePath))

            // Only rename the directory if we are not working in the directory where chdman is!
            var renamingDirectory = !currentOperationInRootDirectory;

            // Make sure the output directory exists
            FileUtilities.CreateDirectory(chdOutputDirectory);

            var originalCueFileName = FileUtilities.GetFileName(cueFilePath);

            var chdOutputFilename = $"{FileUtilities.GetFileNameWithoutExtension(cueFilePath)}.chd";
            var chdOutputFilePath = FileUtilities.CombinePath(chdOutputDirectory, chdOutputFilename);

            // Delete the output file it it already exists
            FileUtilities.DeleteFile(chdOutputFilePath);

            // Rename the directories
            var renamedCueDirectory = cueFilePathDirectory;
            var renamedCHDDirectory = chdOutputDirectory;
            if (renamingDirectory)
            {
                renamedCueDirectory = TemporarilyRenameDirectory(cueFilePath);
                renamedCHDDirectory = TemporarilyRenameDirectory(chdOutputFilePath);
            }

            // Rename the files
            var renamedCueDirectoryWithOriginalCueFileNamePath = FileUtilities.CombinePath(renamedCueDirectory, originalCueFileName);
            var renamedCueFilePath = TemporarilyRenameFile(renamedCueDirectoryWithOriginalCueFileNamePath);
            var renamedCHDDirectoryWithChdOutputFileName = FileUtilities.CombinePath(renamedCHDDirectory, chdOutputFilename);
            var renamedCHDFilePath = TemporarilyRenameFile(renamedCHDDirectoryWithChdOutputFileName);

            var arguments = $"createcd -i \"{renamedCueFilePath}\" -o \"{renamedCHDFilePath}\" -f";

            StartProcess(arguments);

            // Rename the files back
            FileUtilities.MoveFile(renamedCueFilePath, renamedCueDirectoryWithOriginalCueFileNamePath);
            FileUtilities.MoveFile(renamedCHDFilePath, renamedCHDDirectoryWithChdOutputFileName);

            // Rename the directories back
            if (renamingDirectory)
            {
                // var cueFilePathDirectory = FileUtilities.GetDirectory(cueFilePath);
                var chdOutputFilePathDirectory = FileUtilities.GetDirectory(chdOutputFilePath);
                FileUtilities.MoveDirectory(renamedCueDirectory, cueFilePathDirectory);
                FileUtilities.MoveDirectory(renamedCHDDirectory, chdOutputFilePathDirectory);
            }

            return chdOutputFilePath;
            */
        }

        public string ConvertToCueBin(string chdFilePath, string outputDirectory)
        {
            if (!runningOperation)
            {
                return null;
            }

            /****************************/
            return RunCHDMan("extractcd", chdFilePath, outputDirectory, "cue");
            /****************************/

            /*
            // Make sure the output directory exists
            FileUtilities.CreateDirectory(outputDirectory);

            var cueOutputFilename = $"{FileUtilities.GetFileNameWithoutExtension(chdFilePath)}.cue";
            var cueOutputFilePath = FileUtilities.CombinePath(outputDirectory, cueOutputFilename);

            // Delete the output file it it already exists
            FileUtilities.DeleteFile(cueOutputFilePath);

            var arguments = $"extractcd -i \"{chdFilePath}\" -o \"{cueOutputFilePath}\" -f";

            StartProcess(arguments);

            return cueOutputFilename;
            */
        }

        public string CombineMultipleBinsIntoOne(string cueFilePath, string outputDirectory)
        {
            if (!runningOperation)
            {
                return null;
            }

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
            // Logger.Log($"{CHDMANFullFilePath} {arguments}");
            Logger.Log($"{currentOperationCHDMANFilePath} {arguments}");

            var fileReaderProcess = new System.Diagnostics.Process()
            {
                EnableRaisingEvents = false,
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = currentOperationCHDMANFilePath,
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

        private string TemporarilyRenameDirectory(string directory)
        {
            directory = FileUtilities.GetDirectory(directory);

            var directoryName = FileUtilities.GetNameOfDirectory(directory);
            var renamedDirectoryName = $"_{directoryName.GetHashCode()}";
            var parentDirectory = FileUtilities.GetDirectoryParent(directory);
            var renamedFullPath = FileUtilities.CombinePath(parentDirectory, renamedDirectoryName);

            var directoryMoved = FileUtilities.MoveDirectory(directory, renamedFullPath);

            return renamedFullPath;
        }

        private string TemporarilyRenameFile(string filePath)
        {
            var fileExtension = FileUtilities.GetExtension(filePath);
            var renamedFilename = $"_{filePath.GetHashCode()}{fileExtension}";
            var amendedFilePath = FileUtilities.CombinePath(FileUtilities.GetDirectory(filePath), renamedFilename);
            var fileMoved = FileUtilities.MoveFile(filePath, amendedFilePath);

            return amendedFilePath;
        }

        private void FileReaderProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Logger.Log(e.Data);
        }

        public string RunCHDMan(string command, string inputFilePath, string outputDirectory, string outputFileExtension)
        {
            if (!runningOperation)
            {
                return null;
            }

            /*
            var directory = FileUtilities.GetDirectory(currentOperationCHDMANFilePath);
            inputFilePath = inputFilePath.Replace(directory, string.Empty);
            outputDirectory = outputDirectory.Replace(directory, string.Empty);
            */

            var inputFilePathDirectory = FileUtilities.GetDirectory(inputFilePath);

            var currentOperationInRootDirectory =
                string.Equals(FileUtilities.GetNameOfDirectory(inputFilePathDirectory), currentOperationCHDMANDirectoryName) ||
                string.Equals(FileUtilities.GetNameOfDirectory(outputDirectory), currentOperationCHDMANDirectoryName);

            // if (FileUtilities.PathExceedsWindowsMaxFilePathLength(cueFilePath))

            // Only rename the directory if we are not working in the directory where chdman is!
            var renamingDirectory = !currentOperationInRootDirectory;

            // Make sure the output directory exists
            FileUtilities.CreateDirectory(outputDirectory);

            var originalInputFileName = FileUtilities.GetFileName(inputFilePath);

            // var outputFilename = $"{FileUtilities.GetFileNameWithoutExtension(inputFilePath)}.chd";
            var outputFilename = $"{FileUtilities.GetFileNameWithoutExtension(inputFilePath)}.{outputFileExtension}";
            var outputFilePath = FileUtilities.CombinePath(outputDirectory, outputFilename);

            // Delete the output file it it already exists
            FileUtilities.DeleteFile(outputFilePath);

            // Rename the directories
            var renamedInputDirectory = inputFilePathDirectory;
            var renamedOutputDirectory = outputDirectory;
            if (renamingDirectory)
            {
                renamedInputDirectory = TemporarilyRenameDirectory(inputFilePath);
                renamedOutputDirectory = TemporarilyRenameDirectory(outputFilePath);
            }

            // Rename the files
            var renamedInputDirectoryWithOriginalInputFileNamePath = FileUtilities.CombinePath(renamedInputDirectory, originalInputFileName);
            var renamedInputFilePath = TemporarilyRenameFile(renamedInputDirectoryWithOriginalInputFileNamePath);
            var renamedOutputDirectoryWithOutputFileName = FileUtilities.CombinePath(renamedOutputDirectory, outputFilename);
            // var renamedOutputFilePath = TemporarilyRenameFile(renamedOutputDirectoryWithOutputFileName);
            var renamedOutputFilePath = renamedOutputDirectoryWithOutputFileName;

            // var arguments = $"createcd -i \"{renamedInputFilePath}\" -o \"{renamedOutputFilePath}\" -f";
            var arguments = $"{command} -i \"{renamedInputFilePath}\" -o \"{renamedOutputFilePath}\" -f";

            StartProcess(arguments);

            // Rename the files back
            FileUtilities.MoveFile(renamedInputFilePath, renamedInputDirectoryWithOriginalInputFileNamePath);
            //FileUtilities.MoveFile(renamedOutputFilePath, renamedOutputDirectoryWithOutputFileName);

            // Rename the directories back
            if (renamingDirectory)
            {
                var outputFilePathDirectory = FileUtilities.GetDirectory(outputFilePath);
                FileUtilities.MoveDirectory(renamedInputDirectory, inputFilePathDirectory);
                FileUtilities.MoveDirectory(renamedOutputDirectory, outputFilePathDirectory);
            }

            return outputFilePath;
        }
    }
}