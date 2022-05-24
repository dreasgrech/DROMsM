using System.Collections.Generic;
using DROMsM.Forms;

namespace Frontend
{
    public class LaunchBoxPlatformComparer_Name : IComparer<LaunchBoxPlatform>
    {
        public int Compare(LaunchBoxPlatform first, LaunchBoxPlatform second)
        {
            if (first != null && second != null)
            {
                var result = first.Name.CompareTo(second.Name);
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