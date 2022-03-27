using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeProject;
using DRomsMUtils;
using Opulos.Core.IO;

namespace Frontend
{
    public enum MainState
    {
        None,
        FindDuplicatesAnalysis,
        SplitIntoGroupsAnalysis,
        MoveAllROMsToRoot,
        AnalyzeTopLevelDirectories,
        CombineMultipleBinsToOne,
        RemoveROMsFromMAMEExportFile
    }

    public class MainManager
    {
        public int UnprocessedTotalEntries => allGamesROMGroup.TotalEntries;

        // public double AllowedSimilarityValue { get; set; }

        private bool resultsOptions_showTotals = false;

        private const string DuplicatesDirectoryName = "Duplicates";
        private const string ProcessedDirectoryName = "Processed";

        private string currentRomsPath;
        private MultipleGameROMGroup currentDuplicatesROMGroup;

        private MultipleGameROMGroup allGamesROMGroup;

        private MainState currentState;

        private readonly RBGameListFileReader recalboxGameListReader;

        private readonly MainForm mainForm;
        private readonly BlueSimilarity.DamerauLevenshtein dl;

        private bool rbGameListFileAvailable;

        public bool UsingRBGameFile => rbGameListFileAvailable;

        public RBGameListFile rbGameListFile;

        private bool romDirectoryAnalyzed;

        // public readonly OperationsOptions options;

        private string romsPathDirectoryInfoFullName;
        // private ROMEntry[] allROMEntriesThreadSafe;
        private ConcurrentBag<ROMEntry> allROMEntriesThreadSafe;
        private const int MaxAssumedEntriesPerDirectory = 500;

        private SingleGameROMGroupSet groupedByDirectoryROMGroupSet;
        private MultipleGameROMGroup movedToRootRomGroup;
        private MultipleGameROMGroup cueFilesRomGroup;
        private MultipleGameROMGroup fromMAMEFileRomGroup;
        private RomDirectory topLevelDirectory;

        /// <summary>
        /// Regex constructed from https://stackoverflow.com/a/4892517/44084 and is a combination (|) of two expressions:
        /// (?<=\().*?(?=\))
        /// (?<=\[).*?(?=\])
        /// It allows me to read the value between ( and ), and [ an ].
        /// </summary>
        private readonly Regex parenthesesMatchRegex = new Regex(@"(?<=\().*?(?=\))|(?<=\[).*?(?=\])");
        private readonly Regex pirateROMNameRegex = new Regex(@"^p\d+$");
        private readonly Regex badROMNameRegex = new Regex(@"^b\d+$");
        private readonly Regex alternativeROMNameRegex = new Regex(@"^a(lt)?(\d+)?$");
        private readonly Regex multiLanguageROMNameRegex = new Regex(@"^m(\d+)?$");
        private readonly Regex hackedMapperROMNameRegex = new Regex(@"^hm ?(.+)?$");
        // private readonly Regex hackedROMNameRegex = new Regex(@"^h(\d+)?$");
        private readonly Regex hackedROMNameRegex = new Regex(@"^h(\d+)?c?$");
        private readonly Regex fixedROMNameRegex = new Regex(@"^f(\d+)?$");
        // private readonly Regex overdumpROMNameRegex = new Regex(@"^o(\d+)?$");
        private readonly Regex overdumpROMNameRegex = new Regex(@"^o(\d+)?(.*)?$");
        // private readonly Regex trainerROMNameRegex = new Regex(@"^t(\d+)?$");
        private readonly Regex trainerROMNameRegex = new Regex(@"^t(\d+)?(.*)?$");
        // private readonly Regex revisionsROMNameRegex = new Regex(@"^(rev ?(.+)?)|(prg ?(.+)?)|(v ?(.+))$");
        private readonly Regex revisionsROMNameRegex = new Regex(@"^(rev ?(.+)?)$|^(prg ?(.+)?)$|^(v ?(.+))$");
        private readonly Regex trackROMNameRegex = new Regex(@"^(track ?(.+)?)$");

        // private readonly Regex revisionMatchRegex = new Regex(@"(?<=\().*?(?=\))");
        //private readonly Regex revisionMatchRegex = new Regex(@"(?<=v).*?");
        // private readonly Regex revisionMatchRegex = new Regex(@"(?<=v).*?(?=)");
        private readonly Regex revisionMatchRegex = new Regex(@"(?<=v)(.*)|(?<=prg)(.*)|(?<=rev)(.*)");
        private readonly Regex trackMatchRegex = new Regex(@"(?<=track)(.*)");

        private readonly Dictionary<string, ROMCountry> romCountries = new Dictionary<string, ROMCountry>( /* iequalitycomaprer */)
        {
            {"1", ROMCountry.JapanKorea},
            {"4", ROMCountry.USABrazilNTSC},
            {"a", ROMCountry.Australia},
            {"as", ROMCountry.Asia},
            {"b", ROMCountry.Brazil},
            {"c", ROMCountry.Canada},
            {"ch", ROMCountry.China},
            {"d", ROMCountry.Dutch},
            {"f", ROMCountry.France},
            {"fc", ROMCountry.FrenchCanadian},
            {"g", ROMCountry.Germany},
            {"gr", ROMCountry.Greece},
            {"hk", ROMCountry.HongKong},
            {"i", ROMCountry.Italy},
            {"j", ROMCountry.Japan},
            {"japan", ROMCountry.Japan},
            {"ju", ROMCountry.JapanUSA},
            {"k", ROMCountry.Korea},
            {"nl", ROMCountry.Holland},
            {"no", ROMCountry.Norway},
            {"r", ROMCountry.Russia},
            {"s", ROMCountry.Spain},
            {"sw", ROMCountry.Sweden},
            {"u", ROMCountry.USA},
            {"uk", ROMCountry.UnitedKingdom},
            {"w", ROMCountry.World},
            {"unk", ROMCountry.Unknown},
            {"e", ROMCountry.Europe},
            {"english", ROMCountry.Europe},
            {"ue", ROMCountry.USAEurope},
        };

