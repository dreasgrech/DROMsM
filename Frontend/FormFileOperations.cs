using System.Windows.Forms;

namespace Frontend
{
    public static class FormFileOperations
    {
        public static string TextFilesFilter = "Text File|*.txt";
        public static string OpenDialog_DATFilesFilter = "DAT Files (*.dat; *.xml)| *.dat; *.xml; |All Files (*.*;)| *.*; ";
        public static string SaveDialog_DATFilesFilter = "DAT Files (*.dat)| *.dat; |XML Files (*.xml)| *.xml; |All Files (*.*;)| *.*; ";
        public static string SaveDialog_CSVFilesFilter = "CSV Files (*.csv)| *.csv; |Microsoft Excel Worksheet (*.xlsx)| *.xlsx; |All Files (*.*;)| *.*; ";

        public static string ShowSaveFileDialog_Text()
        {
            return ShowSaveFileDialog(TextFilesFilter);
        }

        public static string ShowSaveFileDialog(string filter)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = filter,
            };

            var dialogResult = sfd.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return string.Empty;
            }

            return sfd.FileName;
        }

        public static string ShowOpenFileDialog_TextFiles()
        {
            return ShowOpenFileDialog(TextFilesFilter);
        }

        public static string ShowOpenFileDialog(string filter)
        {
            var ofd = new OpenFileDialog
            {
                Filter = filter,
            };

            var dialogResult = ofd.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return string.Empty;
            }

            return ofd.FileName;
        }
    }
}