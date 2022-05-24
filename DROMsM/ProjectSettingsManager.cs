using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using DROMsM.ProgramSettings;
using DROMsM.Properties;
using Frontend;
using Newtonsoft.Json;

namespace DROMsM
{
    public static class ProjectSettingsManager
    {
        public static ProgramSettings_Main MainSettings { get; }
        public static ProgramSettings_DATFileViewer DATFileViewerSettings { get; }
        public static ProgramSettings_LaunchBox LaunchBoxSettings { get; }

        static ProjectSettingsManager()
        {
            /*
            // UpgradeSettingsVersion();
            RestoreSettings();
            */

            var mainSettingsJson = Settings.Default.ProgramSettings_Main;
            MainSettings = JsonConvert.DeserializeObject<ProgramSettings_Main>(mainSettingsJson) ?? new ProgramSettings_Main();

            var datSettingsJson = Settings.Default.ProgramSettings_DATFileViewer;
            DATFileViewerSettings = JsonConvert.DeserializeObject<ProgramSettings_DATFileViewer>(datSettingsJson) ?? new ProgramSettings_DATFileViewer();

            var launchBoxSettingsJson = Settings.Default.ProgramSettings_LaunchBox;
            LaunchBoxSettings  = JsonConvert.DeserializeObject<ProgramSettings_LaunchBox>(launchBoxSettingsJson) ?? new ProgramSettings_LaunchBox();
        }

        public static void UpdateProgramSettings(ProgramSettingsType programSettingsType)
        {
            switch (programSettingsType)
            {
                case ProgramSettingsType.Main:
                {
                    Settings.Default.ProgramSettings_Main = JsonConvert.SerializeObject(MainSettings);
                }
                    break;
                case ProgramSettingsType.DATFileViewer:
                {
                    Settings.Default.ProgramSettings_DATFileViewer = JsonConvert.SerializeObject(DATFileViewerSettings);
                }
                    break;
                case ProgramSettingsType.LaunchBox:
                {
                    Settings.Default.ProgramSettings_LaunchBox = JsonConvert.SerializeObject(LaunchBoxSettings);
                }
                    break;
            }

            Settings.Default.Save();
        }

        /// <summary>
        /// TODO: This is currently not being used.  Why not??
        ///
        /// TODO: Do we even need this now that we're not using ClickOnce anymore???
        /// </summary>
        private static void UpgradeSettingsVersion()
        {
            if (Settings.Default.UpdateSettingsVersion)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpdateSettingsVersion = false;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Make a backup of our settings.
        /// Used to persist settings across updates.
        /// </summary>
        public static void BackupSettings()
        {
            string settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            string destination = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";
            File.Copy(settingsFile, destination, true);
        }

        /// <summary>
        /// Restore our settings backup if any.
        /// Used to persist settings across updates.
        /// </summary>
        private static void RestoreSettings()
        {
            //Restore settings after application update            
            string destFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            string sourceFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";
            // Check if we have settings that we need to restore
            if (!File.Exists(sourceFile))
            {
                // Nothing we need to do
                return;
            }
            // Create directory as needed
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            }
            catch (Exception ex)
            {
                MessageBoxOperations.ShowException(ex);
            }

            // Copy our backup file in place 
            try
            {
                File.Copy(sourceFile, destFile, true);
            }
            catch (Exception ex)
            {
                MessageBoxOperations.ShowException(ex);
            }

            // Delete backup file
            try
            {
                File.Delete(sourceFile);
            }
            catch (Exception ex)
            {
                MessageBoxOperations.ShowException(ex);
            }
        }
    }

    public enum ProgramSettingsType
    {
        Main,
        DATFileViewer,
        LaunchBox
    }
}