        private readonly Dictionary<string, ROMLicense> romLicenses = new Dictionary<string, ROMLicense>( /* iequalitycomaprer */)
        {
            {"unl", ROMLicense.Unlicensed},
            {"pd", ROMLicense.PublicDomain},
        };

        private readonly HashSet<string> fileTypesToIgnore = new HashSet<string>()
        {
            ".txt",
            ".xml",
            ".mp4",
            ".png",
            ".pdf",
            ".conf",
            ".cfg",
            ".exe",
        };

        private readonly HashSet<string> topLevelDirectoriesToIgnore = new HashSet<string>()
        {
            "media",
            DuplicatesDirectoryName,
        };

        public MainManager(MainForm mainForm)
        {
            this.mainForm = mainForm;

            // OperationsOptions.Instance.AllowedSimilarityValue = 0.9;

            // var gameListFilePath = @"D:\dromsmanagerdirs\gamelist_neogeocd.xml";
            // var gameListFilePath = @"D:\dromsmanagerdirs\gamelist_nes.xml";
            recalboxGameListReader = new RBGameListFileReader();
            // recalboxGameListReader.ReadGameListFile(gameListFilePath);

            // ReadROMAttributes(new SingleROMEntry{Filename = "My Game! (U)(K) (pd) [!] [h]"});

            dl = new BlueSimilarity.DamerauLevenshtein();

            // options = new OperationsOptions();

            /*
            // Normalize the ROM Symbols
            for (var i = 0; i < RomSymbols.Length; i++)
            {
                var romSymbol = RomSymbols[i];
                romSymbol = romSymbol.ToLowerInvariant();
                RomSymbols[i] = romSymbol;
            }
            */

            // Normalize the ROM Countries list
            var romCountriesKeys = romCountries.Keys.ToList();
            using (var romCountriesKeysEnumerator = romCountriesKeys.GetEnumerator())
            {
                while (romCountriesKeysEnumerator.MoveNext())
                {
                    var romCountryKey = romCountriesKeysEnumerator.Current;
                    RenameDictionaryKey(romCountries, romCountryKey, romCountryKey.ToLowerInvariant());
                }
            }

            var romLicensesKeys = romLicenses.Keys.ToList();
            using (var romLicensesKeysEnumerator = romLicensesKeys.GetEnumerator())
            {
                while (romLicensesKeysEnumerator.MoveNext())
                {
                    var romLicensesKey = romLicensesKeysEnumerator.Current;
                    RenameDictionaryKey(romLicenses, romLicensesKey, romLicensesKey.ToLowerInvariant());
                }
            }
        }

        public void AnalyzeROMsDirectory(string romsPath)
        {
            if (string.IsNullOrEmpty(romsPath))
            {
                return;
            }

            // Normalize the slashes in the roms path
            romsPath = FileUtilities.NormalizePathSlashes(romsPath);

            //// If the last character of the roms path is a '\', remove it
            //if (romsPath[romsPath.Length - 1] == '\\')
            //{
            //    romsPath = romsPath.Substring(0, romsPath.Length - 1);
            //}

            if (!FileUtilities.DirectoryExists(romsPath))
            {
                Logger.Log($"Unable to analyze ROM directory because it does not exist: {romsPath}");
                return;
            }

            Logger.Log($"Analyzing ROM directory: {romsPath}");

            currentRomsPath = romsPath;

            /// Recalbox GameFile
            rbGameListFile = ReadRBGameFile(romsPath);
            rbGameListFileAvailable = rbGameListFile != null;

            allGamesROMGroup = CreateAllROMEntries(romsPath);

            mainForm.OnFinishedAnalyzingROMDirectory(allGamesROMGroup);

            // OutputFileWriter.Write(allGamesROMGroup, @"D:\dromsmanagerdirs\roms\output.txt");

            Logger.Log($"Finished analyzing ROM directory: {romsPath}");

            romDirectoryAnalyzed = true;
        }

        public void ExecuteRemoveEmptyTopLevelDirectoriesOperation()
        {
            topLevelDirectory = FindEmptyTopLevelDirectories();

            mainForm.OnFinishedAnalyzingTopLevelDirectoriesOperation(topLevelDirectory);

            currentState = MainState.AnalyzeTopLevelDirectories;
        }

