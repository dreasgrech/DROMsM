using DROMsM.ProgramSettings;
using DROMsM.Properties;
using Newtonsoft.Json;

namespace DROMsM
{
    public static class ProjectSettingsManager
    {
        public static ProgramSettings_Main MainSettings { get; }
        public static ProgramSettings_DATFileViewer DATFileViewerSettings { get; }

        static ProjectSettingsManager()
        {
            UpgradeSettingsVersion();

            var mainSettingsJson = Settings.Default.ProgramSettings_Main;
            MainSettings = JsonConvert.DeserializeObject<ProgramSettings_Main>(mainSettingsJson) ?? new ProgramSettings_Main();

            var datSettingsJson = Settings.Default.ProgramSettings_DATFileViewer;
            DATFileViewerSettings = JsonConvert.DeserializeObject<ProgramSettings_DATFileViewer>(datSettingsJson) ?? new ProgramSettings_DATFileViewer();
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
            }

            Settings.Default.Save();
        }

        private static void UpgradeSettingsVersion()
        {
            if (Settings.Default.UpdateSettingsVersion)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpdateSettingsVersion = false;
                Settings.Default.Save();
            }
        }
    }

    public enum ProgramSettingsType
    {
        Main,
        DATFileViewer
    }
}