﻿namespace DROMsM.ProgramSettings
{
    public class ProgramSettings_DATFileViewer
    {
        public bool ShowColors { get; set; }
        public bool ShowGridLines { get; set; }
        public byte[] SavedState { get; set; }
        public bool Maximized { get; set; }
        public bool OnlyShowUsedColumns { get; set; }
        public FindDialogSettings FindDialogSettings { get; set; }
        public ProgramSettings_DatFileViewer_CreateMAMEIniFiles CreateMAMEIniFilesSettings { get; set; }

        public ProgramSettings_DATFileViewer()
        {
            // Set default values
            ShowColors = true;
            ShowGridLines = true;
            Maximized = false;
            FindDialogSettings = new FindDialogSettings();
            CreateMAMEIniFilesSettings = new ProgramSettings_DatFileViewer_CreateMAMEIniFiles();
            OnlyShowUsedColumns = true;
        }
    }

    public class FindDialogSettings
    {
        public string SearchTerm;
        public bool UseRegularExpressions;
    }

    public class ProgramSettings_DatFileViewer_CreateMAMEIniFiles
    {
        public string LastSelectedIniCreationPath { get; set; }
        public bool OverwriteExistingIniFiles { get; set; }
        public bool OnlyUpdateDifferentValues{ get; set; }


        public ProgramSettings_DatFileViewer_CreateMAMEIniFiles()
        {
            LastSelectedIniCreationPath = string.Empty;
            OverwriteExistingIniFiles = false;
            OnlyUpdateDifferentValues= false;
        }
    }
}