        private RomDirectory FindEmptyTopLevelDirectories()
        {
            var allTopDirectories = FileUtilities.GetDirectories(currentRomsPath, "*", SearchOption.TopDirectoryOnly);
            var allTopDirectoriesLength = allTopDirectories.Length;
            var topDirectoriesThreaded = new RomDirectory[allTopDirectoriesLength];

            Parallel.For(0, allTopDirectoriesLength, i =>
            {
                var path = allTopDirectories[i];

                // FileUtilities.GetDirectoryInfo(path, out var directoryName, out var directoryFullName);
                //var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
                //var directoryName = directoryInfo.Name;
                var directoryName = FileUtilities.GetNameOfDirectory(path);

                // Make sure that this is not an ignored directory
                foreach (var directoryNameToIgnore in topLevelDirectoriesToIgnore)
                {
                    if (string.Equals(directoryName, directoryNameToIgnore, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }
                }

                // Check if the directory is empty
                var filesInDirectory = FileUtilities.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
                var foldersInDirectory = FileUtilities.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                var isDirectoryEmpty = filesInDirectory.Count == 0 && foldersInDirectory.Length == 0;

                if (!isDirectoryEmpty)
                {
                    return;
                }

                var romDirectory = new RomDirectory
                {
                    FullPath = path
                };

                topDirectoriesThreaded[i] = romDirectory;
            });

            var topDirectories = topDirectoriesThreaded.Where(d => d != null).ToList();

            var rootDirectory = new RomDirectory
            {
                FullPath = currentRomsPath,
                SubDirectories = topDirectories
            };

            return rootDirectory;
        }

        public void ExecuteFindDuplicateROMsOperation()
        {
            if (!romDirectoryAnalyzed)
            {
                return;
            }

            // Create a ROM Group set where the ROMs are grouped by name similarity
            var allROMsGroupSet = CreateSimilarityGroupedROMGroupSet(allGamesROMGroup);
            mainForm.PopulateLeftTreeView(allROMsGroupSet, true, false);

            // Create a ROM Group Set where the recommended ROM is chosen and the duplicates are replaced by dummy entries
            var suggestedROMSet = allROMsGroupSet.FindSuggestedROMs(out currentDuplicatesROMGroup);
            // mainForm.PopulateRightTreeView(suggestedROMSet, true, true);

            mainForm.OnFinishedFindingDuplicateROMsOperation(suggestedROMSet, currentDuplicatesROMGroup);

            currentState = MainState.FindDuplicatesAnalysis;
        }

        public void ExecuteGroupROMsOperation()
        {
            if (!romDirectoryAnalyzed)
            {
                return;
            }

            // Create a ROM Group set where the ROMs are grouped by name similarity
            groupedByDirectoryROMGroupSet = CreateSimilarityGroupedROMGroupSet(allGamesROMGroup);

            // mainForm.PopulateRightTreeView(groupedByDirectoryROMGroupSet, false, true);

            mainForm.OnFinishedProcessingIntoDirectoriesOperation(groupedByDirectoryROMGroupSet);

            currentState = MainState.SplitIntoGroupsAnalysis;
        }

        public void ExecuteMoveAllROMsToRootOperation()
        {
            if (!romDirectoryAnalyzed)
            {
                return;
            }

            // TODO: Create MultipleGameROMGroup which contains all the ROMs
            movedToRootRomGroup = (MultipleGameROMGroup)allGamesROMGroup.Clone();

            // Create a ROM Group set where the ROMs are grouped by name similarity
            // groupedByDirectoryROMGroupSet = CreateSimilarityGroupedROMGroupSet(allGamesROMGroup, false);

            mainForm.OnFinishedMovingAllROMsToRootOperation(movedToRootRomGroup);

            currentState = MainState.MoveAllROMsToRoot;
        }

        public void ExecuteConvertMultipleBinsToOneOperation()
        {
            if (!romDirectoryAnalyzed)
            {
                return;
            }

            var cueFilesToUse_ThreadSafe = new ConcurrentBag<ROMEntry>();

            Parallel.ForEach(allGamesROMGroup.Entries, romEntry =>
            {
                if (romEntry.FileType != ROMFileType.Cue)
                {
                    return;
                }

                //// Ignore ROMs in the Processed directory
                //var isInProcessedDirectory = FileUtilities.PathContainsDirectory(romEntry.AbsoluteFilePath, ProcessedDirectoryName);
                //if (isInProcessedDirectory)
                //{
                //    return;
                //}

                cueFilesToUse_ThreadSafe.Add(romEntry);
            });

            cueFilesRomGroup = new MultipleGameROMGroup();
            cueFilesRomGroup.AddRange(cueFilesToUse_ThreadSafe.ToList());
            cueFilesRomGroup.Sort(false);

            mainForm.OnFinishedCombineMultipleBinsIntoOneOperation(cueFilesRomGroup);

            currentState = MainState.CombineMultipleBinsToOne;
        }

        public void ExecuteRemoveROMsFromMameFileOperation(string mameExportFilePath)
        {
            if (!romDirectoryAnalyzed)
            {
                return;
            }

            var mameExportFileReader = new MAMEExportFileReader();
            var mameFile = mameExportFileReader.ParseFile(mameExportFilePath);

            var matchingROMEntries_ThreadSafe = new ConcurrentBag<ROMEntry>();

            // var mameFileEntries = mameFile.Entries;
            Parallel.ForEach(allGamesROMGroup.Entries, romEntry =>
            {
                //// Ignore ROMs in the Processed directory
                //var isInProcessedDirectory = FileUtilities.PathContainsDirectory(romEntry.AbsoluteFilePath, ProcessedDirectoryName);
                //if (isInProcessedDirectory)
                //{
                //    return;
                //}

                // Check if this entry
                if (!mameFile.TryResolve(romEntry.Filename, out var entryFromFile))
                {
                    return;
                }

                matchingROMEntries_ThreadSafe.Add(romEntry);
            });

            fromMAMEFileRomGroup = new MultipleGameROMGroup();
            fromMAMEFileRomGroup.AddRange(matchingROMEntries_ThreadSafe.ToList());
            fromMAMEFileRomGroup.Sort(false);

            mainForm.OnFinishedRemoveROMsFromMAMEExportFileOperation(mameExportFilePath, fromMAMEFileRomGroup);

            currentState = MainState.RemoveROMsFromMAMEExportFile;
        }

        public bool ExecuteRemoveEmptyTopLevelDirectoriesSubOperation(out int directoriesDeleted, out int directoriesNotDeleted)
        {
            directoriesNotDeleted = 0;
            directoriesDeleted = 0;

            if (currentState != MainState.AnalyzeTopLevelDirectories)
            {
                return false;
            }

            var totalNotDeleted = 0;

            var topLevelDirectories = topLevelDirectory.SubDirectories;
            Parallel.ForEach(topLevelDirectories, romDirectory =>
            {
                var directoryPath = romDirectory.FullPath;
                try
                {
                    FileUtilities.DeleteDirectory(directoryPath);
                }
                catch (Exception)
                {
                    Interlocked.Add(ref totalNotDeleted, 1);
                }
            });

            var totalTopLevelDirectories = topLevelDirectories.Count;

            directoriesNotDeleted = totalNotDeleted;
            directoriesDeleted = totalTopLevelDirectories - directoriesNotDeleted;

            // Reprocess the ROMs list
            AnalyzeROMsDirectory(currentRomsPath);

            return true;
        }

        public ROMGroup ExecuteRemoveDuplicateDOMsSubOperation()
        {
            var removedROMsGroup = new SingleGameROMGroup();

            if (currentState != MainState.FindDuplicatesAnalysis)
            {
                return removedROMsGroup;
            }

            var duplicatesDirectory = FileUtilities.CombinePath(currentRomsPath, DuplicatesDirectoryName);
            // currentDuplicatesROMGroup.MoveEntriesWithTheirSubdirectoriesToDirectory(duplicatesDirectory);
            currentDuplicatesROMGroup.MoveEntriesToDirectory(duplicatesDirectory, true);

            // Make a copy of the removed roms list
            removedROMsGroup.AddRange(currentDuplicatesROMGroup);

            // Reprocess the ROMs list
            AnalyzeROMsDirectory(currentRomsPath);

            ExecuteFindDuplicateROMsOperation();

            return removedROMsGroup;
        }

        public bool ExecuteGroupIntoDirectoriesSubOperation()
        {
            if (currentState != MainState.SplitIntoGroupsAnalysis)
            {
                return false;
            }

            var split = groupedByDirectoryROMGroupSet.SplitIntoDirectories();
            if (!split)
            {
                return false;
            }

            // Reprocess the ROMs list
            AnalyzeROMsDirectory(currentRomsPath);

            // ExecuteGroupROMsOperation();

            return true;
        }

        public bool ExecuteMoveROMsToRootSubOperation()
        {
            if (currentState != MainState.MoveAllROMsToRoot)
            {
                return false;
            }

            movedToRootRomGroup.MoveEntriesToDirectory(romsPathDirectoryInfoFullName, false);

            // Reprocess the ROMs list
            AnalyzeROMsDirectory(currentRomsPath);

            return true;
        }

        public MultipleGameROMGroup ExecuteConvertMultipleBinsToOneSubOperation()
        {
            if (currentState != MainState.CombineMultipleBinsToOne)
            {
                return null;
            }

            // TODO: This needs to change such that chdman.exe is copied to the root directory of the roms and executed from there.
            // TODO: This is because if you feed in a path which is longer than the allowed Windows limit the CHD man, it will fail

            var chdmanHandler = new CHDMANHandler();

            var outputProcessedDirectory = FileUtilities.CombinePath(currentRomsPath, ProcessedDirectoryName);

            var processedRoms = new ConcurrentBag<ROMEntry>();

            // https://stackoverflow.com/a/9290531/44084
            // var cpuPercentageToUse = 0.5f;
            //var parallelOptions = new ParallelOptions
            //{
            //    // MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * cpuPercentageToUse) * 1.0))
            //    MaxDegreeOfParallelism = 2
            //};

            var operationStarted = chdmanHandler.StartOperation(currentRomsPath);
            if (!operationStarted)
            {
                return null;
            }

            try
            {
                // TODO: Switch back to Parallel once you're done testing.
                // TODO: ACTUALLY, Running more than one at a time was actually causing my CPU to max at 100%, so probably best to let this run synchronously.
                // Parallel.ForEach(cueFilesRomGroup.Entries, parallelOptions, romEntry =>
                foreach (var romEntry in cueFilesRomGroup.Entries)
                {
                    var cueFilePath = romEntry.AbsoluteFilePath;
                    var chdOutputDirectory = FileUtilities.CombinePath(outputProcessedDirectory, romEntry.Filename);
                    var combinedCueFilePath = chdmanHandler.CombineMultipleBinsIntoOne(cueFilePath, chdOutputDirectory);
                    Logger.Log($"Combined cue: {combinedCueFilePath}");

                    processedRoms.Add(romEntry);
                    // });
                }

                var operationStopped = chdmanHandler.StopOperation();
                if (!operationStopped)
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                chdmanHandler.StopOperation();

                return null;
            }

            var romsGroup = new MultipleGameROMGroup();
            romsGroup.AddRange(processedRoms.ToList());
            romsGroup.Sort(false);

            // Reprocess the ROMs list
            AnalyzeROMsDirectory(currentRomsPath);

            return romsGroup;
        }

        public bool ExecuteRemoveROMsFromMAMEExportFileSubOperation(out string outputDirectory)
        {
            outputDirectory = string.Empty;

            if (currentState != MainState.RemoveROMsFromMAMEExportFile)
            {
                return false;
            }

            var processedDirectory = FileUtilities.CombinePath(currentRomsPath, ProcessedDirectoryName);
            fromMAMEFileRomGroup.MoveEntriesToDirectory(processedDirectory, true);

            // Reprocess the ROMs list
            AnalyzeROMsDirectory(currentRomsPath);

            outputDirectory = processedDirectory;

            return true;
        }

        private void ProcessFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                // continue;
                return;
            }

