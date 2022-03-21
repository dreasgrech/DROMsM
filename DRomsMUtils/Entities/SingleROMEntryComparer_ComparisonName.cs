using System;
using System.Collections.Generic;

namespace Frontend
{
    public class SingleROMEntryComparer_ComparisonName : IComparer<ROMEntry>
    {
        public bool IgnoreRelativeDirectory { get; set; }

        public int Compare(ROMEntry first, ROMEntry second)
        {
            if (first != null && second != null)
            {
                // Compare by path ascending
                // var result = string.Compare(first.AbsoluteDirectory, second.AbsoluteDirectory, StringComparison.Ordinal);
                var result = IgnoreRelativeDirectory ? 0 : string.Compare(first.AbsoluteDirectory, second.AbsoluteDirectory, StringComparison.Ordinal);
                // var result = 0;
                if (result == 0)
                {
                    // Compare by ComparisonName ascending
                    result = string.Compare(first.ComparisonName, second.ComparisonName, StringComparison.Ordinal);
                    if (result == 0)
                    {
                        // Compare by Revision descending
                        result = string.Compare(second.Revision, first.Revision, StringComparison.Ordinal);
                        if (result == 0)
                        {
                            // Compare by ROM score descending
                            // result = (second.ROMUsageScore > first.ROMUsageScore) ? 1 : -1; // TODO: Do not use this because it creates inconsistencies and adds null elements to the list (https://stackoverflow.com/questions/1026044/list-sort-in-c-comparer-being-called-with-null-object)
                            // result = (second.ROMUsageScore > first.ROMUsageScore) ? 1 : first.ROMUsageScore == second.ROMUsageScore ? 0 : -1;
                            result = (second.ROMUsageScore > first.ROMUsageScore) ? 1 : first.ROMUsageScore > second.ROMUsageScore ? -1 : 0;
                            if (result == 0)
                            {
                                // Compare by Track ascending
                                result = string.Compare(first.Track, second.Track, StringComparison.Ordinal);
                            }
                        }
                    }
                }

                return result;
            }

            if (first == null && second == null)
            {
                // We can't compare any properties, so they are essentially equal.
                return 0;
            }

            if (first != null)
            {
                // Only the first instance is not null, so prefer that.
                return -1;
            }

            // Only the second instance is not null, so prefer that.
            return 1;
        }
    }
}