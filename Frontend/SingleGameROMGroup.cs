using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend
{
    /// <summary>
    /// A collection of SingleROMEntry
    /// </summary>
    public class SingleGameROMGroup : ROMGroup
    {
        public List<ROMEntry> VerifiedGoodDumps { get; }
        public List<ROMEntry> ScoreSortedEntries { get; }

        // public bool ContainsDuplicates => Entries.Count > 1;
        public bool ContainsDuplicates => TotalEntries > 1;
        public bool ContainsVerifiedGoodDumps => VerifiedGoodDumps.Count > 0;

        private static readonly ScoreSortedROMComparer scoreSortedROMComparer = new ScoreSortedROMComparer();

        private bool romsSorted;

        private const string DummyName = "------------------------------------------------------";

        // TODO: why is it telling that base() is redundant??
        public SingleGameROMGroup(): base()
        {
            VerifiedGoodDumps = new List<ROMEntry>();
            ScoreSortedEntries = new List<ROMEntry>();
        }

        //public SingleGameROMGroup(List<SingleROMEntry> entries) : base(entries)
        //{
        //}

        // public void Add(SingleROMEntry entry)
        protected override ROMGroup _Clone()
        {
            var romGroup = new SingleGameROMGroup();
            romGroup.AddRange(Entries);
            return romGroup;
        }

        protected override void _Add(ROMEntry entry)
        {
            // If this is a verified good dump, save a reference to it
            if (entry.HasCode(ROMStandardCodes.VerifiedGoodDump))
            {
                VerifiedGoodDumps.Add(entry);
            }

            //// Add the rom to the score-sorted list
            ScoreSortedEntries.Add(entry);

            romsSorted = false;
        }

        public void SortEntries()
        {
            ScoreSortedEntries.Sort(scoreSortedROMComparer);
            romsSorted = true;
        }

        public ROMEntry FindSuggestedROM()
        {
            if (!romsSorted)
            {
                throw new Exception("ROMs are not sorted");
            }

            return ScoreSortedEntries[0];

            /*
            if (ContainsVerifiedGoodDumps)
            {
                // If we only have one verified good dump in this group, use that one
                if (VerifiedGoodDumps.Count == 1)
                {
                    return VerifiedGoodDumps[0];
                }

                // TODO: If we have more than one good dump in this group, we need to decide what to do...
                return VerifiedGoodDumps[0];
            }

            // TODO: If we don't have any verified good dumps in this group, we need to decide what to do...

            // TODO: avoid roms which are nor (U) or have other languages
            // TODO: avoid roms which have stuff like (sample), (prototype), or (beta) or things like that

            return Entries[0];
            */
        }

        /// <summary>
        /// Creates a SingleGame Rom Group which contains the suggested ROM
        /// and then padded with dummy rom entries instead of the duplicates
        /// </summary>
        public SingleGameROMGroup CreateGroupWithSingleROM(out ROMGroup duplicateRoms)
        {
            duplicateRoms = new SingleGameROMGroup();

            var suggestedROM = FindSuggestedROM();

            var newGroup = new SingleGameROMGroup();
            var entriesCount = TotalEntries;
            for (var i = 0; i < entriesCount; i++)
            {
                var rom = EntryAt(i);
                if (rom == suggestedROM)
                {
                    newGroup.Add(suggestedROM);
                    continue;
                }

                newGroup.Add(CreateDummyROMEntry());
                duplicateRoms.Add(rom);
            }

            // Manually specify that these ROMs are already sorted
            // newGroup.romsSorted = true;
            newGroup.SortEntries();

            return newGroup;
        }

        private ROMEntry CreateDummyROMEntry()
        {
            var dummyROM = new ROMEntry
            {
                DisplayName = DummyName,
                Filename = DummyName,
                FilenameWithExtension = DummyName,
                IsDummyROM = true
            };

            dummyROM.ROMUsageScore = ROMScoreCalculator.GenerateScore(dummyROM);
            return dummyROM;
        }

        public override string ToString()
        {
            var entriesCount = TotalEntries;

            // No name if we don't have any entries
            if (entriesCount < 1)
            {
                return string.Empty;
            }

            // Pick our first entry for now
            var entry = EntryAt(0);

            // If we have any verified good dumps, take use the first verified good dump
            if (ContainsVerifiedGoodDumps)
            {
                entry = VerifiedGoodDumps[0];
            }
            // Otherwise pick the first entry which isn't a dummy ROM
            else
            {
                for (int i = 0; i < entriesCount; i++)
                {
                    var rom = EntryAt(i);
                    if (!rom.IsDummyROM)
                    {
                        entry = rom;
                        break;
                    }

                }
            }

            // return $"{entry.DisplayName} ({entriesCount})";
            // return $"{entry.DisplayName}";
            return $"{entry.DisplayNameWithPath}";
        }
    }

    /*
    /// <summary>
    /// https://stackoverflow.com/a/21886340/44084
    /// Comparer for comparing two keys, handling equality as being greater
    /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            // Ascending
            // int result = x.CompareTo(y);

            // Descending
            int result = y.CompareTo(x);
            if (result == 0)
            {
                return 1; // Handle equality as being greater. Note: this will break Remove(key) or IndexOfKey(key) since the comparer never returns 0 to signal key equality
            }

            return result;
        }
    }
    */
}