namespace DROMsM.ProgramSettings
{
    public class ProgramSettings_Main
    {
        public string ROMsDirectory { get; set; }
        public bool MatchUsingGameListXMLName { get; set; }
        public bool AutoExpandTreeViewsAfterOperations { get; set; }
        public bool UpdateSettingsVersion { get; set; }
        public float AllowedSimilarityValue { get; set; }

        public ProgramSettings_Main()
        {
            // Set default values
            AllowedSimilarityValue = 0.9f;
        }
    }
}