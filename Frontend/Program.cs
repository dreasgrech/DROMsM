// #define CATCH_UNHANDLED_EXCEPTIONS

using System;
using System.Windows.Forms;
using DROMsM.Forms;

namespace DROMsM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if CATCH_UNHANDLED_EXCEPTIONS
            try
            {
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
#if CATCH_UNHANDLED_EXCEPTIONS
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, true);
                throw;
            }
#endif
        }
    }
}
