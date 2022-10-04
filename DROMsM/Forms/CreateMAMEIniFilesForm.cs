// #define SINGLE_THREAD

using DROMsM.ProgramSettings;
using DROMsMUtils;
using Frontend;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DIOUtils;

namespace DROMsM.Forms
{
    public partial class CreateMAMEIniFilesForm : Form
    {
        private readonly IList machinesList;

        private readonly ProgramSettings_DatFileViewer_CreateMAMEIniFiles createMAMEIniFilesSettings;

        public CreateMAMEIniFilesForm()
        {
            InitializeComponent();

            var datFileViewerSettings = ProjectSettingsManager.DATFileViewerSettings;
            createMAMEIniFilesSettings = datFileViewerSettings.CreateMAMEIniFilesSettings;

            overwriteExistingIniFilesOption.Checked = createMAMEIniFilesSettings.OverwriteExistingIniFiles;
            onlyUpdateDifferentValuesOption.Checked = createMAMEIniFilesSettings.OnlyUpdateDifferentValues;

            HandleOptionsVisibility();
        }

        public CreateMAMEIniFilesForm(IList datFileMachineList) : this()
        {
            machinesList = datFileMachineList;

            totalRomsSelectedLabel.Text = $"{machinesList.Count} roms selected.";
        }

        private void CreateFiles()
        {
            // Show a folder browser dialog so we can select a folder where to save the ini files to
            string chosenDirectory;
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = createMAMEIniFilesSettings.LastSelectedIniCreationPath;
                var folderBrowserDialogResult = folderBrowserDialog.ShowDialog();
                if (folderBrowserDialogResult != DialogResult.OK)
                {
                    return;
                }

                chosenDirectory = folderBrowserDialog.SelectedPath;

                createMAMEIniFilesSettings.LastSelectedIniCreationPath = chosenDirectory;
                ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
            }

            var overwriteExistingIniFiles = overwriteExistingIniFilesOption.Checked;
            var onlyUpdateDifferentValues = onlyUpdateDifferentValuesOption.Checked;

            // Show a confirmation before creating any files
            var confirmationMessage = $"Are you sure you want to create {machinesList.Count} ini files in {chosenDirectory}?" + $"{Environment.NewLine}" +
                                      $"{Environment.NewLine}" +
                                      $"Existing files will{(!overwriteExistingIniFiles ? " not" : "")} be overwritten.";

            if (onlyUpdateDifferentValues)
            {
                confirmationMessage = $"{confirmationMessage}" + $"{Environment.NewLine}" +
                                      $"{Environment.NewLine}" +
                                      $"Only different values will be updated in existing files.{Environment.NewLine}" +
                                      "# and ; comments and [Section]s will be ignored from your inputted text.";
            }

            var confirmResult = MessageBoxOperations.ShowConfirmation(confirmationMessage, $"Creating {machinesList.Count} ini files");
            if (!confirmResult)
            {
                return;
            }

            var contentForIniFilesText = contentForIniFiles.Text;

            // Remove the newline characters
            // https://stackoverflow.com/questions/5843495/what-does-m-character-mean-in-vim
            // https://stackoverflow.com/questions/873043/removing-carriage-return-and-new-line-from-the-end-of-a-string-in-c-sharp
            contentForIniFilesText = contentForIniFilesText.TrimEnd('\r', '\n');

            int totalFilesCreated = 0,
                totalFilesUpdated = 0;

            var mameIniFileHandler = new MAMEIniFileHandler();
            var contentForIniFilesLines = contentForIniFiles.Lines;
            var contentForIniFilesLinesMAMEIniFile = mameIniFileHandler.ParseMAMEIniLines(contentForIniFilesLines);
            if (contentForIniFilesLinesMAMEIniFile == null)
            {
                return;
            }

