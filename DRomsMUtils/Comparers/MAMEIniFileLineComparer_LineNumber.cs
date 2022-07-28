using System.Collections.Generic;
using DROMsMUtils;

namespace Frontend
{
    public class MAMEIniFileLineComparer_LineNumber : IComparer<MAMEIniFileLine>
    {
        public int Compare(MAMEIniFileLine line1, MAMEIniFileLine line2)
        {
            return line1.LineNumber > line2.LineNumber ? 1 : -1;
        }
    }
}