using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend
{
    public partial class DATFileViewerForm : Form
    {
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
                        machine.Year.ToString(),
                        machine.Manufacturer,
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
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFilePath = FormFileOperations.ShowSaveFileDialog(FormFileOperations.SaveDialog_DATFilesFilter);
            if (string.IsNullOrEmpty(saveFilePath))
            {
                return;
            }

        }
    }
}
