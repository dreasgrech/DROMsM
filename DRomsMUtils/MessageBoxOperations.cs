using System;
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

        public static void ShowException(Exception exception, string caption = null)
        {
            var text = $"{exception.Message}\r\n{exception.StackTrace}";
            var innerException = exception.InnerException;
            if (innerException != null)
            {
                text += $"\r\n\r\n{innerException.Message}\r\n{innerException.StackTrace}";
            }

            ShowError(text, caption ?? "An exception has been encountered while running the program");
        }
    }
}