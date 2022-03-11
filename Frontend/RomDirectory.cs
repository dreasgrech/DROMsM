using System.Collections.Generic;

namespace Frontend
{
    public class RomDirectory
    {
        public string FullPath { get; set; }
        public List<RomDirectory> SubDirectories { get; set; }

        public RomDirectory()
        {
            SubDirectories = new List<RomDirectory>();
        }

        public string GetDisplayName(TreeViewROMDirectoryDisplayNameType displayNameType)
        {
            switch (displayNameType)
            {
                case TreeViewROMDirectoryDisplayNameType.FullPath: return FullPath;
            }

            return null;
        }


        public override string ToString()
        {
            return FullPath;
        }
    }
}