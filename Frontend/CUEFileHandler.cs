using System.Drawing;
using DRomsMUtils;

namespace Frontend
{
    public class CUEFileHandler
    {
        private const string BinFileStartOfLine = "FILE ";

        public CUEFile ParseCUEFile(string cueFilePath)
        {
            if (!FileUtilities.FileExists(cueFilePath))
            {
                return null;
            }

            TextFileReader.ReadFile(cueFilePath, (line, lineNumber) =>
            {
                if (!line.StartsWith(BinFileStartOfLine))
                {
                    return;
                }

                // TODO: Continue here
                // TODO: Continue here
                // TODO: Continue here
            });

            return null;
        }
    }
}