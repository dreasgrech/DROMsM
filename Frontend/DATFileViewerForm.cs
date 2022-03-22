using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DRomsMUtils;

namespace Frontend
{
    public partial class DATFileViewerForm : Form
    {
        private DATFile currentDATFile;

        public DATFileViewerForm()
        {
            InitializeComponent();
        }

        public void ProcessDATFile(string datFilePath)
        {
            var datFileHandler = new DATFileHandler();
            var datFile = datFileHandler.ParseDATFile(datFilePath);

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

                    var listViewItem = new ListViewItem(new[]
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

                    datFileListView.Items.Add(listViewItem);
                }
            }

            // Expand the columns
            datFileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

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

            var fileWritten = DATFileCSVWriter.WriteToFile(saveFilePath, currentDATFile);
            if (!fileWritten)
            {
                MessageBoxOperations.ShowError($"Unable to create file {saveFilePath}", "Unable to write file");
                return;
            }

            MessageBoxOperations.ShowInformation($"File successfully written at {saveFilePath}", "File exported");
        }
    }
}
