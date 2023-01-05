using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DIOUtils;
using DRomsMUtils;
using Frontend;
using U8Xml;

namespace DROMsM.Forms
{
    public class LaunchBoxManager
    {
        private const string VisiblePlatformFileExtension = ".xml";
        private const string HiddenPlatformFileExtension = ".hidden";

        private const string RelativeCorePath = @"Core\";
        private const string RelativePlatformsPath = @"Data\Platforms\";
        private const string RelativeMAMEMetadataFilePath = @"Metadata\Mame.xml";

        private const string LaunchBoxExecutableFilename = @"LaunchBox.exe";

        private readonly string baseLaunchBoxPath;
        private readonly string platformsDirectory;
        private readonly string mameMetadataFilePath;

        public LaunchBoxManager(string launchBoxPath)
        {
            baseLaunchBoxPath = launchBoxPath;
            platformsDirectory = FileUtilities.CombinePath(baseLaunchBoxPath, RelativePlatformsPath);
            mameMetadataFilePath = FileUtilities.CombinePath(baseLaunchBoxPath, RelativeMAMEMetadataFilePath);
        }

        public static bool IsValidLaunchBoxDirectory(string directory, out string searchForLaunchBoxExecutableFilePath)
        {
            var launchBoxCorePath = Path.Combine(directory, RelativeCorePath);
            searchForLaunchBoxExecutableFilePath = Path.Combine(launchBoxCorePath, LaunchBoxExecutableFilename);

            var launchBoxExecutableFileExists = FileUtilities.FileExists(searchForLaunchBoxExecutableFilePath);

            return launchBoxExecutableFileExists;
        }

        public static bool PlatformsDirectoryExists(string directory, out string searchForLaunchBoxPlatformsDirectory)
        {
            searchForLaunchBoxPlatformsDirectory = Path.Combine(directory, RelativePlatformsPath);
            var platformsDirectoryExists = FileUtilities.DirectoryExists(searchForLaunchBoxPlatformsDirectory);

            return platformsDirectoryExists;
        }

        public static bool MAMEMetadataFileExists(string directory, out string searchForLaunchBoxMAMEMetadataFilePath)
        {
            searchForLaunchBoxMAMEMetadataFilePath = Path.Combine(directory, RelativeMAMEMetadataFilePath);
            var exists = FileUtilities.FileExists(searchForLaunchBoxMAMEMetadataFilePath);

            return exists;
        }

        public List<LaunchBoxPlatform> GetPlatforms()
        {
            var platformsFiles = FileUtilities.GetFiles(platformsDirectory, "*.*", SearchOption.TopDirectoryOnly);
            var platformsCollection_threaded = new ConcurrentBag<LaunchBoxPlatform>();

            Parallel.ForEach(platformsFiles, (fileInfo, parallelLoopState, index) =>
            {
                var filePath = fileInfo.FullName;
                var fileExtension = FileUtilities.GetExtension(filePath);
                var platformName = FileUtilities.GetFileNameWithoutExtension(filePath);

                var visible = fileExtension.Equals(VisiblePlatformFileExtension, StringComparison.OrdinalIgnoreCase);

                var platform = new LaunchBoxPlatform
                {
                    Name = platformName,
                    FilePath = filePath,
                    Visible = visible
                };

                platformsCollection_threaded.Add(platform);
            });

            var platformsCollection = new List<LaunchBoxPlatform>();
            foreach (var platform in platformsCollection_threaded)
            {
                platformsCollection.Add(platform);
            }

            platformsCollection.Sort(new LaunchBoxPlatformComparer_Name());

            return platformsCollection;
        }

