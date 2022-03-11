namespace Frontend
{
    public static class ProjectSettingsManager
    {
        public static string ResolveString(ProjectSettings key)
        {
            switch (key)
            {
                case ProjectSettings.ROMsDirectory: return Properties.Settings.Default.ROMsDirectory;
            }

            return null;
        }

        public static void SaveString(ProjectSettings key, string value)
        {
            switch (key)
            {
                case ProjectSettings.ROMsDirectory:
                    Properties.Settings.Default.ROMsDirectory = value;
                    break;
            }

            Properties.Settings.Default.Save();
        }

        public static bool ResolveBool(ProjectSettings key)
        {
            switch (key)
            {
                case ProjectSettings.MatchUsingGameListXMLName:
                    return Properties.Settings.Default.MatchUsingGameListXMLName;
                case ProjectSettings.AutoExpandTreeViewsAfterOperations:
                    return Properties.Settings.Default.AutoExpandTreeViewsAfterOperations;
            }

            return false;
        }

        public static void SaveBool(ProjectSettings key, bool value)
        {
            switch (key)
            {
                case ProjectSettings.MatchUsingGameListXMLName:
                    Properties.Settings.Default.MatchUsingGameListXMLName = value;
                    break;
                case ProjectSettings.AutoExpandTreeViewsAfterOperations:
                    Properties.Settings.Default.AutoExpandTreeViewsAfterOperations = value;
                    break;
            }

            Properties.Settings.Default.Save();
        }
    }

    public enum ProjectSettings
    {
        ROMsDirectory,
        MatchUsingGameListXMLName,
        AutoExpandTreeViewsAfterOperations
    }
}