            /*
            // Ignore ROMs in our special directories
            var isInSpecialDirectory =
                FileUtilities.PathContainsDirectory(filePath, ProcessedDirectoryName) ||
                FileUtilities.PathContainsDirectory(filePath, DuplicatesDirectoryName);

            if (isInSpecialDirectory)
            {
                return;
            }
            */

            // var directory = FileUtilities.GetDirectoryName(filePath);
            var directory = FileUtilities.GetDirectory(filePath);
            if (directory == null)
            {
                return;
            }

            // var directoryInfo = new DirectoryInfo(directory);
            FileUtilities.GetDirectoryInfo(directory, out var directoryName, out var absoluteDirectory);
            //var directoryInfo = FileUtilities.CreateDirectoryInfo(directory);
            //var directoryName = directoryInfo.Name;

            // Ignore ROMs in our special directories
            if (
                string.Equals(directoryName, DuplicatesDirectoryName, StringComparison.OrdinalIgnoreCase) || 
                string.Equals(directoryName, ProcessedDirectoryName, StringComparison.OrdinalIgnoreCase)
                )
            {
                return;
            }

            var fileExtension = FileUtilities.GetExtension(filePath);

            // Check if we should ignore this file extension
            if (fileTypesToIgnore.Contains(fileExtension))
            {
                // continue;
                Logger.Log($"Skipping {filePath}");
                return;
            }

