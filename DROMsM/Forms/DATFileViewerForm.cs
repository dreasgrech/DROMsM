using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using DRomsMUtils;
using Frontend;

namespace DROMsM.Forms
{
    public partial class DATFileViewerForm : Form
    {
        public bool WantsToOpenNewFile { get; private set; }
        private DATFileMachineVirtualListDataSource datFileMachineVirtualListDataSource;

        private DATFile currentDATFile;
        private bool showingWorkingColors;

        public DATFileViewerForm()
        {
            InitializeComponent();

            var settings = ProjectSettingsManager.DATFileViewerSettings;

            if (settings.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }

            showingWorkingColors = settings.ShowColors;
            showColorsToolStripMenuItem.Checked = showingWorkingColors;

            var showingGridLines = settings.ShowGridLines;
            olvDatFileListView.GridLines = showingGridLines;
            showGridLinesToolStripMenuItem.Checked = showingGridLines;

            // Restore the grid's state
            var savedState = settings.SavedState;
            if (savedState != null)
            {
                var restored = olvDatFileListView.RestoreState(savedState);
                if (!restored)
                {
                    Logger.LogError($"Unable to restore the DAT File Viewer's grid state");
                }
            }

            olvIsCloneColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
            olvBIOSColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
            olvRequireCHDsColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
            olvMechanicalColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
            olvSamplesColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
        }

        public void ProcessDATFile(string datFilePath)
        {
            var datFileHandler = new U8XMLDATFileHandler();
            // var datFileHandler = new XmlReaderDATFileHandler();
            var datFile = datFileHandler.ParseDATFile(datFilePath);

            datFileMachineVirtualListDataSource = new DATFileMachineVirtualListDataSource(olvDatFileListView);
            datFileMachineVirtualListDataSource.AddObjects(datFile.Machines);
            olvDatFileListView.VirtualListDataSource = datFileMachineVirtualListDataSource;

            // Expand the columns
            // datFileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            Text = $@"DAT File Viewer - {datFilePath}";
            datFilePathLabel.Text = datFilePath;
            totalSetsLabel.Text = datFile.TotalMachines.ToString();
            buildLabel.Text = datFile.Build;

            currentDATFile = datFile;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDATFile == null)
            {
                Logger.LogError("There's no currently stored DAT file!");
                return;
            }

            var saveAsFilePath = FormFileOperations.ShowSaveFileDialog(FormFileOperations.SaveDialog_DATFilesFilter);
            if (string.IsNullOrEmpty(saveAsFilePath))
            {
                return;
            }

            var fullXMLFileStringBuilder = new StringBuilder();

            // Write the declaration
            var declaration = currentDATFile.XMLDeclaration;
            if (!string.IsNullOrEmpty(declaration))
            {
                fullXMLFileStringBuilder.AppendLine(declaration);
            }

            // Write the doc type
            var docType = currentDATFile.XMLDocType;
            if (!string.IsNullOrEmpty(docType))
            {
                fullXMLFileStringBuilder.AppendLine(docType);
            }

            // Write the root node opening tag
            var rootNodeXMLOpeningTag = currentDATFile.GetRootNodeXMLOpeningTag();
            fullXMLFileStringBuilder.AppendLine(rootNodeXMLOpeningTag);

            // Write all the filtered nodes
            var filteredObjectList = datFileMachineVirtualListDataSource.FilteredObjectList;
            foreach (DATFileMachine datFileMachine in filteredObjectList)
            {
                var xmlValue = datFileMachine.XMLValue;
                fullXMLFileStringBuilder.Append("\t");
                fullXMLFileStringBuilder.AppendLine(xmlValue);
            }

            // Write the root node closing tag
            var rootNodeXMLClosingTag = currentDATFile.GetRootNodeXMLClosingTag();
            fullXMLFileStringBuilder.AppendLine(rootNodeXMLClosingTag);

            var full = fullXMLFileStringBuilder.ToString();
            FileUtilities.WriteAllText(saveAsFilePath, full);
            MessageBoxOperations.ShowInformation($"File successfully saved to {saveAsFilePath}", "DAT File saved");
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exportFilePath = FormFileOperations.ShowSaveFileDialog(FormFileOperations.SaveDialog_CSVFilesFilter);
            if (string.IsNullOrEmpty(exportFilePath))
            {
                return;
            }

