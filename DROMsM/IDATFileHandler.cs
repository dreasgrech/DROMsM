using System.Collections.Generic;

namespace DROMsM
{
    public interface IDATFileHandler
    {
        DATFile ParseDATFile(string filePath, out HashSet<DATFileMachineField> usedFields);
    }
}