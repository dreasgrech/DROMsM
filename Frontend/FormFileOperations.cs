using System.Windows.Forms;

namespace Frontend
{
    public static class FormFileOperations
    {
        public static string ShowSaveFileDialog()
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "Text File|*.txt",
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
            return ShowOpenFileDialog("Text File|*.txt");
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