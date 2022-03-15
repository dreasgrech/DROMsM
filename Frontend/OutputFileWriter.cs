using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using DRomsMUtils;

namespace Frontend
{
    public static class OutputFileWriter
    {
        public static bool Write(SingleGameROMGroupSet romGroup, string outputFilePath)
        {
            if (string.IsNullOrEmpty(outputFilePath))
            {
                return false;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Total: {romGroup.TotalROMGroups}");
            sb.AppendLine();

            var groups = romGroup.SingleGameROMGroups;
            foreach (var singleGameROMGroup in groups)
            {
                var bestROMInGroup = singleGameROMGroup.FindSuggestedROM();
                sb.AppendLine(bestROMInGroup.DisplayName);

                var groupROMEntries = singleGameROMGroup.Entries;
                foreach (var groupROMEntry in groupROMEntries)
                {
                    sb.AppendLine($"\t{groupROMEntry.FilenameWithExtension}");
                }
            }

            FileUtilities.WriteAllText(outputFilePath, sb.ToString());

            return true;
        }

        public static bool Write(ROMGroup romGroup, string outputFilePath)
        {
            if (string.IsNullOrEmpty(outputFilePath))
            {
                return false;
            }

            // new XElement()
            var totalROMEntries = romGroup.TotalEntries;
            var romEntries = romGroup.Entries;

            var sb = new StringBuilder();
            sb.AppendLine($"Total: {totalROMEntries}");
            sb.AppendLine();

            foreach (var romEntry in romEntries)
            {
                sb.AppendLine($"{romEntry.FilenameWithExtension}");
            }

            FileUtilities.WriteAllText(outputFilePath, sb.ToString());

            return true;
        }
    }
}