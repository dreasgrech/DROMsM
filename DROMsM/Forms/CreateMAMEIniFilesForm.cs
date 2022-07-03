using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DROMsM.ProgramSettings;
using DRomsMUtils;
using Frontend;

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
        }

        public CreateMAMEIniFilesForm(IList datFileMachineList):this()
        {
            machinesList = datFileMachineList;

            totalRomsSelectedLabel.Text = $"{machinesList.Count} roms selected.";
        }

        private void createINIFilesButton_Click(object sender, EventArgs e)
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

                chosenDirectory = createMAMEIniFilesSettings.LastSelectedIniCreationPath = folderBrowserDialog.SelectedPath;
            }

            var overwriteExistingIniFiles = overwriteExistingIniFilesOption.Checked;

            // Show a confirmation before creating any files
            var confirmationMessage = $"Are you sure you want to create {machinesList.Count} ini files in {chosenDirectory}?{Environment.NewLine}" +
                                      $"{Environment.NewLine}" +
                                      $"Existing files will{(!overwriteExistingIniFiles?" not":"")} be overwritten.";
            var confirmResult = MessageBoxOperations.ShowConfirmation(confirmationMessage, $"Creating {machinesList.Count} ini files");
            if (!confirmResult)
            {
                return;
            }

            var contentForIniFilesText = contentForIniFiles.Text;
            int totalFilesCreated = 0;

            var listForParallel = machinesList.Cast<DATFileMachine>();
            Parallel.ForEach(listForParallel, datFileMachine =>
            {
                var fileName = $"{datFileMachine.Name}.ini";

                try
                {
                    var filePath = FileUtilities.CombinePath(chosenDirectory, fileName);
                    var iniFileAlreadyExists = FileUtilities.FileExists(filePath);
                    if (iniFileAlreadyExists)
                    {
                        if (!overwriteExistingIniFiles)
                        {
                            Logger.LogError($"Skipping {fileName} because the file already exists");
                            // continue;
                            return;
                        }
                    }

                    // Create the ini file
                    FileUtilities.WriteAllText(filePath, contentForIniFilesText);
                    Interlocked.Increment(ref totalFilesCreated);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"An error has occured while trying to create the ini file: {fileName}{Environment.NewLine}{ex.Message}{Environment.NewLine}");
                }
            });

            if (totalFilesCreated == 0)
            {
                var errorMessage = "Unable to create any ini files";
                MessageBoxOperations.ShowError(errorMessage, "No ini files created");
                Logger.Log(errorMessage);

                return;
            }

            var successMessage = $"Successfully created {totalFilesCreated} ini files";
            MessageBoxOperations.ShowInformation(successMessage, $"Created {totalFilesCreated} files");
            Logger.Log(successMessage);
        }

        private void CreateMAMEIniFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save our options before closing
            createMAMEIniFilesSettings.OverwriteExistingIniFiles = overwriteExistingIniFilesOption.Checked;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }
    }
}
