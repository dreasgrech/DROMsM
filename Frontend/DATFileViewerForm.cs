using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using DRomsMUtils;

namespace Frontend
{
    public partial class DATFileViewerForm : Form
    {
        private DATFileMachineVirtualListDataSource datFileMachineVirtualListDataSource;

        private bool showingWorkingColors;

        public DATFileViewerForm()
        {
            InitializeComponent();

            var settings = ProjectSettingsManager.DATFileViewerSettings;

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
        }

        public void ProcessDATFile(string datFilePath)
        {
            var datFileHandler = new DATFileHandler();
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
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFilePath = FormFileOperations.ShowSaveFileDialog(FormFileOperations.SaveDialog_CSVFilesFilter);
            if (string.IsNullOrEmpty(saveFilePath))
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

            var filteredObjectList = datFileMachineVirtualListDataSource.FilteredObjectList;
            var fileWritten = DATFileCSVWriter.WriteToFile(saveFilePath, filteredObjectList, classMap);
            if (!fileWritten)
            {
                MessageBoxOperations.ShowError($"Unable to create file {saveFilePath}", "Unable to write file");
                return;
            }

            MessageBoxOperations.ShowInformation($"File successfully written at {saveFilePath}", "File exported");
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
                {
                    olvListItem.BackColor = Color.LightYellow;
                } break;
                case "good":
                {
                    olvListItem.BackColor = Color.LightGreen;
                } break;
                case "preliminary":
                {
                    olvListItem.BackColor = Color.LightCoral;
                } break;
            }
        }

        private void DATFileViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the grid's state
            var olvDatFileListViewState = olvDatFileListView.SaveState();

            // Save the state so that we can restore it the next time this form is opened
            var savedSettings = ProjectSettingsManager.DATFileViewerSettings;
            savedSettings.SavedState = olvDatFileListViewState;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}
