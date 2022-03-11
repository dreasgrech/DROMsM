using System.Collections.Generic;

namespace Frontend
{
    public class ROMEntry
    {
        public string FilePath { get; set; }
        public string AbsoluteDirectory { get; set; }
        public string RelativeFilePath { get; set; }
        public string Filename { get; set; }
        public string FilenameWithExtension { get; set; }
        public string ComparisonName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameWithPath { get; set; }

        public ROMCountry Country { get; set; }
        public ROMLicense License { get; set; }
        public HashSet<ROMStandardCodes> StandardCodes { get; set; }
        public HashSet<ROMUniversalCode> UniversalCodes { get; set; }
        public string Revision { get; set; }
        public string Track { get; set; }

        public RBGameListGame GameListFileEntry { get; set; }

        //public bool VerifiedGoodDump { get; set; }
        //public bool AlternateVersion { get; set; }
        //public bool BadDump { get; set; }
        //public bool CorrectedDump { get; set; }
        //public bool HackedROM { get; set; }
        //public bool OverDump { get; set; }
        //public bool PiratedVersion { get; set; }
        //public bool VersionWithTrainer { get; set; }
        //public bool DelayedDump { get; set; }

        public bool IsDummyROM { get; set; }

        public float ROMUsageScore { get; set; }

        public ROMEntry()
        {
            StandardCodes = new HashSet<ROMStandardCodes>();
            UniversalCodes = new HashSet<ROMUniversalCode>();

            Revision = string.Empty;
            Track = string.Empty;
        }

        public void AddCode(ROMStandardCodes code)
        {
            StandardCodes.Add(code);
        }

        public void AddCode(ROMUniversalCode code)
        {
            UniversalCodes.Add(code);
        }

        public bool HasCode(ROMStandardCodes code)
        {
            return StandardCodes.Contains(code);
        }

        public bool HasCode(ROMUniversalCode code)
        {
            return UniversalCodes.Contains(code);
        }

        public bool HasStandardCodes()
        {
            return StandardCodes.Count > 0;
        }

        public bool HasUniversalCodes()
        {
            return UniversalCodes.Count > 0;
        }

        public string GetDisplayName(TreeViewROMDisplayNameType displayNameType)
        {
            switch (displayNameType)
            {
                case TreeViewROMDisplayNameType.DisplayName: return DisplayName;
                case TreeViewROMDisplayNameType.RelativeFilePath: return RelativeFilePath;
                case TreeViewROMDisplayNameType.FilenameWithExtension: return FilenameWithExtension;
            }

            return null;
        }

        public override string ToString()
        {
            return Filename;
        }
    }
}