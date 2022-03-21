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
        private readonly OperationsOptions mainPreferences;

        public MainPreferencesForm()
        {
            InitializeComponent();
            mainPreferences = OperationsOptions.Instance;
        }

        private void matchUsingGamelistXMLNameCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxValue = matchUsingGamelistXMLNameCheckbox.Checked;

            mainPreferences.MatchUsingGameListXMLName = checkboxValue;
            ProjectSettingsManager.SaveBool(ProjectSettings.MatchUsingGameListXMLName, checkboxValue);
        }

        private void autoExpandAfterOperationsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxValue = autoExpandAfterOperationsCheckbox.Checked;

            mainPreferences.AutoExpandAfterOperations = checkboxValue;
            ProjectSettingsManager.SaveBool(ProjectSettings.AutoExpandTreeViewsAfterOperations, checkboxValue);
        }

        private void allowedSimilarityValueTextbox_Leave(object sender, EventArgs e)
        {
            var allowedSimilarityValueTextBoxText = allowedSimilarityValueTextbox.Text;
            if (!double.TryParse(allowedSimilarityValueTextbox.Text, out var allowedSimilarityValue))
            {
                Logger.Log($"Unable to parse {allowedSimilarityValueTextBoxText} as an Allowed Similarity Value.  Numbers only.");

                // allowedSimilarityValueTextbox.Text = mainManager.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
                allowedSimilarityValueTextbox.Text = OperationsOptions.Instance.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
                return;
            }

            allowedSimilarityValue = DoubleExtensions.Clamp(allowedSimilarityValue, 0, 1);
            allowedSimilarityValueTextbox.Text = allowedSimilarityValue.ToString(CultureInfo.InvariantCulture);

            // mainManager.AllowedSimilarityValue = allowedSimilarityValue;
            OperationsOptions.Instance.AllowedSimilarityValue = allowedSimilarityValue;
            Logger.Log($"Updating the Allowed Similarity Value to {allowedSimilarityValueTextBoxText}.");
        }
    }
}
