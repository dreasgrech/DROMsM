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

            var mainSettingsJson = Properties.Settings.Default.ProgramSettings_Main;
            MainSettings = JsonConvert.DeserializeObject<ProgramSettings_Main>(mainSettingsJson) ?? new ProgramSettings_Main();

            var datSettingsJson = Properties.Settings.Default.ProgramSettings_DATFileViewer;
            DATFileViewerSettings = JsonConvert.DeserializeObject<ProgramSettings_DATFileViewer>(datSettingsJson) ?? new ProgramSettings_DATFileViewer();
        }

        public static void UpdateProgramSettings(ProgramSettingsType programSettingsType)
        {
            switch (programSettingsType)
            {
                case ProgramSettingsType.Main:
                {
                    Properties.Settings.Default.ProgramSettings_Main = JsonConvert.SerializeObject(MainSettings);
                }
                    break;
                case ProgramSettingsType.DATFileViewer:
                {
                    Properties.Settings.Default.ProgramSettings_DATFileViewer = JsonConvert.SerializeObject(DATFileViewerSettings);
                }
                    break;
            }

            Properties.Settings.Default.Save();
        }

        private static void UpgradeSettingsVersion()
        {
            if (Properties.Settings.Default.UpdateSettingsVersion)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettingsVersion = false;
                Properties.Settings.Default.Save();
            }
        }
    }

    public enum ProgramSettingsType
    {
        Main,
        DATFileViewer
    }
}