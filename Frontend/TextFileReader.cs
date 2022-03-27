using System;
using DRomsMUtils;

namespace DROMsM
{
    /// <summary>
    /// Contains helper methods for reading a text file
    /// </summary>
    public static class TextFileReader
    {
        public static void ReadFile(string filePath, Action<string, int> action)
        {
            if (!FileUtilities.FileExists(filePath))
            {
                return;
            }

            var currentLineNumber = -1;

            using (var enumerator = FileUtilities.ReadLines(filePath).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var line = enumerator.Current;
                    currentLineNumber++;
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    line = line.Trim();

                    action(line, currentLineNumber);
                }
            }
        }
    }
}