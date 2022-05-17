using System;
using System.Reflection;
using System.Windows.Forms;

namespace DROMsM
{
    public static class ApplicationVersion
    {
        public static string ProductVersion { get; private set; }
        public static Version FullVersion { get; private set; }
        public static string FullVersionText { get; private set; }

        static ApplicationVersion()
        {
            ProductVersion = Application.ProductVersion;

            FullVersion = Assembly.GetExecutingAssembly().GetName().Version;
            FullVersionText = FullVersion.ToString();
        }

    }
}