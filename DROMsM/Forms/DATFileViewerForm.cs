﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using DIOUtils;
using DROMsM.ProgramSettings;
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

        private readonly Dictionary<DATFileMachineField, OLVColumn> fieldColumnMappings;

        private readonly ProgramSettings_DATFileViewer settings;

        HashSet<DATFileMachineField> usedFields;

        public DATFileViewerForm()
        {
            InitializeComponent();

            fieldColumnMappings = new Dictionary<DATFileMachineField, OLVColumn>(EqualityComparer<DATFileMachineField>.Default)
            {
                {DATFileMachineField.Name, olvSetColumn},
                {DATFileMachineField.Description, olvNameColumn},
                {DATFileMachineField.Year, olvYearColumn},
                {DATFileMachineField.Manufacturer, olvManufacturerColumn}, 
                {DATFileMachineField.Status, olvStatusColumn},
                {DATFileMachineField.Emulation, olvEmulationColumn},
                {DATFileMachineField.SaveStates, olvSaveStatesColumn}, 
                {DATFileMachineField.Players, olvPlayersColumn}, 
                {DATFileMachineField.Coins, olvCoinsColumn}, 
                {DATFileMachineField.Controls, olvControlsColumn}, 
                {DATFileMachineField.JoystickWay, olvJoystickWays}, 
                {DATFileMachineField.ScreenType, olvScreenTypeColumn}, 
                {DATFileMachineField.ScreenWidth, olvScreenWidth}, 
                {DATFileMachineField.ScreenHeight, olvScreenHeight}, 
                {DATFileMachineField.ScreenOrientation, olvScreenOrientationColumn}, 
                {DATFileMachineField.ScreenRefreshRate, olvScreenRefreshRateColumn},  
                {DATFileMachineField.IsBIOS, olvBIOSColumn}, 
                {DATFileMachineField.IsClone, olvIsCloneColumn}, 
                {DATFileMachineField.IsMechanical, olvMechanicalColumn}, 
                {DATFileMachineField.RequireCHDs, olvRequireCHDsColumn}, 
                {DATFileMachineField.RequireSamples, olvSamplesColumn}, 
                {DATFileMachineField.IsDevice, olvDeviceColumn}, 
            };

            settings = ProjectSettingsManager.DATFileViewerSettings;

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
            olvDeviceColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
            olvDeviceColumn.AspectToStringConverter = cellValue => (bool) cellValue ? "Yes" : "No";
            olvPlayersColumn.AspectToStringConverter = cellValue =>
            {
                if (cellValue == null)
                {
                    return null;
                }

                var cv = cellValue.ToString();
                return cv == "0" ? null : cv;
            };
            olvScreenWidth.AspectToStringConverter = cellValue =>
            {
                if (cellValue == null)
                {
                    return null;
                }

                var cv = cellValue.ToString();
                return cv == "0" ? null : cv;
            };
            olvScreenHeight.AspectToStringConverter = cellValue =>
            {
                if (cellValue == null)
                {
                    return null;
                }

                var cv = cellValue.ToString();
                return cv == "0" ? null : cv;
            };

            olvSetColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Name;
            olvNameColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Description;
            olvYearColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Year;
            olvManufacturerColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Manufacturer;
            olvStatusColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Status;
            olvEmulationColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Emulation;
            olvSaveStatesColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.SaveStates;
            olvPlayersColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Players;
            olvCoinsColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Coins;
            olvControlsColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.Controls;
            olvJoystickWays.AspectGetter = rowObject => (rowObject as DATFileMachine)?.JoystickWays;
            olvScreenTypeColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.ScreenType;
            olvScreenWidth.AspectGetter = rowObject => (rowObject as DATFileMachine)?.ScreenWidth;
            olvScreenHeight.AspectGetter = rowObject => (rowObject as DATFileMachine)?.ScreenHeight;
            olvScreenOrientationColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.ScreenOrientation;
            olvScreenRefreshRateColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.ScreenRefreshRate;
            olvBIOSColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.IsBIOS;
            olvIsCloneColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.IsClone;
            olvMechanicalColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.IsMechanical;
            olvRequireCHDsColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.RequireCHDs;
            olvSamplesColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.RequireSamples;
            olvDeviceColumn.AspectGetter = rowObject => (rowObject as DATFileMachine)?.IsDevice;
        }

        public bool ProcessDATFile(string datFilePath)
        {
            var datFileHandler = new U8XMLDATFileHandler();
            // var datFileHandler = new XmlReaderDATFileHandler();

            DATFile datFile;
            try
            {
                datFile = datFileHandler.ParseDATFile(datFilePath, out usedFields);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unable to process file: {datFilePath}" +
                                   $"{Environment.NewLine}{Environment.NewLine}" +
                                   $"Make sure that it's a valid DAT file." +
                                   $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}" +
                                   $"Error message for the developer:" +
                                   $"{Environment.NewLine}" +
                                   $"{ex.Message}";
                                   //$"{Environment.NewLine}" +
                                   //$"{ex.StackTrace}";

                MessageBoxOperations.ShowError(errorMessage, "Unable to process DAT file");
                return false;
            }

            // If no recognized fields were found in the file, then there's nothing to show
            if (usedFields.Count == 0)
            {
                MessageBoxOperations.ShowError($"No entries found in DAT file: {datFilePath}", "No entries found in file");
                return false;
            }

            // Only show the columns which were actually parsed from the file and hide the rest
            if (settings.OnlyShowUsedColumns)
            {
                HideUnusedColumns();
            }

            datFileMachineVirtualListDataSource = new DATFileMachineVirtualListDataSource(olvDatFileListView);
            datFileMachineVirtualListDataSource.AddObjects(datFile.Machines);
            olvDatFileListView.VirtualListDataSource = datFileMachineVirtualListDataSource;

            // Expand the columns
            // datFileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            Text = $@"DAT File Viewer - {datFilePath}";
            datFilePathLabel.Text = datFilePath;
            totalSetsLabel.Text = StringUtilities.AddCommasToNumber(datFile.TotalMachines);
            var build = datFile.Build;
            // mameInfoTable.Visible = !string.IsNullOrEmpty(build);
            buildLabel.Text = build;

            currentDATFile = datFile;

            return true;
        }

        private void HideUnusedColumns()
        {
            using (var fieldColumnMappingsEnumerator = fieldColumnMappings.GetEnumerator())
            {
                while (fieldColumnMappingsEnumerator.MoveNext())
                {
                    var current = fieldColumnMappingsEnumerator.Current;
                    var field = current.Key;
                    var column = current.Value;

                    column.IsVisible = usedFields.Contains(field);
                }
            }

            // Rebuild the columns since we might have hidden some of them
            olvDatFileListView.RebuildColumns();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentDATFile == null)
            {
                Logger.LogError("There's no currently stored DAT file!");
                return;
            }

            var filteredObjectList = olvDatFileListView.SelectedObjects;
            var totalFilteredObjects = filteredObjectList.Count;
            if (totalFilteredObjects == 0)
            {
                var errorMessage = "Please select at least one entry in the grid";
                Logger.LogError(errorMessage);
                MessageBoxOperations.ShowError(errorMessage, "No entries selected");
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

            // If this file has a header, write the header now
            var datFileHeader = currentDATFile.Header;
            if (datFileHeader != null)
            {
                var headerXMLValue = datFileHeader.XMLValue;
                fullXMLFileStringBuilder.Append("\t");
                fullXMLFileStringBuilder.AppendLine(headerXMLValue);
            }

            // Write all the filtered nodes
            // var filteredObjectList = datFileMachineVirtualListDataSource.FilteredObjectList;
            // Write all the selected nodes
            foreach (DATFileMachine datFileMachine in filteredObjectList)
            {
                var xmlValue = datFileMachine.XMLValue;
                fullXMLFileStringBuilder.Append("\t");
                fullXMLFileStringBuilder.AppendLine(xmlValue);
            }

            // Write the root node closing tag
            var rootNodeXMLClosingTag = currentDATFile.GetRootNodeXMLClosingTag();
            fullXMLFileStringBuilder.AppendLine(rootNodeXMLClosingTag);

            // Write the generated text to file
            var full = fullXMLFileStringBuilder.ToString();
            FileUtilities.WriteAllText(saveAsFilePath, full);

            // MessageBoxOperations.ShowInformation($"File successfully saved to {saveAsFilePath}", "DAT File saved");
            MessageBoxOperations.ShowInformation($"Export with {StringUtilities.AddCommasToNumber(totalFilteredObjects)} entries successfully saved to {saveAsFilePath}", "DAT File saved");
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure there's at least one entry currently selected in the grid
            var filteredObjectList = olvDatFileListView.SelectedObjects;
            var totalFilteredObjects = filteredObjectList.Count;
            if (totalFilteredObjects == 0)
            {
                var errorMessage = "Please select at least one entry in the grid";
                Logger.LogError(errorMessage);
                MessageBoxOperations.ShowError(errorMessage, "No entries selected");
                return;
            }

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
            classMap.ToggleColumn(m => m.Manufacturer, visibleColumns.Contains(olvManufacturerColumn), olvManufacturerColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Status, visibleColumns.Contains(olvStatusColumn), olvStatusColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Emulation, visibleColumns.Contains(olvEmulationColumn), olvEmulationColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.SaveStates, visibleColumns.Contains(olvSaveStatesColumn), olvSaveStatesColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Players, visibleColumns.Contains(olvPlayersColumn), olvPlayersColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.Coins, visibleColumns.Contains(olvCoinsColumn), olvCoinsColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenType, visibleColumns.Contains(olvScreenTypeColumn), olvScreenTypeColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenWidth, visibleColumns.Contains(olvScreenWidth), olvScreenWidth.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenHeight, visibleColumns.Contains(olvScreenHeight), olvScreenHeight.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenOrientation, visibleColumns.Contains(olvScreenOrientationColumn), olvScreenOrientationColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.ScreenRefreshRate, visibleColumns.Contains(olvScreenRefreshRateColumn), olvScreenRefreshRateColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsClone, visibleColumns.Contains(olvIsCloneColumn), olvIsCloneColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsBIOS, visibleColumns.Contains(olvBIOSColumn), olvBIOSColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsMechanical, visibleColumns.Contains(olvMechanicalColumn), olvMechanicalColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.RequireCHDs, visibleColumns.Contains(olvRequireCHDsColumn), olvRequireCHDsColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.RequireSamples, visibleColumns.Contains(olvSamplesColumn), olvSamplesColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.IsDevice, visibleColumns.Contains(olvDeviceColumn), olvDeviceColumn.DisplayIndex);
            classMap.ToggleColumn(m => m.JoystickWays, visibleColumns.Contains(olvJoystickWays), olvJoystickWays.DisplayIndex);
            classMap.ToggleColumn(m => m.Controls, visibleColumns.Contains(olvControlsColumn), olvControlsColumn.DisplayIndex);

            // var filteredObjectList = datFileMachineVirtualListDataSource.FilteredObjectList;
            var fileWritten = DATFileCSVWriter.WriteToFile(exportFilePath, filteredObjectList, classMap);
            if (!fileWritten)
            {
                MessageBoxOperations.ShowError($"Unable to create file {exportFilePath}", "Unable to export file");
                return;
            }

            // MessageBoxOperations.ShowInformation($"File successfully written to {exportFilePath}", "File exported");
            MessageBoxOperations.ShowInformation($"File with {StringUtilities.AddCommasToNumber(totalFilteredObjects)} entries successfully saved to {exportFilePath}", "File exported");
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
            using (var findForm = new DATFileViewerFindForm("Search in DAT file", ProjectSettingsManager.DATFileViewerSettings.FindDialogSettings))
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
            olvDatFileListView.ResetColumnFiltering();

            // MessageBoxOperations.ShowInformation("Not implemented yet", "Work in Progress");
            // Reset sorting
            // olvDatFileListView.Sorting = SortOrder.None;
            //olvDatFileListView.Sort(olvDatFileListView.LastSortColumn, SortOrder.None);
            //olvDatFileListView.ListViewItemSorter = new DatFileMachineComparer_MAMEIndex();
        }

        private void showAllColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var columns = olvDatFileListView.AllColumns;
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                column.IsVisible = true;
            }

            olvDatFileListView.RebuildColumns();
        }

        private void hideUnusedColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideUnusedColumns();
        }

        private void createMAMEIniFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedObjects = olvDatFileListView.SelectedObjects;
            
            // Make sure there are rom entries in the grid selected
            if (selectedObjects.Count == 0)
            {
                MessageBoxOperations.ShowError("Please select the rom entries for which to create the ini files for", "No rom files are selected");
                return;
            }

            // Show the Create MAME ini files form
            using (var createMAMEIniFilesForm = new CreateMAMEIniFilesForm(selectedObjects))
            {
                createMAMEIniFilesForm.ShowDialog();
            }
        }

        private void olvDatFileListView_SelectionChanged(object sender, EventArgs e)
        {
            // Update the total number of selected rom sets in the grid
            var olvDatFileListViewSelectedObjects = olvDatFileListView.SelectedObjects;
            selectedSetsLabel.Text = StringUtilities.AddCommasToNumber(olvDatFileListViewSelectedObjects.Count);
        }

        private void olvDatFileListView_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            // Update the total number of visible rom sets in the grid
            var olvDatFileListViewVisibleObjects = olvDatFileListView.Items;
            visibleSetsLabel.Text = StringUtilities.AddCommasToNumber(olvDatFileListViewVisibleObjects.Count);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            olvDatFileListView.SelectAll();
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            olvDatFileListView.DeselectAll();
        }
    }
}