            var filename = FileUtilities.GetFileNameWithoutExtension(filePath);
            var filenameWithExtension = $"{filename}{fileExtension}";

            var displayName = ROMEntry.RemoveROMSymbols(filename);

            // var absoluteDirectory = directoryInfo.FullName;
            var relativeFilePath = filePath.Replace(romsPathDirectoryInfoFullName, string.Empty);

            if (relativeFilePath[0] == FileUtilities.DirectoryBackSlash)
            {
                relativeFilePath = relativeFilePath.Substring(1, relativeFilePath.Length - 1);
            }

            string romSubDirectoryPath = string.Empty;
            var relativeFilePathLastIndexOf = relativeFilePath.LastIndexOf(FileUtilities.DirectoryBackSlash);
            if (relativeFilePathLastIndexOf >= 0)
            {
                romSubDirectoryPath = relativeFilePath.Substring(0, relativeFilePathLastIndexOf + 1);
            }

            // var displayNameWithPath = Path.ChangeExtension(relativeFilePath, null);
            var displayNameWithPath = FileUtilities.CombinePath(romSubDirectoryPath, displayName);

            var singleROMEntry = new ROMEntry(filePath)
            {
                Filename = filename,
                FilenameWithExtension = filenameWithExtension,
                AbsoluteDirectory = absoluteDirectory,
                RelativeFilePath = relativeFilePath,
                RelativeSubDirectory = romSubDirectoryPath,
                ComparisonName = ROMEntry.ConvertFilenameForComparison(filename),
                DisplayName = displayName,
                DisplayNameWithPath = displayNameWithPath,
            };

            if (rbGameListFileAvailable)
            {
                singleROMEntry.GameListFileEntry = rbGameListFile.ResolveGame(filenameWithExtension);
            }

            SaveROMAttributes(singleROMEntry);
            singleROMEntry.ROMUsageScore = ROMScoreCalculator.GenerateScore(singleROMEntry);

            // allROMEntriesThreadSafe[i] = singleROMEntry;
            allROMEntriesThreadSafe.Add(singleROMEntry);
        }

        private MultipleGameROMGroup CreateAllROMEntries(string romsPath)
        {
            // currentRomsPath = romsPath;

            // var dir = @"D:\dromsmanagerdirs\roms";
            // var allFilesPaths = Directory.GetFiles(dir);
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\neslist.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\mame2015list.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\sneslist.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\bd.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\problems.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\problems2.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\problems3.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\pcenginecdlist.txt");
            // var allFilesPaths = File.ReadAllLines(@"D:\dromsmanagerdirs\roms\mamelist.txt");

            // var stopwatch = StopwatchManager.CreateStopWatch();

            
            
            /*
            stopwatch.Start();
            FastFileInfo.GetFiles(romsPath, "*", SearchOption.AllDirectories, null);
            StopShowAndResetStopWatchTime(stopwatch, "FastFileInfo.GetFiles");

            stopwatch.Start();
            Directory.GetFiles(romsPath, "*.*", SearchOption.AllDirectories);
            StopShowAndResetStopWatchTime(stopwatch, "Directory.GetFiles");

            stopwatch.Start();
            FastDirectoryEnumerator.GetFiles(romsPath, "*", SearchOption.AllDirectories);
            StopShowAndResetStopWatchTime(stopwatch, "FastDirectoryEnumerator.GetFiles");
            */




            // var romsPathDirectoryInfo = new DirectoryInfo(romsPath);
            // FileUtilities.GetDirectoryInfo(romsPath, out var directoryName, out romsPathDirectoryInfoFullName);
            // var romsPathDirectoryInfo = FileUtilities.CreateDirectoryInfo(romsPath);
            // romsPathDirectoryInfoFullName = romsPathDirectoryInfo.FullName;
            romsPathDirectoryInfoFullName = FileUtilities.GetDirectoryFullname(romsPath);

            /*
            // stopwatch.Start();
            // var allFilesPaths = Directory.GetFiles(romsPath, "*.*", SearchOption.AllDirectories);
            // var allFilesPaths = FastFileInfo.GetFiles2(romsPath, "*", true);
            var allFilesPaths = FastFileInfo.GetFiles(romsPath, "*", SearchOption.AllDirectories, null);
            // StopShowAndResetStopWatchTime(stopwatch, "fetching files from directory");

            int allFilesPathsLength = allFilesPaths.Count;
            */

            // allROMEntriesThreadSafe = new ROMEntry[allFilesPathsLength];
            // allROMEntriesThreadSafe = new ROMEntry[100000*MaxAssumedEntriesPerDirectory];
            allROMEntriesThreadSafe = new ConcurrentBag<ROMEntry>();

            // Fetch all the directories in our ROMS directory
            var directoriesToScan = Directory.GetDirectories(romsPath, "*", SearchOption.TopDirectoryOnly);//.ToList();

            // Add our ROMS directory to the list as well so that we scan it as well
            // directoriesToScan.Add(romsPath);
            // ProcessFilesInDirectory(romsPath, SearchOption.TopDirectoryOnly, 0);
            ProcessFilesInDirectory(romsPath, SearchOption.TopDirectoryOnly);

            Parallel.ForEach(directoriesToScan, directory =>
            // Parallel.For(0, directoriesToScan.Length, (int j) =>
            {
                // var directory = directoriesToScan[j];
                // ProcessFilesInDirectory(directory, SearchOption.AllDirectories, j + 1); // The '+ 1' is to offset the root directory which was processed before
                ProcessFilesInDirectory(directory, SearchOption.AllDirectories);

                /*
                var allFilesInDirectory = FastFileInfo.GetFiles(directory, "*", SearchOption.AllDirectories);
                Parallel.For(0, allFilesInDirectory.Count, (int i) =>
                {
                    var fastFileInfo = allFilesInDirectory[i];
                    // var filePath = allFilesPaths[i];
                    var filePath = fastFileInfo.FullName;
                    ProcessFile(filePath, (j * MaxAssumedEntriesPerDirectory) + i);
                });
                */
            });

            // Logger.Log($"After All parallel done.  allROMEntriesThreadSafe.Count: {allROMEntriesThreadSafe.Count}");

            // stopwatch.Start();

            /*
            Parallel.For(0, allFilesPathsLength, (int i) =>
            {
                var fastFileInfo = allFilesPaths[i];
                // var filePath = allFilesPaths[i];
                var filePath = fastFileInfo.FullName;
                ProcessFile(filePath, i);
            });
            */
            // StopShowAndResetStopWatchTime(stopwatch, "threaded for");


            // stopwatch.Start();
            // Take the ones which are not null since we might have skipped some files
            var allROMEntriesList = allROMEntriesThreadSafe.Where(rom => rom != null).ToList();

            // Sort all the ROM entries by their comparison name and other criteria so they can be grouped
            var singleROMEntryComparer_ComparisonName = new SingleROMEntryComparer_ComparisonName {IgnoreRelativeDirectory = false};
            allROMEntriesList.Sort(singleROMEntryComparer_ComparisonName);

            var multipleGameROMGroup = new MultipleGameROMGroup();
            multipleGameROMGroup.AddRange(allROMEntriesList);

            // StopShowAndResetStopWatchTime(stopwatch, "other");

            return multipleGameROMGroup;
        }

