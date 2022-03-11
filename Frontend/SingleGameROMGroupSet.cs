using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DRomsMUtils;

namespace Frontend
{
    /// <summary>
    /// A collection of Single Game ROM groups
    /// </summary>
    public class SingleGameROMGroupSet
    {
        public int TotalROMGroups => SingleGameROMGroups.Count;
        public List<SingleGameROMGroup> SingleGameROMGroups { get; }

        public SingleGameROMGroupSet()
        {
            SingleGameROMGroups = new List<SingleGameROMGroup>();
        }

        public void Add(SingleGameROMGroup romGroup)
        {
            SingleGameROMGroups.Add(romGroup);
        }

        /// <summary>
        /// Creates a copy of this ROMGroup Set where in each group
        /// there's only the suggested ROM with the duplicates replaced
        /// with dummy entries.
        /// </summary>
        public SingleGameROMGroupSet FindSuggestedROMs(out MultipleGameROMGroup duplicateROMsGroup)
        {
            duplicateROMsGroup = new MultipleGameROMGroup();

            var romSet = new SingleGameROMGroupSet();
            var romGroupsCount = SingleGameROMGroups.Count;
            for (var i = 0; i < romGroupsCount; i++)
            {
                var romGroup = SingleGameROMGroups[i];
                var groupWithSingleROM = romGroup.CreateGroupWithSingleROM(out var entryDuplicateROMs);
                duplicateROMsGroup.AddRange(entryDuplicateROMs);

                romSet.Add(groupWithSingleROM);
            }

            return romSet;
        }

        public bool SplitIntoDirectories()
        {
            var romGroupsCount = SingleGameROMGroups.Count;
            if (romGroupsCount == 0)
            {
                return false;
            }

            // for (var i = 0; i < romGroupsCount; i++)
            Parallel.ForEach(SingleGameROMGroups, romGroup =>
            {
                // var romGroup = SingleGameROMGroups[i];
                var oneROMFromTheGroup = romGroup.FindSuggestedROM();
                var newDirectoryName = oneROMFromTheGroup.DisplayName;
                var newDirectoryPath = FileUtilities.CombinePath(oneROMFromTheGroup.AbsoluteDirectory, newDirectoryName);

                // romGroup.MoveEntriesFilesToDirectory(newDirectoryPath);
                romGroup.MoveEntriesToDirectory(newDirectoryPath, false);
            });

            return true;
        }
    }
}