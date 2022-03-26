using System.Collections.Generic;

namespace Frontend
{
    public class ProgramSettings_DATFileViewer
    {
        public bool ShowColors { get; set; }
        public Dictionary<string, DatFileViewerColumnSettings> ColumnSettings { get; set; }

        public ProgramSettings_DATFileViewer()
        {
            // Set default values
            ShowColors = true;
            ColumnSettings = new Dictionary<string, DatFileViewerColumnSettings>(/* icomparer */);
        }
    }

    public class DatFileViewerColumnSettings
    {
        public int DisplayIndex { get; set; }
        public bool Visible { get; set; }
    }
}