        public LaunchBoxMAMEMetadataFile ParseMAMEMetadataXMLFile()
        {
            // MessageBoxOperations.ShowInformation(mameMetadataFilePath, mameMetadataFilePath);

            var xml = XmlParser.ParseFile(mameMetadataFilePath);

            var rootNode = xml.Root;
            var rootNodeChildren = rootNode.Children;
            var rootNodeChildrenCount = rootNodeChildren.Count;
            var rootNodeChildrenEnumerable = (IEnumerable<XmlNode>) rootNodeChildren;

            var launchBoxMAMEMetadataFileEntryCollection_threaded = new ConcurrentQueue<LaunchBoxMAMEMetadataFileEntry>();
            Parallel.ForEach(rootNodeChildrenEnumerable, (mameFileNode, parallelLoopState, index) =>
            {
                var mameMetadataFileEntry = new LaunchBoxMAMEMetadataFileEntry();

                var mameFileNodeChildren = mameFileNode.Children;
                Parallel.ForEach(mameFileNodeChildren, mameFileNodeChildNode =>
                {
                    var mameFileNodeChildNodeName = mameFileNodeChildNode.Name.ToString().ToLowerInvariant();
                    var mameFileNodeChildNodeInnerText = mameFileNodeChildNode.InnerText.ToString();
                    switch (mameFileNodeChildNodeName)
                    {
                        case "filename":
                            mameMetadataFileEntry.FileName = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "name":
                            mameMetadataFileEntry.Name = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "status":
                            mameMetadataFileEntry.Status = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "developer":
                            mameMetadataFileEntry.Developer = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "publisher":
                            mameMetadataFileEntry.Publisher = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "year":
                            mameMetadataFileEntry.Year = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "ismechanical":
                            mameMetadataFileEntry.IsMechanical = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isbootleg":
                            mameMetadataFileEntry.IsBootleg = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isprototype":
                            mameMetadataFileEntry.IsPrototype = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "ishack":
                            mameMetadataFileEntry.IsHack = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "ismature":
                            mameMetadataFileEntry.IsMature = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isquiz":
                            mameMetadataFileEntry.IsQuiz = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isfruit":
                            mameMetadataFileEntry.IsFruit = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "iscasino":
                            mameMetadataFileEntry.IsCasino = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isrhythm":
                            mameMetadataFileEntry.IsRhythm = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "istabletop":
                            mameMetadataFileEntry.IsTableTop = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isplaychoice":
                            mameMetadataFileEntry.IsPlayChoice = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "ismahjong":
                            mameMetadataFileEntry.IsMahjong = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "isnonarcade":
                            mameMetadataFileEntry.IsNonArcade = ParseBooleanValue(mameFileNodeChildNodeInnerText);
                            break;
                        case "genre":
                            mameMetadataFileEntry.Genre = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "playmode":
                            mameMetadataFileEntry.Playmode = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "language":
                            mameMetadataFileEntry.Language = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                        case "source":
                            mameMetadataFileEntry.Source = XMLFileOperations.NormalizeText(mameFileNodeChildNodeInnerText);
                            break;
                    }

                });

                launchBoxMAMEMetadataFileEntryCollection_threaded.Enqueue(mameMetadataFileEntry);
            });

            xml.Dispose();

            var mameMetadataFile = new LaunchBoxMAMEMetadataFile();
            foreach (var entry in launchBoxMAMEMetadataFileEntryCollection_threaded)
            {
                mameMetadataFile.AddEntry(entry);
            }

            mameMetadataFile.SortEntries();

            return mameMetadataFile;
        }

        private static bool ParseBooleanValue(string value)
        {
            if (!bool.TryParse(value, out var realBool))
            {
                return false;
            }

            return realBool;
        }

        public bool SetPlatformVisibility(LaunchBoxPlatform platform, bool visibility)
        {
            // If the platform already matches the requested visibility, then there's nothing else to do
            if ((platform.Visible && visibility) || (!platform.Visible && !visibility))
            {
                return false;
            }

            try
            {
                var filePath = platform.FilePath;
                var currentFilename = FileUtilities.GetFileNameWithoutExtension(filePath);
                var newFileExtension = visibility ? VisiblePlatformFileExtension : HiddenPlatformFileExtension;
                var newFileName = $"{currentFilename}{newFileExtension}";
                var newFilePath = Path.Combine(platformsDirectory, newFileName);

                var fileRenamed = FileUtilities.MoveFile(filePath, newFilePath);

                // If the file was successfully renamed, update the model
                if (fileRenamed)
                {
                    platform.Visible = visibility;
                    platform.FilePath = newFilePath;
                }

                return fileRenamed;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class LaunchBoxMAMEMetadataFile
    {
        public List<LaunchBoxMAMEMetadataFileEntry> Entries;// { get; }

        public LaunchBoxMAMEMetadataFile()
        {
            Entries = new List<LaunchBoxMAMEMetadataFileEntry>();
        }

        public void AddEntry(LaunchBoxMAMEMetadataFileEntry entry)
        {
            Entries.Add(entry);
        }

        public void SortEntries()
        {
            var comparer = new LaunchBoxMAMEMetadataFileEntryComparer_FileName();
            Entries.Sort(comparer);
        }
    }
}