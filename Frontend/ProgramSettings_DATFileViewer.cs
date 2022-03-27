using System.Collections.Generic;

namespace Frontend
{
    public class ProgramSettings_DATFileViewer
    {
        public bool ShowColors { get; set; }
        public Dictionary<string, DatFileViewerColumnSettings> ColumnsSettings { get; set; }
        public byte[] SavedState { get; set; }

        public ProgramSettings_DATFileViewer()
        {
            // Set default values
            ShowColors = true;
            ColumnsSettings = new Dictionary<string, DatFileViewerColumnSettings>(/* icomparer */);
        }
    }

    public class DatFileViewerColumnSettings
    {
        public string AspectName { get; set; }
        public int DisplayIndex { get; set; }
        public bool Visible { get; set; }

        public DatFileViewerColumnSettings()
        {
            Visible = true;
        }

        public override string ToString()
        {
            return $"{DisplayIndex}. {AspectName}";
        }
    }
}