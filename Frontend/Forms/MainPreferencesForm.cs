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

        public MainPreferencesForm()
        {
            InitializeComponent();

            mainSettings = ProjectSettingsManager.MainSettings;
            allowedSimilarityValueTextBox.Text = mainSettings.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
            matchUsingGamelistXMLNameCheckbox.Checked = mainSettings.MatchUsingGameListXMLName;
            autoExpandAfterOperationsCheckbox.Checked = mainSettings.AutoExpandTreeViewsAfterOperations;
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
            mainSettings.MatchUsingGameListXMLName = matchUsingGamelistXMLNameCheckbox.Checked;
            mainSettings.AutoExpandTreeViewsAfterOperations = autoExpandAfterOperationsCheckbox.Checked;

            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
        }

        private void MainPreferencesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOptions();
        }
    }
}
