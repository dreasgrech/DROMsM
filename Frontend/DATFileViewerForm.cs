﻿using System;
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

        public DATFileViewerForm()
        {
            InitializeComponent();

            showingWorkingColors = ProjectSettingsManager.ResolveBool(ProjectSettings.DATFileViewer_ShowColors);
            showColorsToolStripMenuItem.Checked = showingWorkingColors;
        }

        public void ProcessDATFile(string datFilePath)
        {
            var datFileHandler = new DATFileHandler();
            var datFile = datFileHandler.ParseDATFile(datFilePath);

            /*
            // var listViewItems = new List<ListViewItem>();
            var listViewItems = new List<OLVListItem>();
            using (var machinesEnumerator = datFile.GetMachinesEnumerator())
            {
                while (machinesEnumerator.MoveNext())
                {
                    var machine = machinesEnumerator.Current;
                    if (machine == null)
                    {
                        Logger.LogError($"Unable to add machine to ListView");
                        continue;
                    }

                    // var listViewItem = new ListViewItem(new[]
                    var listViewItem = new OLVListItem(new[]
                    {
                        machine.Name, 
                        machine.Description,
                        machine.Year,
                        machine.Manufacturer,
                        machine.Status,
                        machine.Emulation,
                        machine.SaveStates,
                        machine.Players,
                        machine.Coins,
                        machine.Controls,
                    });

                    // datFileListView.Items.Add(listViewItem);
                    // olvDatFileListView.Items.Add(listViewItem);
                    listViewItems.Add(listViewItem);
                }
            }
            */

            datFileMachineVirtualListDataSource = new DATFileMachineVirtualListDataSource(olvDatFileListView);
            datFileMachineVirtualListDataSource.AddObjects(datFile.Machines);
            // datFileMachineVirtualListDataSource.AddObjects(listViewItems);
            olvDatFileListView.VirtualListDataSource = datFileMachineVirtualListDataSource;

            // Expand the columns
            // datFileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            Text = $@"Viewing {datFilePath}";
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

            var filteredObjectList = datFileMachineVirtualListDataSource.FilteredObjectList;
            // var fileWritten = DATFileCSVWriter.WriteToFile(saveFilePath, currentDATFile);
            var fileWritten = DATFileCSVWriter.WriteToFile(saveFilePath, filteredObjectList);
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
            ProjectSettingsManager.SaveBool(ProjectSettings.DATFileViewer_ShowColors, showingWorkingColors);
        }
    }
}
