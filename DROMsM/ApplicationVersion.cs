using System;
using System.Reflection;

namespace DROMsM
{
    public static class ApplicationVersion
    {
        public static Version FullVersion { get; private set; }
        public static string FullVersionText { get; private set; }

        static ApplicationVersion()
        {
            FullVersion = Assembly.GetExecutingAssembly().GetName().Version;
            FullVersionText = FullVersion.ToString();
        }

    }
}