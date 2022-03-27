using System.Collections.Generic;

namespace Frontend
{
    public class ProgramSettings_DATFileViewer
    {
        public bool ShowColors { get; set; }
        public bool ShowGridLines { get; set; }
        public byte[] SavedState { get; set; }
        public ProgramSettings_DatFileViewer_FindDialog FindDialogSettings { get; set; }

        public ProgramSettings_DATFileViewer()
        {
            // Set default values
            ShowColors = true;
            ShowGridLines = true;
            FindDialogSettings = new ProgramSettings_DatFileViewer_FindDialog();
        }
    }

    public class ProgramSettings_DatFileViewer_FindDialog
    {
        public string SearchTerm;
        public bool UseRegularExpressions;
    }
}