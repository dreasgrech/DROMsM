using System.Collections.Generic;

namespace Frontend
{
    public class ProgramSettings_DATFileViewer
    {
        public bool ShowColors { get; set; }
        public byte[] SavedState { get; set; }

        public ProgramSettings_DATFileViewer()
        {
            // Set default values
            ShowColors = true;
        }
    }
}