            var visibleColumns = olvDatFileListView.Columns;
            var classMap = new DATFileMachineCSVClassMap();
            classMap.ToggleColumn(m => m.Name, visibleColumns.Contains(olvSetColumn), olvSetColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Description, visibleColumns.Contains(olvNameColumn), olvNameColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Year, visibleColumns.Contains(olvYearColumn), olvYearColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Manufacturer, visibleColumns.Contains(olvManufacturer), olvManufacturer.DisplayIndex);
            classMap.ToggleColumn(m => m.Status, visibleColumns.Contains(olvStatusColumn), olvStatusColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Emulation, visibleColumns.Contains(olvEmulationColumn), olvEmulationColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.SaveStates, visibleColumns.Contains(olvSaveStates), olvSaveStates.DisplayIndex);
            classMap.ToggleColumn(m => m.Players, visibleColumns.Contains(olvPlayersColumn), olvPlayersColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Coins, visibleColumns.Contains(olvCoinsColumn), olvCoinsColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenType, visibleColumns.Contains(olvScreenType), olvScreenType.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenOrientation, visibleColumns.Contains(olvScreenOrientation), olvScreenOrientation.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenRefreshRate, visibleColumns.Contains(olvScreenRefreshRate), olvScreenRefreshRate.DisplayIndex);
            classMap.ToggleColumn(m => m.Controls, visibleColumns.Contains(olvControlsColumn), olvControlsColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsClone, visibleColumns.Contains(olvIsCloneColumn), olvIsCloneColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsBIOS, visibleColumns.Contains(olvBIOSColumn), olvBIOSColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsMechanical, visibleColumns.Contains(olvMechanicalColumn), olvMechanicalColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.RequireCHDs, visibleColumns.Contains(olvRequireCHDsColumn), olvRequireCHDsColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.RequireSamples, visibleColumns.Contains(olvSamplesColumn), olvSamplesColumn.DisplayIndex);

            var filteredObjectList = datFileMachineVirtualListDataSource.FilteredObjectList;
            var fileWritten = DATFileCSVWriter.WriteToFile(exportFilePath, filteredObjectList, classMap);
            if (!fileWritten)
            {
                MessageBoxOperations.ShowError($"Unable to create file {exportFilePath}", "Unable to export file");
                return;
            }

            MessageBoxOperations.ShowInformation($"File successfully written to {exportFilePath}", "File exported");
        }

        private void olvDatFileListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (!showingWorkingColors)
            {
                return;
            }

            var datFileMachine = e.Model as DATFileMachine;
            if (datFileMachine == null)
            {
                return;
            }

            var status = datFileMachine.Status;
            if (string.IsNullOrEmpty(status))
            {
                return;
            }

            var olvListItem = e.Item;
            var statusString = status.ToLowerInvariant();
            switch (statusString)
            {
                case "imperfect":
                case "protection":
                {
                    olvListItem.BackColor = Color.LightYellow;
                }
                    break;
                case "good":
                {
                    olvListItem.BackColor = Color.LightGreen;
                }
                    break;
                case "preliminary":
                {
                    olvListItem.BackColor = Color.LightCoral;
                }
                    break;
            }
        }

        private void showGridLinesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var showingGridLines = showGridLinesToolStripMenuItem.Checked;
            olvDatFileListView.GridLines = showingGridLines;

            // Refresh the ListView to reflect the change
            olvDatFileListView.Refresh();

            // Save the setting change
            var settings = ProjectSettingsManager.DATFileViewerSettings;
            settings.ShowGridLines = showingGridLines;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }

        private void showColorsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            showingWorkingColors = showColorsToolStripMenuItem.Checked;

            // Refresh the ListView to reflect the colour change
            olvDatFileListView.Refresh();

            // Save the setting change
            var settings = ProjectSettingsManager.DATFileViewerSettings;
            settings.ShowColors = showingWorkingColors;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFindDialog();
        }

        //private void DATFileViewerForm_KeyDown(object sender, KeyEventArgs e)
        //{
        //    // form.KeyPreview must be set to true to catch these
        //    if (e.Control && e.KeyCode == Keys.F)
        //    {
        //        OpenFindDialog();
        //    }
        //}

        private void OpenFindDialog()
        {
            using (var findForm = new DATFileViewerFindForm())
            {
                // Open the form at the top left of our DAT File Viewer form
                findForm.StartPosition = FormStartPosition.Manual;
                findForm.Location = FormOperations.GetRelativeControlPoint(this, topMenuStrip);
                findForm.ShowDialog();

                var searchTerm = findForm.SearchTerm;
                var useRegularExpressions = findForm.UseRegularExpressions;

                var filter = useRegularExpressions ? TextMatchFilter.Regex(olvDatFileListView, searchTerm) : TextMatchFilter.Contains(olvDatFileListView, searchTerm);
                // filter.Columns = olvDatFileListView.AllColumns.ToArray();
                olvDatFileListView.AdditionalFilter = filter;
            }
        }

        private void DATFileViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the grid's state
            var olvDatFileListViewState = olvDatFileListView.SaveState();

            // Save the state so that we can restore it the next time this form is opened
            var savedSettings = ProjectSettingsManager.DATFileViewerSettings;
            savedSettings.SavedState = olvDatFileListViewState;
            savedSettings.Maximized = WindowState == FormWindowState.Maximized;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WantsToOpenNewFile = true;
            Close();
        }

        private void resetFilteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Still need to implement this
            // olvDatFileListView.ResetColumnFiltering();

            // olvDatFileListView.Sorting = SortOrder.None;
            //olvDatFileListView.Sort(olvDatFileListView.LastSortColumn, SortOrder.None);
            //olvDatFileListView.ListViewItemSorter = new DatFileMachineComparer_MAMEIndex();
        }
    }
}
