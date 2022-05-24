using System;
using System.Globalization;
using System.Windows.Forms;
using DROMsM.ProgramSettings;
using Frontend;

namespace DROMsM.Forms
{
    public partial class MainPreferencesForm : Form
    {
        private readonly ProgramSettings_Main mainSettings;
        private readonly ProgramSettings_DATFileViewer datFileViewerSettings;
        private readonly ProgramSettings_LaunchBox launchBoxSettings;

        public MainPreferencesForm()
        {
            InitializeComponent();

            mainSettings = ProjectSettingsManager.MainSettings;
            datFileViewerSettings = ProjectSettingsManager.DATFileViewerSettings;
            launchBoxSettings = ProjectSettingsManager.LaunchBoxSettings;

            // Main Operations Options
            allowedSimilarityValueTextBox.Text = mainSettings.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
            matchUsingGamelistXMLNameCheckbox.Checked = mainSettings.MatchUsingGameListXMLName;
            autoExpandAfterOperationsCheckbox.Checked = mainSettings.AutoExpandTreeViewsAfterOperations;

            // Dat File Viewer Options
            datFileViewer_OnlyShowUsedColumns.Checked = datFileViewerSettings.OnlyShowUsedColumns;

            // LaunchBox
            launchBox_BasePathTextBox.Text = launchBoxSettings.LaunchBoxPath;
        }

        private void allowedSimilarityValueTextBox_Leave(object sender, EventArgs e)
        {
            var allowedSimilarityValueTextBoxText = allowedSimilarityValueTextBox.Text;
            if (!float.TryParse(allowedSimilarityValueTextBox.Text, out var allowedSimilarityValue))
            {
                Logger.Log($"Unable to parse {allowedSimilarityValueTextBoxText} as an Allowed Similarity Value.  Numbers only.");

                allowedSimilarityValueTextBox.Text = mainSettings.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
                return;
            }

            allowedSimilarityValue = allowedSimilarityValue.Clamp(0, 1);
            allowedSimilarityValueTextBox.Text = allowedSimilarityValue.ToString(CultureInfo.InvariantCulture);

            mainSettings.AllowedSimilarityValue = allowedSimilarityValue;
        }

        private void MainPreferencesForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void SaveOptions()
        {
            // Main Operations Options
            mainSettings.MatchUsingGameListXMLName = matchUsingGamelistXMLNameCheckbox.Checked;
            mainSettings.AutoExpandTreeViewsAfterOperations = autoExpandAfterOperationsCheckbox.Checked;

            // DAT File Viewer Options
            datFileViewerSettings.OnlyShowUsedColumns = datFileViewer_OnlyShowUsedColumns.Checked;

            // LaunchBox Options
            launchBoxSettings.LaunchBoxPath = launchBox_BasePathTextBox.Text;

            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.LaunchBox);
        }

        private void MainPreferencesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOptions();
        }

        private void launchBox_BasePath_BrowseButton_Click(object sender, EventArgs e)
        {
            ShowLaunchBoxBasePathFolderBrowserDialog();
        }

        private void launchBox_BasePathTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            ShowLaunchBoxBasePathFolderBrowserDialog();
        }

        private void ShowLaunchBoxBasePathFolderBrowserDialog()
        {
            var folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = launchBoxSettings.LaunchBoxPath
            };

            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var selectedPath = folderBrowserDialog.SelectedPath;
                launchBox_BasePathTextBox.Text = selectedPath;

                launchBoxSettings.LaunchBoxPath = selectedPath;
            }
        }
    }
}
