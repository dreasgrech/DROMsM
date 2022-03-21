using System.Collections.Generic;

namespace Frontend
{
    public class CUEFile : ROMEntry
    {
        public List<string> BINFilesPaths { get; private set; }

        public CUEFile(string absoluteFilePath) : base(absoluteFilePath)
        {
            BINFilesPaths = new List<string>();
        }
    }
}