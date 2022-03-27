using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend
{
    public partial class MainPreferencesForm : Form
    {
        private readonly ProgramSettings_Main mainSettings;

        public MainPreferencesForm()
        {
            InitializeComponent();

            mainSettings = ProjectSettingsManager.MainSettings;
            allowedSimilarityValueTextbox.Text = mainSettings.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
            matchUsingGamelistXMLNameCheckbox.Checked = mainSettings.MatchUsingGameListXMLName;
            autoExpandAfterOperationsCheckbox.Checked = mainSettings.AutoExpandTreeViewsAfterOperations;
        }

        private void allowedSimilarityValueTextbox_Leave(object sender, EventArgs e)
        {
            // var settings = ProjectSettingsManager.MainSettings;

            var allowedSimilarityValueTextBoxText = allowedSimilarityValueTextbox.Text;
            if (!float.TryParse(allowedSimilarityValueTextbox.Text, out var allowedSimilarityValue))
            {
                Logger.Log($"Unable to parse {allowedSimilarityValueTextBoxText} as an Allowed Similarity Value.  Numbers only.");

                allowedSimilarityValueTextbox.Text = mainSettings.AllowedSimilarityValue.ToString();
                return;
            }

            allowedSimilarityValue = FloatExtensions.Clamp(allowedSimilarityValue, 0, 1);
            allowedSimilarityValueTextbox.Text = allowedSimilarityValue.ToString(CultureInfo.InvariantCulture);

            mainSettings.AllowedSimilarityValue = allowedSimilarityValue;
            // ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
            // Logger.Log($"Updating the Allowed Similarity Value to {allowedSimilarityValueTextBoxText}.");
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
