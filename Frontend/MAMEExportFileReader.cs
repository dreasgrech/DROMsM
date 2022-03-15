using System;
using DRomsMUtils;

namespace Frontend
{
    public class MAMEExportFileReader
    {
        public MAMEExportFile ParseFile(string filePath)
        {
            if (!FileUtilities.FileExists(filePath))
            {
                return null;
            }

            var mameExportFile = new MAMEExportFile();
            var mameExportFileEntries = mameExportFile.Entries;
            TextFileReader.ReadFile(filePath, (line, lineNumber) =>
            {
                // Skip the first line since it just contains "Name:" and "Description"
                if (lineNumber == 0)
                {
                    return;
                }

                var firstSpaceIndex = line.IndexOf(" ", StringComparison.Ordinal);
                var gameFilename = line.Substring(0, firstSpaceIndex);
                var gameName = line.Substring(firstSpaceIndex, line.Length - firstSpaceIndex).Trim();

                // Game names in the MAME export file are enclosed in quotation marks, so remove them
                gameName = gameName.Substring(1, gameName.Length - 2);

                var mameExportFileEntry = new MAMEExportFileEntry
                {
                    Filename = gameFilename,
                    Name = gameName
                };

                mameExportFileEntries.Add(mameExportFileEntry);
            });

            return mameExportFile;
        }
    }
}