            var listForParallel = machinesList.Cast<DATFileMachine>();
#if SINGLE_THREAD
            foreach (var datFileMachine in listForParallel)
#else
            Parallel.ForEach(listForParallel, datFileMachine =>
#endif
            {
                var fileName = $"{datFileMachine.Name}.ini";

                try
                {
                    var filePath = FileUtilities.CombinePath(chosenDirectory, fileName);
                    var iniFileAlreadyExists = FileUtilities.FileExists(filePath);
                    if (iniFileAlreadyExists)
                    {
                        // If we're not overwriting existing ini files, then don't do anything with this file
                        if (!overwriteExistingIniFiles)
                        {
                            Logger.LogError($"Skipping {fileName} because the file already exists");
#if SINGLE_THREAD
                            continue;
#else
                            return;
#endif
                        }

                        // If we're only updating values for existing ini files, then read the existing file and update the values
                        if (onlyUpdateDifferentValues)
                        {
                            // Read the existing ini file data
                            var iniFile = mameIniFileHandler.ParseMAMEIniFile(filePath);

                            // Apply the changes to the existing ini file data
                            iniFile.ApplyChanges(contentForIniFilesLinesMAMEIniFile);

                            // Write the updated ini file to disk
                            mameIniFileHandler.SaveMAMEIniFileToDisk(iniFile, filePath);

                            Interlocked.Increment(ref totalFilesUpdated);
#if SINGLE_THREAD
                            continue;
#else
                            return;
#endif
                        }
                    }

                    // Create the ini file on the disk and write the text in it
                    FileUtilities.WriteAllText(filePath, contentForIniFilesText);

                    Interlocked.Increment(ref totalFilesCreated);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"An error has occured while trying to create the ini file: {fileName}{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                }
#if SINGLE_THREAD
            }
#else
            });
#endif

            var totalFilesTouched = totalFilesCreated + totalFilesUpdated;
            if (totalFilesTouched == 0)
            {
                var errorMessage = "Unable to create or update any ini files";
                MessageBoxOperations.ShowError(errorMessage, "No ini files created or updated");
                Logger.Log(errorMessage);

                return;
            }

            string successMessage = string.Empty;
            if (totalFilesCreated > 0)
            {
                successMessage = $"Created {totalFilesCreated} ini files{Environment.NewLine}";
            }

            if (totalFilesUpdated > 0)
            {
                successMessage = $"{successMessage}Updated existing {totalFilesUpdated} ini files";
            }

            MessageBoxOperations.ShowInformation(successMessage, $"Created {totalFilesCreated} files and updated {totalFilesUpdated} existing files");
            Logger.Log(successMessage);
        }

        private void HandleOptionsVisibility()
        {
            var overwriteExistingIniFiles = overwriteExistingIniFilesOption.Checked;

            // Only have the "Only Update Different Values" option enabled if the "Overwrite Existing ini files" is enabled
            onlyUpdateDifferentValuesOption.Enabled = overwriteExistingIniFiles;

            // If we're not overwriting existing files, don't allow the "Only Update Different Values" option to be checked
            if (!overwriteExistingIniFiles)
            {
                onlyUpdateDifferentValuesOption.Checked = false;
            }
        }

        private void createINIFilesButton_Click(object sender, EventArgs e)
        {
            CreateFiles();
        }

        private void overwriteExistingIniFilesOption_CheckedChanged(object sender, EventArgs e)
        {
            HandleOptionsVisibility();
        }

        private void overwriteExistingIniFilesOptionLabel_Click(object sender, EventArgs e)
        {
            overwriteExistingIniFilesOption.Checked = !overwriteExistingIniFilesOption.Checked;
        }

        private void onlyUpdateDifferentValuesOptionLabel_Click(object sender, EventArgs e)
        {
            if (onlyUpdateDifferentValuesOption.Enabled)
            {
                onlyUpdateDifferentValuesOption.Checked = !onlyUpdateDifferentValuesOption.Checked;
            }
        }

        private void CreateMAMEIniFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save our options before closing
            createMAMEIniFilesSettings.OverwriteExistingIniFiles = overwriteExistingIniFilesOption.Checked;
            createMAMEIniFilesSettings.OnlyUpdateDifferentValues = onlyUpdateDifferentValuesOption.Checked;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }
    }
}