        /*
        private ROMGroupSet CreateAllGamesROMGroupSet()
        {
            var romGroupSet = new ROMGroupSet();
            var allRomEntriesCount = allGamesROMGroup.TotalEntries;
            for (var i = 0; i < allRomEntriesCount; i++)
            {
                var rom = allGamesROMGroup.EntryAt(i);

                var currentFileGroup = new SingleGameROMGroup();
                romGroupSet.Add(currentFileGroup);
                currentFileGroup.Add(rom);

                currentFileGroup.SortEntries();
            }

            return romGroupSet;
        }
        */

        // private void ProcessFilesInDirectory(string directory, SearchOption directoryOptions, int directoryIndex)
        private void ProcessFilesInDirectory(string directory, SearchOption directoryOptions)
        {
            var allFilesInDirectory = FileUtilities.GetFiles(directory, "*", directoryOptions);
            Parallel.ForEach(allFilesInDirectory, fastFileInfo =>
            // Parallel.For(0, allFilesInDirectory.Count, (int i) =>
            {
                // var fastFileInfo = allFilesInDirectory[i];
                var filePath = fastFileInfo.FullName;

                // ProcessFile(filePath, (directoryIndex * MaxAssumedEntriesPerDirectory) + i);
                ProcessFile(filePath);
            });
        }

