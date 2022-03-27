using Newtonsoft.Json;

namespace Frontend
{
    public static class ProjectSettingsManager
    {
        public static ProgramSettings_Main MainSettings { get; }
        public static ProgramSettings_DATFileViewer DATFileViewerSettings { get; }

        static ProjectSettingsManager()
        {
            UpgradeSettingsVersion();

            var mainSettingsJson = DROMsM.Properties.Settings.Default.ProgramSettings_Main;
            MainSettings = JsonConvert.DeserializeObject<ProgramSettings_Main>(mainSettingsJson) ?? new ProgramSettings_Main();

            var datSettingsJson = DROMsM.Properties.Settings.Default.ProgramSettings_DATFileViewer;
            DATFileViewerSettings = JsonConvert.DeserializeObject<ProgramSettings_DATFileViewer>(datSettingsJson) ?? new ProgramSettings_DATFileViewer();
        }

        public static void UpdateProgramSettings(ProgramSettingsType programSettingsType)
        {
            switch (programSettingsType)
            {
                case ProgramSettingsType.Main:
                {
                    DROMsM.Properties.Settings.Default.ProgramSettings_Main = JsonConvert.SerializeObject(MainSettings);
                }
                    break;
                case ProgramSettingsType.DATFileViewer:
                {
                    DROMsM.Properties.Settings.Default.ProgramSettings_DATFileViewer = JsonConvert.SerializeObject(DATFileViewerSettings);
                }
                    break;
            }

            DROMsM.Properties.Settings.Default.Save();
        }

        private static void UpgradeSettingsVersion()
        {
            if (DROMsM.Properties.Settings.Default.UpdateSettingsVersion)
            {
                DROMsM.Properties.Settings.Default.Upgrade();
                DROMsM.Properties.Settings.Default.UpdateSettingsVersion = false;
                DROMsM.Properties.Settings.Default.Save();
            }
        }
    }

    public enum ProgramSettingsType
    {
        Main,
        DATFileViewer
    }
}