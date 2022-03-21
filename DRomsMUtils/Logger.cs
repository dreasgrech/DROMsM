using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Frontend
{
    public class LoggerEntry
    {
        public string Entry { get; set; }

        public LoggerEntry(string entry)
        {
            Entry = entry;
        }
    }

    public static class Logger
    {
        // TODO: This HAS to be a thread-safe list
        // private static List<LoggerEntry> entries;

        private static ListView listView;

        static Logger()
        {
            // entries = new List<LoggerEntry>();
        }

        public static void LogError(string action)
        {
            // TODO: this.

            var error = $"Error: {action}";
            Log(error);
        }

        public static void Log(string action)
        {
            var entry = new LoggerEntry(action);
            // entries.Add(entry);

            // if (listView.InvokeRequired)
            // {
                // listView.BeginInvoke(new Action(() => { AddLogToTreeView(action); }));
                listView.BeginInvoke(new Action(() => { AddLogToTreeView(entry); }));
            // }
            //else
            //{
            //    AddLogToTreeView(action);
            //}
        }

        // static void AddLogToTreeView(string action)
        static void AddLogToTreeView(LoggerEntry entry)
        {
            var lvItem = new ListViewItem();
            // lvItem.Text = action;
            lvItem.Text = entry.Entry;

            listView.Items.Add(lvItem);

            // Scroll to the bottom of the ListView
            listView.Items[listView.Items.Count - 1].EnsureVisible();
        }

        public static void SetListView(ListView lv)
        {
            listView = lv;
        }

        public static void LogException(Exception ex, bool showMessageBox = false)
        {
            var errorMessage = $"{ex.Message}\r\n\r\n{ex.StackTrace}";
            var innerException = ex.InnerException;
            if (innerException != null)
            {
                errorMessage += $"\r\n\r\n{innerException.Message}\r\n\r\n{innerException.StackTrace}";
            }

            Log(errorMessage);

            if (showMessageBox)
            {
                MessageBoxOperations.ShowError(errorMessage, "Error");
            }
        }
    }
}