using System.Collections.Generic;
using System.Text.RegularExpressions;
using DIOUtils;

namespace Frontend
{
    public enum ROMFileType
    {
        Other,
        Zip,
        SevenZip,
        Cue,
        Bin,
        Adf
    }

    public class ROMEntry
    {
        public string AbsoluteFilePath { get; set; }
        public string AbsoluteDirectory { get; set; }
        public string RelativeFilePath { get; set; }
        public string RelativeSubDirectory { get; set; }
        public string Filename { get; set; }
        public string FilenameWithExtension { get; set; }
        public string FileExtension { get; private set; }
        public string ComparisonName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameWithPath { get; set; }
        public ROMFileType FileType { get; private set; }

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

        public ROMEntry(string absoluteFilePath)
        {
            AbsoluteFilePath = absoluteFilePath;
            FileExtension = FileUtilities.GetExtension(absoluteFilePath);
            FileType = GetROMFileType(FileExtension);

            StandardCodes = new HashSet<ROMStandardCodes>();
            UniversalCodes = new HashSet<ROMUniversalCode>();

            Revision = string.Empty;
            Track = string.Empty;
        }

        private static ROMFileType GetROMFileType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".zip": return ROMFileType.Zip;
                case ".7z": return ROMFileType.SevenZip;
                case ".cue": return ROMFileType.Cue;
                case ".bin": return ROMFileType.Bin;
            }

            return ROMFileType.Other;
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

        public static string RemoveROMSymbols(string filename)
        {
            // TODO: these can be combined
            filename = Regex.Replace(filename, @"\(.*?\)", string.Empty);
            filename = Regex.Replace(filename, @"\[.*?\]", string.Empty);

            /*
            // TODO: Commenting this to see if replacing bracket stuff with regex is enough
            for (int i = 0; i < RomSymbols.Length; i++)
            {
                filename = Replace(filename, RomSymbols[i], string.Empty, StringComparison.OrdinalIgnoreCase);
            }
            */

            filename = filename.Trim();
            return filename;
        }

        public static string ConvertFilenameForComparison(string filename)
        {
            // First remove all the ROM Symbols
            var filename_compare = ROMEntry.RemoveROMSymbols(filename);

            // Next replace any fluff characters that can affect comparison matches
            filename_compare = filename_compare.ToLowerInvariant()
                    // .Replace("&", "and")

                    // TODO: Fix this numbers-replacement bullshit because the order matters
                    .Replace(" vii", "7")
                    .Replace(" vi", "6")
                    .Replace(" iv", "4")
                    .Replace(" v", "5")
                    .Replace(" iii", "3")
                    .Replace(" ii", "2")
                    .Replace(" i", "1")

                    .Replace(" ", string.Empty)
                    .Replace("-", string.Empty)
                    .Replace("_", string.Empty)
                    .Replace(",", string.Empty)
                    .Replace("'", string.Empty)

                    .Replace("the", string.Empty)
                    .Replace("and", string.Empty)
                    .Replace("&", string.Empty)
                ;

            filename_compare = filename_compare
                // Covert it to lower again because of any replacement modifications we did in the previous set
                .ToLowerInvariant()
                .Trim();

            return filename_compare;
        }

        public override string ToString()
        {
            return Filename;
        }
    }

    public enum TreeViewROMDisplayNameType
    {
        RelativeFilePath,
        DisplayName,
        FilenameWithExtension
    }

}