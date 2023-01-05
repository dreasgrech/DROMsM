﻿using System.Collections.Generic;
using DROMsM.Forms;

namespace Frontend
{
    public class LaunchBoxMAMEMetadataFileEntryComparer_FileName : IComparer<LaunchBoxMAMEMetadataFileEntry>
    {
        public int Compare(LaunchBoxMAMEMetadataFileEntry first, LaunchBoxMAMEMetadataFileEntry second)
        {
            if (first != null && second != null)
            {
                var result = first.FileName.CompareTo(second.FileName);
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