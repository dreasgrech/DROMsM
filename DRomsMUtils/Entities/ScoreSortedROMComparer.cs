using System.Collections.Generic;

namespace Frontend
{
    public class ScoreSortedROMComparer : IComparer<ROMEntry>
    {
        public int Compare(ROMEntry x, ROMEntry y)
        {
            return y.ROMUsageScore.CompareTo(x.ROMUsageScore);
            // return -x.CompareTo(y);
        }
    }
}