using System;
using System.Windows.Forms;

namespace Frontend.Forms
{
    public partial class DATFileViewerFindForm : Form
    {
        public string SearchTerm { get; private set; }
        public bool UseRegularExpressions { get; private set; }

        public DATFileViewerFindForm()
        {
            InitializeComponent();

            // Load the previous form's state
            var settings = ProjectSettingsManager.DATFileViewerSettings.FindDialogSettings;
            searchTermTextBox.Text = settings.SearchTerm;
            useRegularExpressionsCheckbox.Checked = settings.UseRegularExpressions;
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            HandleFindButtonPressed();
        }

        private void HandleFindButtonPressed()
        {
            SearchTerm = searchTermTextBox.Text;
            UseRegularExpressions = useRegularExpressionsCheckbox.Checked;

            // Close the form since we're done now
            CloseForm();
        }

        private void DATFileViewerFindForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseForm();
            }
        }

        private void searchTermTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var pressedKeyCode = e.KeyCode;

            if (pressedKeyCode == Keys.Return || pressedKeyCode == Keys.Enter)
            {
                HandleFindButtonPressed();
            }
        }

        private void CloseForm()
        {
            Close();
        }

        private void DATFileViewerFindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the current state of the form
            var settings = ProjectSettingsManager.DATFileViewerSettings.FindDialogSettings;
            settings.SearchTerm = searchTermTextBox.Text;
            settings.UseRegularExpressions = useRegularExpressionsCheckbox.Checked;

            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }
    }
}
