using System.Collections.Generic;

namespace Frontend
{
    public class DatFileMachineComparer_MAMEIndex : IComparer<DATFileMachine>
    {
        public int Compare(DATFileMachine first, DATFileMachine second)
        {
            if (first != null && second != null)
            {
                var result = first.MAMESortingIndex.CompareTo(second.MAMESortingIndex);
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