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

        // private readonly Dictionary<string, OLVColumn> columnsDictionary;
        private readonly Dictionary<int, OLVColumn> orderedColumns;

        public DATFileViewerForm()
        {
            InitializeComponent();

            var settings = ProjectSettingsManager.DATFileViewerSettings;
            showingWorkingColors = settings.ShowColors;

            showColorsToolStripMenuItem.Checked = showingWorkingColors;

            var columns = olvDatFileListView.Columns;

            var savedColumnSettings = settings.ColumnsSettings;

            var savedState = settings.SavedState;
            if (savedState != null)
            {
                var restored = olvDatFileListView.RestoreState(savedState);
                var x = restored;
            }

            /*

            // Before changing the columns' display indexes, save a reference to their original display index values
            // because once you change a DisplayIndex for one column, the others will be affected too.
            var originalColumnsHeaderDisplayIndexes = new Dictionary<string, int> (EqualityComparer<string>.Default);
            // var originalColumnsHeaderDisplayIndexes = new Orderedd
            var sortedColumns = new SortedSet<OLVColumnWrapper>(new OLVColumnWrapperDisplayIndexComparer());
            var sortedSettings = new SortedSet<DatFileViewerColumnSettings>(new DatFileViewerColumnSettingsDisplayIndexComparer());
            foreach (var savedColumnSetting in savedColumnSettings)
            {
                sortedSettings.Add(savedColumnSetting.Value);
            }

            foreach (OLVColumn columnHeader in columns)
            {
                originalColumnsHeaderDisplayIndexes[columnHeader.AspectName] = columnHeader.DisplayIndex;

                sortedColumns.Add(new OLVColumnWrapper(columnHeader));
            }

            foreach (OLVColumn columnHeader in columns)
            {
                var aspectName = columnHeader.AspectName;
                if (!savedColumnSettings.ContainsKey(aspectName))
                {
                    var columnSavedSettings = new DatFileViewerColumnSettings
                    {
                        AspectName = aspectName,
                        DisplayIndex = columnHeader.DisplayIndex,
                        Visible = columnHeader.IsVisible
                    };

                    savedColumnSettings[aspectName] = columnSavedSettings;
                }
            }

            // todo: when a clumn is moved, all the columns after the new display index need to be moved too.

            orderedColumns = new Dictionary<int, OLVColumn>(EqualityComparer<int>.Default);
            // columnsDictionary = new Dictionary<string, OLVColumn>(EqualityComparer<string>.Default);

            foreach (OLVColumn columnHeader in columns)
            {
                var aspectName = columnHeader.AspectName;
                // columnsDictionary[aspectName] = columnHeader;

                //// TODO: CONTINUE WORKING HERE
                //// TODO: WHAT'S HAPPENING IS THAT WHEN YOU ARE CHANGING A DISPLAYINDEX, THE DISPLAYINDEX OF ALL THE OTHER COLUMNS IS AUTOMATICALLY CHANGING
                //// Apply the column settings from our saved settings
                //if (savedColumnSettings.TryGetValue(aspectName, out var columnSavedSettings))
                //{
                //    columnHeader.DisplayIndex = columnSavedSettings.DisplayIndex;
                //    columnHeader.IsVisible = columnSavedSettings.Visible;
                //}
                //else
                //{
                //    columnHeader.DisplayIndex = originalColumnsHeaderDisplayIndexes[aspectName];
                //}

                var columnSavedSettings = savedColumnSettings[aspectName];
                columnHeader.DisplayIndex = columnSavedSettings.DisplayIndex;
                columnHeader.IsVisible = columnSavedSettings.Visible;

                orderedColumns[columnHeader.DisplayIndex] = columnHeader;
            }

            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);

            // Rebuild the columns since we could have hidden some of them
            // olvDatFileListView.RebuildColumns();

            */
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
            settings.ShowColors = showingWorkingColors;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
        }

        private void olvDatFileListView_Filter(object sender, FilterEventArgs e)
        {
            // UpdateColumnSettings();
        }

        void UpdateColumnSettings()
        {
            var savedSettings = ProjectSettingsManager.DATFileViewerSettings;
            var savedColumnsSettings = savedSettings.ColumnsSettings;

            var allColumns = olvDatFileListView.AllColumns;
            foreach (var column in allColumns)
            {
                var columnAspectName = column.AspectName;
                //if (!savedColumnsSettings.TryGetValue(columnAspectName, out var columnSavedSettings))
                //{
                //    columnSavedSettings = new DatFileViewerColumnSettings();
                //}

                var columnSavedSettings = savedColumnsSettings[columnAspectName];

                columnSavedSettings.Visible = column.IsVisible;
                columnSavedSettings.DisplayIndex = column.DisplayIndex;
                /*****************************/
                savedColumnsSettings[columnAspectName] = columnSavedSettings;
            }

            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);

        }

        private void olvDatFileListView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {
            // TODO: So the problem here is that this event runs BEFORE the columns after reordered,
            // TODO: meaning their current DisplayIndex is not updated yet to reflect the reordering.
            // UpdateColumnSettings();

            /*
            // TODO: The logic of this method is not correct!
            // TODO: Here I need to go through all columns and update our

            var movedColumnHeaderNewDisplayIndex = e.NewDisplayIndex;
            var previousColumnHeaderDisplayIndex = e.OldDisplayIndex;

            var savedSettings = ProjectSettingsManager.DATFileViewerSettings;
            var savedColumnsSettings = savedSettings.ColumnsSettings;

            // // This code is only used to clear while debugging
            //settings.ColumnsSettings = new Dictionary<string, DatFileViewerColumnSettings>();
            //ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);

            // Fetch the saved settings of the moved column
            var movedColumnHeader = (OLVColumn) e.Header;
            var movedColumnAspectName = movedColumnHeader.AspectName;
            if (!savedColumnsSettings.TryGetValue(movedColumnAspectName, out var movedColumnSavedSettings))
            {
                movedColumnSavedSettings = new DatFileViewerColumnSettings();
            }

            // Fetch the saved settings of the column that has just had its position replaced
            var previousColumnHeader = orderedColumns[movedColumnHeaderNewDisplayIndex];
            var previousColumnAspectName = previousColumnHeader.AspectName;
            if (!savedColumnsSettings.TryGetValue(previousColumnAspectName, out var previousColumnSavedSettings))
            {
                previousColumnSavedSettings = new DatFileViewerColumnSettings();
            }

            // Store a reference to the newly updated display indexes to our saved settings
            movedColumnSavedSettings.DisplayIndex = movedColumnHeaderNewDisplayIndex;
            previousColumnSavedSettings.DisplayIndex = previousColumnHeaderDisplayIndex;

            // Re-set the saved settings of both columns to the saved settings collection (just in case this is a new saved settings for this column)
            savedColumnsSettings[movedColumnAspectName] = movedColumnSavedSettings;
            savedColumnsSettings[previousColumnAspectName] = previousColumnSavedSettings;

            // Save a reference to the updated column order in our ordered columns collection
            orderedColumns[movedColumnHeaderNewDisplayIndex] = movedColumnHeader;
            orderedColumns[previousColumnHeaderDisplayIndex] = previousColumnHeader;

            // Finally update the Project Settings with our updated data
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);
            */
        }

        private void DATFileViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            UpdateColumnSettings();
            */

            var olvDatFileListViewState = olvDatFileListView.SaveState();

            var savedSettings = ProjectSettingsManager.DATFileViewerSettings;
            savedSettings.SavedState = olvDatFileListViewState;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.DATFileViewer);

        }
    }

    public class OLVColumnDisplayIndexComparer : IComparer<OLVColumn>
    {
        public int Compare(OLVColumn x, OLVColumn y)
        {
            return x.DisplayIndex.CompareTo(y.DisplayIndex);
        }
    }

    public class OLVColumnWrapperDisplayIndexComparer : IComparer<OLVColumnWrapper>
    {
        public int Compare(OLVColumnWrapper x, OLVColumnWrapper y)
        {
            return x.olvColumn.DisplayIndex.CompareTo(y.olvColumn.DisplayIndex);
        }
    }

    public class DatFileViewerColumnSettingsDisplayIndexComparer : IComparer<DatFileViewerColumnSettings>
    {
        public int Compare(DatFileViewerColumnSettings x, DatFileViewerColumnSettings y)
        {
            return x.DisplayIndex.CompareTo(y.DisplayIndex);
        }
    }

    public class OLVColumnWrapper
    {
        public readonly OLVColumn olvColumn;

        public OLVColumnWrapper(OLVColumn column)
        {
            olvColumn = column;
        }

        public override string ToString()
        {
            return $"{olvColumn.DisplayIndex}. {olvColumn.AspectName}";
        }
    }
}
