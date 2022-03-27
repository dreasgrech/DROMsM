﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Close();
        }

        private void searchTermTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var pressedKeyCode = e.KeyCode;

            if (pressedKeyCode == Keys.Return || pressedKeyCode == Keys.Enter)
            {
                HandleFindButtonPressed();
            }
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
