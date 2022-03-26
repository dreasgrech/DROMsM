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
        private DATFile currentDATFile;
        private DATFileMachineVirtualListDataSource datFileMachineVirtualListDataSource;

        private bool showingWorkingColors;

        private readonly Dictionary<string, OLVColumn> columnsDictionary;

        public DATFileViewerForm()
        {
            InitializeComponent();

            var settings = ProjectSettingsManager.DATFileViewerSettings;
            showingWorkingColors = settings.DATFileViewer_ShowColors;

            showColorsToolStripMenuItem.Checked = showingWorkingColors;

            columnsDictionary = new Dictionary<string, OLVColumn>(/* todo: icomparer */);
            var columns = olvDatFileListView.Columns;
            foreach (OLVColumn columnHeader in columns)
            {
                columnsDictionary[columnHeader.AspectName] = columnHeader;
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

            currentDATFile = datFile;
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

        private void showColorsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            showingWorkingColors = showColorsToolStripMenuItem.Checked;

            // Refresh the ListView to reflect the colour change
            olvDatFileListView.Refresh();

            // Save the setting change
            var settings = ProjectSettingsManager.DATFileViewerSettings;
            settings.DATFileViewer_ShowColors = showingWorkingColors;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }
    }

}
