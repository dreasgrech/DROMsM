using System.Windows.Forms;

namespace Frontend
{
    public static class MessageBoxOperations
    {
        public static bool ShowConfirmation(string text, string caption)
        {
            var confirmResult = MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return confirmResult == DialogResult.Yes;
        }

        public static void ShowError(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public static void ShowInformation(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}