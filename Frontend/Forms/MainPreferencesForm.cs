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
        public MainPreferencesForm()
        {
            InitializeComponent();

            var settings = ProjectSettingsManager.MainSettings;
            allowedSimilarityValueTextbox.Text = settings.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
            matchUsingGamelistXMLNameCheckbox.Checked = settings.MatchUsingGameListXMLName;
            autoExpandAfterOperationsCheckbox.Checked = settings.AutoExpandTreeViewsAfterOperations;
        }

        private void matchUsingGamelistXMLNameCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxValue = matchUsingGamelistXMLNameCheckbox.Checked;

            var mainSettings = ProjectSettingsManager.MainSettings;
            mainSettings.MatchUsingGameListXMLName = checkboxValue;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
        }

        private void autoExpandAfterOperationsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxValue = autoExpandAfterOperationsCheckbox.Checked;

            var mainSettings = ProjectSettingsManager.MainSettings;
            mainSettings.AutoExpandTreeViewsAfterOperations = checkboxValue;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
        }

        private void allowedSimilarityValueTextbox_Leave(object sender, EventArgs e)
        {
            var settings = ProjectSettingsManager.MainSettings;

            var allowedSimilarityValueTextBoxText = allowedSimilarityValueTextbox.Text;
            if (!float.TryParse(allowedSimilarityValueTextbox.Text, out var allowedSimilarityValue))
            {
                Logger.Log($"Unable to parse {allowedSimilarityValueTextBoxText} as an Allowed Similarity Value.  Numbers only.");

                allowedSimilarityValueTextbox.Text = settings.AllowedSimilarityValue.ToString();
                return;
            }

            allowedSimilarityValue = FloatExtensions.Clamp(allowedSimilarityValue, 0, 1);
            allowedSimilarityValueTextbox.Text = allowedSimilarityValue.ToString(CultureInfo.InvariantCulture);

            settings.AllowedSimilarityValue = allowedSimilarityValue;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
            Logger.Log($"Updating the Allowed Similarity Value to {allowedSimilarityValueTextBoxText}.");
        }
    }
}
