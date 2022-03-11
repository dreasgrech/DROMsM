using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DRomsMUtils;

namespace Frontend
{
    public abstract class ROMGroup
    {
        public List<ROMEntry> Entries { get; }

        public int TotalEntries => Entries.Count;

        protected ROMGroup()
        {
            Entries = new List<ROMEntry>();
        }

        protected abstract ROMGroup _Clone();
        public ROMGroup Clone()
        {
            var clone = _Clone();
            return clone;
        }

        public void Sort(IComparer<ROMEntry> comparer)
        {
            Entries.Sort(comparer);
        }

        //protected ROMGroup(List<SingleROMEntry> entries) : this()
        //{
        //    for (int i = 0; i < entries.Count; i++)
        //    {
        //        var entry = entries[i];
        //        Add(entry);
        //    }
        //}

        public ROMEntry EntryAt(int index)
        {
            return Entries[index];
        }

        protected abstract void _Add(ROMEntry entry);

        public void Add(ROMEntry entry)
        {
            if (entry == null)
            {
                // todo: log it
                return;
            }

            // Add the rom to the main collection
            Entries.Add(entry);

            _Add(entry);
        }

        public void AddRange(List<ROMEntry> entries)
        {
            var entriesCount = entries.Count;
            for (var i = 0; i < entriesCount; i++)
            {
                Add(entries[i]);
            }
        }

        public void AddRange(ROMGroup romGroup)
        {
            AddRange(romGroup.Entries);
        }

        /*
        public void MoveEntriesWithTheirSubdirectoriesToDirectory(string destinationDirectory)
        {
            // Make sure the directory exists
            if (!Directory.Exists(destinationDirectory))
            {
                Logger.Log($"Creating directory {destinationDirectory}");
                Directory.CreateDirectory(destinationDirectory);
            }

            // Move all the roms to the new directory
            Parallel.ForEach(Entries, rom =>
            {
                var currentFilePath = rom.FilePath;
                // var newFilePath = Path.Combine(destinationDirectory, rom.FilenameWithExtension);
                var newFilePath = Path.Combine(destinationDirectory, rom.RelativeFilePath);
                var newFilePathDirectory = Path.GetDirectoryName(newFilePath);

                // Make sure that the destination directory exists
                if (!Directory.Exists(newFilePathDirectory))
                {
                    Directory.CreateDirectory(newFilePathDirectory);
                }

                File.Move(currentFilePath, newFilePath);
            });
        }
        */

        /*
        public void MoveEntriesFilesToDirectory(string destinationDirectory)
        {
            if (!Directory.Exists(destinationDirectory))
            {
                Logger.Log($"Creating directory {destinationDirectory}");
                Directory.CreateDirectory(destinationDirectory);
            }

            Parallel.ForEach(Entries, rom =>
            {
                var currentFilePath = rom.FilePath;
                var newFilePath = Path.Combine(destinationDirectory, rom.FilenameWithExtension);

                File.Move(currentFilePath, newFilePath);
            });
        }
        */

        public void MoveEntriesToDirectory(string destinationDirectory, bool moveAlongWithParentDirectory)
        {
            // Make sure the directory exists
            if (!FileUtilities.DirectoryExists(destinationDirectory))
            {
                Logger.Log($"Creating directory {destinationDirectory}");
                // Directory.CreateDirectory(destinationDirectory);
                FileUtilities.CreateDirectory(destinationDirectory);
            }

            // Move all the roms to the new directory
            Parallel.ForEach(Entries, rom =>
            {
                var currentFilePath = rom.FilePath;
                string newFilePath;

                if (moveAlongWithParentDirectory)
                {
                    newFilePath = FileUtilities.CombinePath(destinationDirectory, rom.RelativeFilePath);

                    // Make sure that the destination directory exists
                    var newFilePathDirectory = Path.GetDirectoryName(newFilePath);
                    if (string.IsNullOrEmpty(newFilePathDirectory))
                    {
                        Logger.LogError($"Unable to read the path from {newFilePathDirectory}");
                    }

                    if (!string.IsNullOrEmpty(newFilePathDirectory) && !FileUtilities.DirectoryExists(newFilePathDirectory))
                    {
                        // Directory.CreateDirectory(newFilePathDirectory);
                        FileUtilities.CreateDirectory(newFilePathDirectory);
                    }
                }
                else
                {
                    newFilePath = FileUtilities.CombinePath(destinationDirectory, rom.FilenameWithExtension);
                }

                // Move the file from the current file path to the new path
                Logger.Log($"Moving {currentFilePath} to {newFilePath}");
                // File.Move(currentFilePath, newFilePath);
                FileUtilities.MoveFile(currentFilePath, newFilePath);
            });
        }
    }
}