        // private SingleGameROMGroupSet CreateSimilarityGroupedROMGroupSet(ROMGroup mainROMGroup)
        private SingleGameROMGroupSet CreateSimilarityGroupedROMGroupSet(ROMGroup mainROMGroup, bool requireROMsToBeInSameDirectory = true)
        {
            //var stopwatch = CreateStopWatch();
            //stopwatch.Start();
            var romGroupSet = new SingleGameROMGroupSet();

            var roms = mainROMGroup.Clone(); 
            roms.Sort(!requireROMsToBeInSameDirectory);

            var currentFileGroup = new SingleGameROMGroup();
            currentFileGroup.Add(roms.EntryAt(0));

            romGroupSet.Add(currentFileGroup);

            var settings = ProjectSettingsManager.MainSettings;
            var matchUsingGameListXMLName = settings.MatchUsingGameListXMLName;
            var allowedSimilarityValue = settings.AllowedSimilarityValue;

            var allRomEntriesCount = roms.TotalEntries;
            for (var i = 1; i < allRomEntriesCount; i++)
            {
                var prevROM = roms.EntryAt(i - 1);
                var currentROM = roms.EntryAt(i);

                if (prevROM == null || currentROM == null)
                {
                    throw new Exception("This should not happen.  Report it.");
                }

                var areROMsSame = false;

                // First make sure that these ROMs are in the same directory
                var romsInSameDirectory = string.Equals(prevROM.AbsoluteDirectory, currentROM.AbsoluteDirectory, StringComparison.OrdinalIgnoreCase);
                var compareROMs = !requireROMsToBeInSameDirectory || (requireROMsToBeInSameDirectory && romsInSameDirectory);
                // if (romsInSameDirectory)
                if (compareROMs)
                {
                    /****************************************************************************/
                    // If both entries have gamelist file entries, compare their names
                    if (matchUsingGameListXMLName)
                    {
                        var prevROMGameListFileEntry = prevROM.GameListFileEntry;
                        var currentROMGameListFileEntry = currentROM.GameListFileEntry;
                        if (prevROMGameListFileEntry != null && currentROMGameListFileEntry != null)
                        {
                            var prevROMGameListFileEntryName = prevROMGameListFileEntry.Name;
                            var currentROMGameListFileEntryName = currentROMGameListFileEntry.Name;
                            if (!string.IsNullOrEmpty(prevROMGameListFileEntryName) && !string.IsNullOrEmpty(currentROMGameListFileEntryName))
                            {
                                areROMsSame = string.Equals(prevROMGameListFileEntryName, currentROMGameListFileEntryName, StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                    /****************************************************************************/

                    if (!areROMsSame)
                    {
                        var prevFilename_compare = prevROM.ComparisonName;
                        var currentFilename_compare = currentROM.ComparisonName;

                        var similarity = dl.GetSimilarity(prevFilename_compare, currentFilename_compare);
                        // areROMsSame = similarity >= 0.9;
                        // areROMsSame = similarity >= AllowedSimilarityValue;
                        // areROMsSame = similarity >= OperationsOptions.Instance.AllowedSimilarityValue;
                        areROMsSame = similarity >= allowedSimilarityValue;
                        if (areROMsSame)
                        {
                            // If the name of the ROMs contain a number at the end, compare the number
                            var prevFilename_compare_numberAtTheEnd = Regex.Match(prevFilename_compare, @"\d+$").Value;
                            var currentFilename_compare_numberAtTheEnd = Regex.Match(currentFilename_compare, @"\d+$").Value;

                            if (prevFilename_compare_numberAtTheEnd != currentFilename_compare_numberAtTheEnd)
                            {
                                areROMsSame = false;
                            }
                        }
                    }
                }

                if (!areROMsSame)
                {
                    // Sort the entries of the current group before creating a new one
                    currentFileGroup.SortEntries();

                    currentFileGroup = new SingleGameROMGroup();
                    romGroupSet.Add(currentFileGroup);
                }

                currentFileGroup.Add(currentROM);
            }

            // Sort the entries of the last created group
            currentFileGroup.SortEntries();

            // StopShowAndResetStopWatchTime(stopwatch, "CreateSimilarityGroupedROMGroupSet()");

            return romGroupSet;
        }

        public bool ExportAllROMSListToFile(string filePath)
        {
            if (!romDirectoryAnalyzed)
            {
                return false;
            }

            return OutputFileWriter.Write(allGamesROMGroup, filePath);
        }

        public bool ExportFindDuplicatesOperationResults(string filePath)
        {
            // return OutputFileWriter.Write(, filePath);
            return false;
        }

        public bool ExportGroupByDirectoryOperationResults(string filePath)
        {
            return OutputFileWriter.Write(groupedByDirectoryROMGroupSet, filePath);
        }

        private RBGameListFile ReadRBGameFile(string romsPath)
        {
            var gameListFile = recalboxGameListReader.ReadGameListFile(romsPath);

            var log = gameListFile != null
                ? $"Recalbox GameList file found at {gameListFile.FilePath}"
                : $"Recalbox GameList file not found";

            Logger.Log(log);

            return gameListFile;
        }

        private void SaveROMAttributes(ROMEntry romEntry)
        {
            var romName = romEntry.Filename;

            // Extract the values of parentheses from the ROM filename
            var parenthesesMatches = parenthesesMatchRegex.Matches(romName);
            var parenthesesMatchesCount = parenthesesMatches.Count;
            for (var i = 0; i < parenthesesMatchesCount; i++)
            {
                var match = parenthesesMatches[i].Value;
                if (string.IsNullOrEmpty(match))
                {
                    continue;
                }

                var matchCompare = match.ToLowerInvariant();

                // ROM Country
                if (romCountries.TryGetValue(matchCompare.ToLowerInvariant(), out var romCountry))
                {
                    romEntry.Country = romCountry;
                    continue;
                }

                // ROM License
                if (romLicenses.TryGetValue(matchCompare.ToLowerInvariant(), out var romLicense))
                {
                    romEntry.License = romLicense;
                    continue;
                }

                // Standard Codes
                if (string.Equals(matchCompare, "!"))
                {
                    // romEntry.VerifiedGoodDump = true;
                    romEntry.AddCode(ROMStandardCodes.VerifiedGoodDump);
                    continue;
                }

                // if (string.Equals(matchCompare, "a"))
                if (alternativeROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.AlternateVersion = true;
                    romEntry.AddCode(ROMStandardCodes.AlternateVersion);
                    continue;
                }

                // if (string.Equals(matchCompare, "b") || badROMNameRegex.IsMatch(matchCompare))
                if (badROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.BadDump = true;
                    romEntry.AddCode(ROMStandardCodes.BadDump);
                    continue;
                }

                // if (string.Equals(matchCompare, "f"))
                if (fixedROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.CorrectedDump = true;
                    romEntry.AddCode(ROMStandardCodes.CorrectedDump);
                    continue;
                }

                // if (string.Equals(matchCompare, "h"))
                if (hackedROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.HackedROM = true;
                    romEntry.AddCode(ROMStandardCodes.HackedROM);
                    continue;
                }

                // if (string.Equals(matchCompare, "o"))
                if (overdumpROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.OverDump = true;
                    romEntry.AddCode(ROMStandardCodes.OverDump);
                    continue;
                }

                // if (string.Equals(matchCompare, "p") || pirateROMNameRegex.IsMatch(matchCompare))
                if (pirateROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.PiratedVersion = true;
                    romEntry.AddCode(ROMStandardCodes.PiratedVersion);
                    continue;
                }

                if (string.Equals(matchCompare, "!p"))
                {
                    // romEntry.DelayedDump = true;
                    romEntry.AddCode(ROMStandardCodes.DelayedDump);
                    continue;
                }

                // Universal Codes
                if (matchCompare.Contains("t+"))
                {
                    romEntry.AddCode(ROMUniversalCode.NewerTranslation);
                    continue;
                }

                if (matchCompare.Contains("t-"))
                {
                    romEntry.AddCode(ROMUniversalCode.OlderTranslation);
                    continue;
                }

                if (trackROMNameRegex.IsMatch(matchCompare))
                {
                    romEntry.AddCode(ROMUniversalCode.TrackSpecified);

                    // Extract the track value if possible
                    var valueMatch = trackMatchRegex.Matches(matchCompare);
                    if (valueMatch.Count > 0)
                    {
                        var revValue = valueMatch[0].Value;
                        romEntry.Track = revValue;
                    }

                    continue;
                }

                // TODO: Important: Keep the trainerROMNameRegex AFTER the Newer/OlderTranslation and track checks
                // if (string.Equals(matchCompare, "t"))
                if (trainerROMNameRegex.IsMatch(matchCompare))
                {
                    // romEntry.VersionWithTrainer = true;
                    romEntry.AddCode(ROMStandardCodes.VersionWithTrainer);
                    continue;
                }

                if (matchCompare.Contains("mapper"))
                {
                    romEntry.AddCode(ROMUniversalCode.NESMapper);
                    continue;
                }

                if (matchCompare.Contains("sachen"))
                {
                    romEntry.AddCode(ROMUniversalCode.NESSachen);
                    continue;
                }

                if (string.Equals(matchCompare, "menu"))
                {
                    romEntry.AddCode(ROMUniversalCode.MultiplayerMenu);
                    continue;
                }

                if (string.Equals(matchCompare, "vs"))
                {
                    romEntry.AddCode(ROMUniversalCode.NESVSVersion);
                    continue;
                }

                if (string.Equals(matchCompare, "aladdin"))
                {
                    romEntry.AddCode(ROMUniversalCode.NESAladdinCartridge);
                    continue;
                }

                if (multiLanguageROMNameRegex.IsMatch(matchCompare))
                {
                    romEntry.AddCode(ROMUniversalCode.Multilanguage);
                    continue;
                }

                if (string.Equals(matchCompare, "prototype"))
                {
                    romEntry.AddCode(ROMUniversalCode.Prototype);
                    continue;
                }

                if (string.Equals(matchCompare, "beta"))
                {
                    romEntry.AddCode(ROMUniversalCode.Beta);
                    continue;
                }

                if (string.Equals(matchCompare, "sample"))
                {
                    romEntry.AddCode(ROMUniversalCode.Sample);
                    continue;
                }

                if (revisionsROMNameRegex.IsMatch(matchCompare))
                {
                    romEntry.AddCode(ROMUniversalCode.RevisionSpecified);

                    // Extract the revision value if possible
                    var revisionValueMatch = revisionMatchRegex.Matches(matchCompare);
                    if (revisionValueMatch.Count > 0)
                    {
                        var revValue = revisionValueMatch[0].Value;
                        romEntry.Revision = revValue;
                    }

                    continue;
                }

                if (hackedMapperROMNameRegex.IsMatch(matchCompare))
                {
                    romEntry.AddCode(ROMUniversalCode.HackedMapper);
                    continue;
                }

                if (string.Equals(matchCompare, "time hack"))
                {
                    romEntry.AddCode(ROMUniversalCode.TimeHack);
                    continue;
                }

                if (string.Equals(matchCompare, "neo demiforce hack"))
                {
                    romEntry.AddCode(ROMUniversalCode.NeoDemiforceHack);
                    continue;
                }

                var breakpoint = true;
            }
        }

        /// <summary>
        /// TODO: MOVE OUT OF HERE
        /// https://stackoverflow.com/a/45756981
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another 
        /// specified string according the type of search to use for the specified string.
        /// </summary>
        /// <param name="str">The string performing the replace method.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string replace all occurrences of <paramref name="oldValue"/>. 
        /// If value is equal to <c>null</c>, than all occurrences of <paramref name="oldValue"/> will be removed from the <paramref name="str"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of <paramref name="oldValue"/> are replaced with <paramref name="newValue"/>. 
        /// If <paramref name="oldValue"/> is not found in the current instance, the method returns the current instance unchanged.</returns>
        public static string Replace(string str, string oldValue, string @newValue, StringComparison comparisonType)
        {
            /*
            // Check inputs.
            if (str == null)
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                // Same as original .NET C# string.Replace behavior.
                return str;
            }

            if (oldValue == null)
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (oldValue.Length == 0)
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentException("String cannot be of zero length.");
            }
            */

            //if (oldValue.Equals(newValue, comparisonType))
            //{
            //This condition has no sense
            //It will prevent method from replacesing: "Example", "ExAmPlE", "EXAMPLE" to "example"
            //return str;
            //}

            // Prepare string builder for storing the processed string.
            // Note: StringBuilder has a better performance than String by 30-40%.
            StringBuilder resultStringBuilder = new StringBuilder(str.Length);

            // Analyze the replacement: replace or remove.
            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(@newValue);

            // Replace all values.
            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;
            while ((foundAt = str.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {
                // Append all characters until the found replacement.
                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (!isNothingToAppend)
                {
                    resultStringBuilder.Append(str, startSearchFromIndex, @charsUntilReplacment);
                }

                // Process the replacement.
                if (!isReplacementNullOrEmpty)
                {
                    resultStringBuilder.Append(@newValue);
                }

                // Prepare start index for the next search.
                // This needed to prevent infinite loop, otherwise method always start search 
                // from the start of the string. For example: if an oldValue == "EXAMPLE", newValue == "example"
                // and comparisonType == "any ignore case" will conquer to replacing:
                // "EXAMPLE" to "example" to "example" to "example" … infinite loop.
                startSearchFromIndex = foundAt + oldValue.Length;
                if (startSearchFromIndex == str.Length)
                {
                    // It is end of the input string: no more space for the next search.
                    // The input string ends with a value that has already been replaced. 
                    // Therefore, the string builder with the result is complete and no further action is required.
                    return resultStringBuilder.ToString();
                }
            }

            // Append the last part to the result.
            int @charsUntilStringEnd = str.Length - startSearchFromIndex;
            resultStringBuilder.Append(str, startSearchFromIndex, @charsUntilStringEnd);

            return resultStringBuilder.ToString();
        }

        /// <summary>
        /// TODO: MOVE OUT OF HERE
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        private static void RenameDictionaryKey<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
    }
}
