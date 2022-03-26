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

        //public static void UpdateProgramSettings_Main()
        //{
        //    var serialized = JsonConvert.SerializeObject(MainSettings);
        //    Properties.Settings.Default.ProgramSettings_Main = serialized;
        //}

        //public static void UpdateProgramSettings_DATFileViewer()
        //{
        //    var serialized = JsonConvert.SerializeObject(DATFileViewerSettings);
        //    Properties.Settings.Default.ProgramSettings_DATFileViewer = serialized;
        //}

        //public static float ResolveFloat(ProjectSettings key)
        //{
        //    switch (key)
        //    {
        //        case ProjectSettings.AllowedSimilarityValue: return Properties.Settings.Default.AllowedSimilarityValue;
        //    }

        //    return 0;
        //}

        //public static void SaveFloat(ProjectSettings key, float value)
        //{
        //    switch (key)
        //    {
        //        case ProjectSettings.AllowedSimilarityValue: Properties.Settings.Default.AllowedSimilarityValue = value;
        //            break;
        //    }

        //    Properties.Settings.Default.Save();
        //}

        //public static string ResolveString(ProjectSettings key)
        //{
        //    switch (key)
        //    {
        //        case ProjectSettings.ROMsDirectory: return Properties.Settings.Default.ROMsDirectory;
        //    }

        //    return null;
        //}

        //public static void SaveString(ProjectSettings key, string value)
        //{
        //    switch (key)
        //    {
        //        case ProjectSettings.ROMsDirectory:
        //            Properties.Settings.Default.ROMsDirectory = value;
        //            break;
        //    }

        //    Properties.Settings.Default.Save();
        //}

        //public static bool ResolveBool(ProjectSettings key)
        //{
        //    switch (key)
        //    {
        //        case ProjectSettings.MatchUsingGameListXMLName:
        //            return Properties.Settings.Default.MatchUsingGameListXMLName;
        //        case ProjectSettings.AutoExpandTreeViewsAfterOperations:
        //            return Properties.Settings.Default.AutoExpandTreeViewsAfterOperations;
        //        case ProjectSettings.DATFileViewer_ShowColors:
        //            return Properties.Settings.Default.DATFileViewer_ShowColors;
        //    }

        //    return false;
        //}

        //public static void SaveBool(ProjectSettings key, bool value)
        //{
        //    switch (key)
        //    {
        //        case ProjectSettings.MatchUsingGameListXMLName:
        //            Properties.Settings.Default.MatchUsingGameListXMLName = value;
        //            break;
        //        case ProjectSettings.AutoExpandTreeViewsAfterOperations:
        //            Properties.Settings.Default.AutoExpandTreeViewsAfterOperations = value;
        //            break;
        //        case ProjectSettings.DATFileViewer_ShowColors:
        //            Properties.Settings.Default.DATFileViewer_ShowColors = value;
        //            break;
        //    }

        //    Properties.Settings.Default.Save();
        //}

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

    public enum ProjectSettings
    {
        ROMsDirectory,
        MatchUsingGameListXMLName,
        AutoExpandTreeViewsAfterOperations,
        AllowedSimilarityValue,
        DATFileViewer_ShowColors
    }

    public enum ProgramSettingsType
    {
        Main,
        DATFileViewer
    }
}