// #define LOG_IMAGE_LOADING

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using DIOUtils;
using Frontend;

// using Microsoft.DirectX.AudioVideoPlayback;

namespace DROMsM.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// TODO: The problem with this is that order is important at the moment.
        /// TODO: This is because if you have "v1.1" before "(v1.1)", then "v1.1" will be blanked while leaving the brackets there.

        /*
        /// TODO: I think it's just best to remove stuff with (brackets) and [brackets] and be done with it
        /// </summary>
        private static readonly string[] RomSymbols =
        {
            "[!]",
            "(U)",
            "[U]",
            "[J]",
            "(J)",
            "(vs)",
            "[!p]",
            "[b1]",
            "(ue)",
            "(menu)",
            "[p1]",
            "(sachen)",
            "(sachen-english)",
            "(sachen-usa)",
            "(sachen-jap)",
            "(sachen-hes)",
            "(sachen-ntsc)",
            "(unl)",
            "(v1.0)",
            "(Sample)",
            "(PRG1)",
            "(w)",
            "(rev 3)",
            "(rev x)",
            "(REVA)",
            "[a1]",
            "V1.0",
            "(V1.1)",
            "V1.1",
            "(V1.3)",
            "(V1.4)",
            "(V6.0)",
            "(Ver 6.0)",
            "(Prototype)",
            "(prg 0)",
            "(prg 1)",
            "(prg0)",
            "(prg2)",
            "(REV1.1)",
            "(REV1)",
            "[hFFE]",
            "(Taito)",
            "(UBI Soft)",
            "(Tengen)",
            "(Aladdin)",
            "(CRTJ)",
            "(EDS)",
            "V0.5",
            "[h1]",
            "(Time Hack)",
            "(Neo Demiforce Hack)",
            "(RE)",
            "(1993 Version)",
            "(M4)",
            "(Mapper 182)",
            "(Mapper 4)",
            "[p2]",
            "(ju)",
            "(v4.0)",
            "[b3]",
            "(rev b)",
            "(Garbled)",
            "(no cart present)",
            "(NINA03-USA)",
            "(Sachen+NINA03)",
            "(Sachen-Joy Van)",
            "[o1]",
            "(as)",
            "[T-Eng]",
            "[T+Eng]",
            "(Namco)",
            "(Rev 4)",
            "(Japan)",
            "(USA)",
            "(SADS)",
            "(Rev 2)",
            "(FABT)",
            "(Rev 6)",
            "(Alt)",
            "(Unlicensed)",
            "(Rev 1)",
            "(Rev 5)",
            "(Rev 9)",
            "(Rev 10)",
            "(Rev 7)",
            "(Reprint)",
        };
        */

        private readonly MainManager mainManager;

        // private Video currentRomVideo;

        private bool treeviewsCurrentlyLinked;

        // private OperationsOptions options;

        public MainForm()
        {
            try
            {
                InitializeComponent();

                // Start the program maximized
                WindowState = FormWindowState.Maximized;

                /****************/
                /*
                MessageBoxOperations.ShowError("Dont forget to remove this test", "asd");
                var mameNotWorkingFile = @"D:\dromsmanagerdirs\mameexported_notworking.txt";
                var mameExportFileReader = new MAMEExportFileReader();
                var mameFile = mameExportFileReader.ParseFile(mameNotWorkingFile);
                */
                /*****************/

                // Set the Main Form name
                var windowTitle = $"DROMsM v{ApplicationVersion.ProductVersion} - Manage your ROM collection";
#if DEBUG
                windowTitle = $"DEBUG MODE -- {windowTitle} -- DEBUG MODE";
#endif
                this.Text = windowTitle;

                Logger.SetListView(logsListView);

                mainManager = new MainManager(this);
                // options = mainManager.options;

                exportAllROMsListToolStripMenuItem.Enabled = false;

                ClearTotalsInfo();
                DisableSubOperationsButtons();
                ToggleOperationsButtons(false);

                ApplyInitialSavedProjectSettings();
            }
            catch (Exception ex)
            {
                MessageBoxOperations.ShowException(ex);

                QuitApplication();
            }
        }

        private void ApplyInitialSavedProjectSettings()
        {
            var mainSettings = ProjectSettingsManager.MainSettings;
            romsDirectoryTextBox.Text = mainSettings.ROMsDirectory;
        }

        private void findSuggestedButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Logger.Log("Art of Fighting - Ryuuko no Ken Gaiden ~ The Path of the Warrior - Art of Fighting 3 (Japan, USA) (En,Ja,Es,Pt)\\Art of Fighting - Ryuuko no Ken Gaiden ~ The Path of the Warrior - Art of Fighting 3 (Japan, USA) (En,Ja,Es,Pt)\\Art of Fighting - Ryuuko no Ken Gaiden ~ The Path of the Warrior - Art of Fighting 3 (Japan, USA) (En,Ja,Es,Pt)\\last bit");

                //var romsPath = romsDirectoryTextBox.Text;
                //if (string.IsNullOrEmpty(romsPath))
                //{
                //    return;
                //}

                mainManager.ExecuteFindDuplicateROMsOperation();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void splitIntoDirectoriesButton_Click(object sender, EventArgs e)
        {
            try
            {
                //var romsPath = romsDirectoryTextBox.Text;
                //if (string.IsNullOrEmpty(romsPath))
                //{
                //    return;
                //}

                mainManager.ExecuteGroupROMsOperation();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void moveROMsToRootOperationButton_Click(object sender, EventArgs e)
        {
            try
            {
                //var romsPath = romsDirectoryTextBox.Text;
                //if (string.IsNullOrEmpty(romsPath))
                //{
                //    return;
                //}

                mainManager.ExecuteMoveAllROMsToRootOperation();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }

        private void applyDuplicatesResultsButton_Click(object sender, EventArgs e)
        {
            if (!MessageBoxOperations.ShowConfirmation("Are you sure you want to remove duplicate ROMs?", "Removing duplicate ROMs"))
            {
                return;
            }

            var removedROMsGroup = mainManager.ExecuteRemoveDuplicateDOMsSubOperation();
            if (removedROMsGroup.TotalEntries > 0)
            {
                var removedROMsCount = removedROMsGroup.TotalEntries;
                MessageBoxOperations.ShowInformation($"Successfully moved {removedROMsCount} ROM{(removedROMsCount > 1 ? "s" : string.Empty)} to the Duplicates directory", "ROMS removed");
            }
        }

        private void applySplitResultsButton_Click(object sender, EventArgs e)
        {
            if (!MessageBoxOperations.ShowConfirmation("Are you sure you want to group the ROMs into their own directory?", "Moving ROMs to their own directory"))
            {
                return;
            }

            var success = mainManager.ExecuteGroupIntoDirectoriesSubOperation();
            if (!success)
            {
                MessageBoxOperations.ShowError("Unable to apply split", "Error applying split");
                return;
            }

            MessageBoxOperations.ShowInformation("Successfully split the ROMs to new directories", "ROMS split");
        }

        private void applyMoveAllROMsToRootButton_Click(object sender, EventArgs e)
        {
            if (!MessageBoxOperations.ShowConfirmation("Are you sure you want to move all the ROMs to the root directory?", "Moving ROMs to the root directory"))
            {
                return;
            }

            var success = mainManager.ExecuteMoveROMsToRootSubOperation();
            if (!success)
            {
                MessageBoxOperations.ShowError("Unable to move all ROMs to the root directory", "Error moving ROMs");
                return;
            }

            MessageBoxOperations.ShowInformation("Successfully moved all ROMs to the root directory", "ROMS moved");
        }

        public void PopulateLeftTreeView(SingleGameROMGroupSet romSet, bool duplicatesOnly, bool focusTreeView)
        {
            PopulateResultsTreeView(leftTreeView, romSet, duplicatesOnly, focusTreeView);
        }

        public void PopulateRightTreeView(SingleGameROMGroupSet romSet, bool duplicatesOnly, bool focusTreeView)
        {
            PopulateResultsTreeView(rightTreeView, romSet, duplicatesOnly, focusTreeView);
        }

        private void PopulateTreeView(TreeView treeView, ROMGroup romGroup, TreeViewROMDisplayNameType displayNameType)
        {
            var resultsTreeViewNodes = treeView.Nodes;
            resultsTreeViewNodes.Clear();

            var romGroupTotalEntries = romGroup.TotalEntries;
            for (var i = 0; i < romGroupTotalEntries; i++)
            {
                var romEntry = romGroup.EntryAt(i);
                var romNodeTitle = romEntry.GetDisplayName(displayNameType);
                var romNodeFullTitle = $"{romNodeTitle.Trim()}";
                var treeNode = new TreeNode(romNodeFullTitle) {Tag = romEntry};

                resultsTreeViewNodes.Add(treeNode);
            }

            treeView.Focus();

            // Select the first node
            if (romGroupTotalEntries > 0)
            {
                var firstNode = resultsTreeViewNodes[0];
                treeView.TopNode = firstNode;
                treeView.SelectedNode = firstNode;
            }
        }

        private void PopulateTreeView(TreeView treeView, RomDirectory romDirectory, TreeViewROMDirectoryDisplayNameType displayNameType)
        {
            var resultsTreeViewNodes = treeView.Nodes;
            resultsTreeViewNodes.Clear();

            var subDirectories = romDirectory.SubDirectories;
            var subDirectoriesTotal = subDirectories.Count;
            for (var i = 0; i < subDirectoriesTotal; i++)
            {
                var subDirectory = subDirectories[i];
                var nodeTitle = subDirectory.GetDisplayName(displayNameType);
                var nodeFullTitle = $"{nodeTitle.Trim()}";
                var treeNode = new TreeNode(nodeFullTitle) {Tag = subDirectory};

                resultsTreeViewNodes.Add(treeNode);
            }

            treeView.Focus();

            // Select the first node
            if (subDirectoriesTotal > 0)
            {
                var firstNode = resultsTreeViewNodes[0];
                treeView.TopNode = firstNode;
                treeView.SelectedNode = firstNode;
            }
        }

        private void PopulateResultsTreeView(TreeView treeView, SingleGameROMGroupSet romGroupSet, bool duplicatesOnly, bool focusTreeView)
        {
            var resultsTreeViewNodes = treeView.Nodes;

            resultsTreeViewNodes.Clear();

            var romGroups = romGroupSet.SingleGameROMGroups;
            var romGroupsCount = romGroups.Count;
            for (var i = 0; i < romGroupsCount; i++)
            {
                var singleGameROMGroup = romGroups[i];

                if (duplicatesOnly && !singleGameROMGroup.ContainsDuplicates)
                {
                    continue;
                }

                var romGroupCount = singleGameROMGroup.TotalEntries;

                var childNodes = new TreeNode[romGroupCount];
                for (var j = 0; j < romGroupCount; j++)
                {
                    var singleROMEntry = singleGameROMGroup.EntryAt(j);
                    // var filename = singleROMEntry.Filename;
                    var filename = singleROMEntry.FilenameWithExtension;
                    var childNode = new TreeNode(filename)
                    {
                        Tag = singleROMEntry
                    };

                    childNodes[j] = childNode;
                }

                // var romNodeTitle = romGroupEntries[0].DisplayName;
                var romNodeTitle = singleGameROMGroup.ToString();

                // var romNodeFullTitle = $"{romNodeTitle.Trim()} {(resultsOptions_showTotals ? $"(Total: {romGroupCount})" : string.Empty)}";
                var romNodeFullTitle = $"{romNodeTitle.Trim()}";

                // var treeNode = new TreeNode(romNodeFullTitle, childNodes) {Tag = romGroup};
                var treeNode = new TreeNode(romNodeFullTitle) {Tag = singleGameROMGroup};
                treeNode.Nodes.AddRange(childNodes);

                resultsTreeViewNodes.Add(treeNode);
            }

            if (focusTreeView)
            {
                treeView.Focus();
            }

            // if (romGroupsCount > 0)
            if (resultsTreeViewNodes.Count > 0)
            {
                var firstNode = resultsTreeViewNodes[0];
                treeView.TopNode = firstNode;
                treeView.SelectedNode = firstNode;
            }

            //// var allROMEntriesCount = mainManager.allGamesROMGroup.TotalEntries;
            //var allROMEntriesCount = mainManager.UnprocessedTotalEntries;
            // totalSingleRomsLabel.Text = romGroupsCount.ToString();
            // totalAllRomsLabel.Text = allROMEntriesCount.ToString();
            /*
            totalDuplicateRomsLabel.Text = (allROMEntriesCount - romGroups.Count).ToString();
            */
        }

        private void ClearROMDetailsPanel()
        {
            romFilePath.Text = string.Empty;
            romFilename.Text = string.Empty;
            romGoodDump.Checked = false;
            romAlternateVersion.Checked = false;
            romBadDump.Checked = false;
            romFixedDump.Checked = false;
            romHackedDump.Checked = false;
            romOverdump.Checked = false;
            romPirated.Checked = false;
            romHasTrainer.Checked = false;
            romDelayed.Checked = false;

            romRegion.Text = string.Empty;
            romLicense.Text = string.Empty;
            romRevision.Text = string.Empty;
            romTrack.Text = string.Empty;
            romUniversalCodes.Items.Clear();

            romScore.Text = string.Empty;

            romName.Text = string.Empty;

            romPictureBox.Image = null;

            /*
            if (currentRomVideo != null)
            {
                currentRomVideo.Dispose();
                currentRomVideo = null;
            }
            */
        }

        private void HandleTreeViewNodeSelected(TreeNode selectedNode)
        {
            if (selectedNode == null)
            {
                return;
            }

            try
            {
                // Clear all the fields before we refill
                ClearROMDetailsPanel();

                var selectedNodeTag = selectedNode.Tag;

                // var singleROMEntry = selectedNodeTag as ROMEntry 
                var singleROMEntry = selectedNodeTag as ROMEntry;
                // if (selectedNodeTag is ROMEntry singleROMEntry)
                if (singleROMEntry != null && !singleROMEntry.IsDummyROM)
                {
                    romFilePath.Text = singleROMEntry.AbsoluteFilePath;
                    romFilename.Text = singleROMEntry.FilenameWithExtension;
                    romGoodDump.Checked = singleROMEntry.HasCode(ROMStandardCodes.VerifiedGoodDump);
                    romAlternateVersion.Checked = singleROMEntry.HasCode(ROMStandardCodes.AlternateVersion);
                    romBadDump.Checked = singleROMEntry.HasCode(ROMStandardCodes.BadDump);
                    romFixedDump.Checked = singleROMEntry.HasCode(ROMStandardCodes.CorrectedDump);
                    romHackedDump.Checked = singleROMEntry.HasCode(ROMStandardCodes.HackedROM);
                    romOverdump.Checked = singleROMEntry.HasCode(ROMStandardCodes.OverDump);
                    romPirated.Checked = singleROMEntry.HasCode(ROMStandardCodes.PiratedVersion);
                    romHasTrainer.Checked = singleROMEntry.HasCode(ROMStandardCodes.VersionWithTrainer);
                    romDelayed.Checked = singleROMEntry.HasCode(ROMStandardCodes.DelayedDump);

                    romRegion.Text = singleROMEntry.Country != ROMCountry.None
                        ? singleROMEntry.Country.ToString()
                        : string.Empty;
                    romLicense.Text = singleROMEntry.License != ROMLicense.None
                        ? singleROMEntry.License.ToString()
                        : string.Empty;
                    romRevision.Text = singleROMEntry.Revision;
                    romTrack.Text = singleROMEntry.Track;

                    romUniversalCodes.Items.Clear();

                    var singleRomEntryUniversalCodes = singleROMEntry.UniversalCodes;
                    foreach (var romUniversalCode in singleRomEntryUniversalCodes)
                    {
                        romUniversalCodes.Items.Add(romUniversalCode.ToString());
                    }

                    romScore.Text = singleROMEntry.ROMUsageScore.ToString(CultureInfo.InvariantCulture);

                    if (mainManager.UsingRBGameFile)
                    {
                        var gameListFileEntry = singleROMEntry.GameListFileEntry;
                        if (gameListFileEntry != null)
                        {
                            var rbGameListFile = mainManager.rbGameListFile;
                            var rbGameListFileFileBaseDirectoryPath = rbGameListFile.FileBaseDirectoryPath;
                            var imageFilename = gameListFileEntry.Image;
                            var hasImage = !string.IsNullOrEmpty(imageFilename);
                            if (hasImage)
                            {
                                var imageFilePath = FileUtilities.CombinePath(rbGameListFileFileBaseDirectoryPath, gameListFileEntry.Image);
                                if (FileUtilities.FileExists(imageFilePath))
                                {
                                    romPictureBox.LoadAsync(imageFilePath);

#if LOG_IMAGE_LOADING
                                    Logger.Log($"Loading image from {imageFilePath}");
#endif
                                }
#if LOG_IMAGE_LOADING
                                else
                                {
                                    Logger.Log($"Image not found at {imageFilePath}");
                                }
#endif
                            }
#if LOG_IMAGE_LOADING
                            else
                            {
                                Logger.Log($"Game {singleROMEntry.DisplayName} does not have image");
                            }
#endif

                            romName.Text = gameListFileEntry.Name;

                            /*
                            var videoFilePath = Path.Combine(rbGameListFileFileBaseDirectoryPath, gameListFileEntry.Video);
                            if (File.Exists(videoFilePath))
                            {
                                // romVideo.URL = videoFilePath;
    
                                int height = romVideoPanel.Height;
                                int width = romVideoPanel.Width;
    
                                var video = new Video(videoFilePath);
                                video.Owner = romVideoPanel;
                                romVideoPanel.Width = width;
                                romVideoPanel.Height = height;
                                // play the first frame of the video so we can identify it  
                                video.Play();
                                video.Pause();
                            }
                            */
                        }
                        /*
                        else
                        {
                            Logger.Log($"No gamelist.xml entry found for {singleROMEntry.DisplayName}");
                        }
                        */
                    }
                    //else
                    //{
                    //    Logger.Log($"Not using a gamelist.xml file");
                    //}
                }

                if (selectedNodeTag is SingleGameROMGroup romGroup)
                {
                    romFilePath.Text = romGroup.FindSuggestedROM().DisplayName;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void romsDirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            romsFolderBrowserDialog.SelectedPath = romsDirectoryTextBox.Text;
            var result = romsFolderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var selectedPath = romsFolderBrowserDialog.SelectedPath;
                romsDirectoryTextBox.Text = selectedPath;
            }
        }

        public void OnFinishedAnalyzingROMDirectory(MultipleGameROMGroup allGamesRomGroup)
        {
            // Clear the left and right treeviews since we're analyzing a directory
            ClearLeftTreeView();
            ClearRightTreeView();

            ClearTotalsInfo();
            DisableSubOperationsButtons();
            ToggleOperationsButtons(true);
            treeviewsCurrentlyLinked = false;

            exportAllROMsListToolStripMenuItem.Enabled = true;

            PopulateTreeView(unprocessedROMsTreeview, allGamesRomGroup, TreeViewROMDisplayNameType.RelativeFilePath);

            var multipleGameROMGroupTotalEntries = allGamesRomGroup.TotalEntries;
            totalAllRomsLabel.Text = multipleGameROMGroupTotalEntries.ToString();
        }

        public void OnFinishedAnalyzingTopLevelDirectoriesOperation(RomDirectory rootDirectory)
        {
            ClearLeftTreeView();
            ClearRightTreeView();

            ClearTotalsInfo();
            DisableSubOperationsButtons();
            ToggleOperationsButtons(true);
            treeviewsCurrentlyLinked = false;

            removeEmptyTopLevelDirectoriesSubOperationButton.Enabled = true;

            PopulateTreeView(rightTreeView, rootDirectory, TreeViewROMDirectoryDisplayNameType.FullPath);
        }

        public void OnFinishedFindingDuplicateROMsOperation(SingleGameROMGroupSet singlesROMGroupSet, MultipleGameROMGroup duplicateROMs)
        {
            ClearTotalsInfo();
            DisableSubOperationsButtons();
            treeviewsCurrentlyLinked = true;

            applyDuplicatesResultsButton.Enabled = duplicateROMs.TotalEntries > 0;
            findDuplicatesResultsToolStripMenuItem.Enabled = true;

            PopulateRightTreeView(singlesROMGroupSet, true, true);

            // var totalSingles = singlesROMGroupSet.SingleGameROMGroups.Count - duplicateROMs.TotalEntries;
            // var totalSingles = singlesROMGroupSet.TotalROMGroups - duplicateROMs.TotalEntries;
            var totalSingles = singlesROMGroupSet.TotalROMGroups;
            totalSingleRomsLabel.Text = totalSingles.ToString();
            totalDuplicateRomsLabel.Text = duplicateROMs.TotalEntries.ToString();

            AfterFinishingOperation();
        }

        public void OnFinishedProcessingIntoDirectoriesOperation(SingleGameROMGroupSet splitIntoDirectoriesROMGroupSet)
        {
            ClearTotalsInfo();
            DisableSubOperationsButtons();
            treeviewsCurrentlyLinked = false;

            applySplitResultsButton.Enabled = true;
            splitIntoDirectoriesResultsToolStripMenuItem.Enabled = true;

            ClearLeftTreeView();

            PopulateRightTreeView(splitIntoDirectoriesROMGroupSet, false, true);

            var total = splitIntoDirectoriesROMGroupSet.SingleGameROMGroups.Count;
            totalSingleRomsLabel.Text = total.ToString();

            AfterFinishingOperation();
        }

        // public void OnFinishedMovingAllROMsToRootOperation(SingleGameROMGroupSet movedToRootROMGroupSet)
        public void OnFinishedMovingAllROMsToRootOperation(MultipleGameROMGroup movedToRootROMGroup)
        {
            ClearTotalsInfo();
            DisableSubOperationsButtons();
            treeviewsCurrentlyLinked = false;

            /*
            applySplitResultsButton.Enabled = true;
            splitIntoDirectoriesResultsToolStripMenuItem.Enabled = true;
            */
            applyMoveAllROMsToRootButton.Enabled = true;

            ClearLeftTreeView();

            PopulateTreeView(rightTreeView, movedToRootROMGroup, TreeViewROMDisplayNameType.FilenameWithExtension);

            /*
            var total = splitIntoDirectoriesROMGroupSet.SingleGameROMGroups.Count;
            totalSingleRomsLabel.Text = total.ToString();
            */

            Logger.Log($"Found {movedToRootROMGroup.TotalEntries} matching roms");

            AfterFinishingOperation();
        }

        public void OnFinishedCombineMultipleBinsIntoOneOperation(MultipleGameROMGroup processedROMs)
        {
            ClearTotalsInfo();
            DisableSubOperationsButtons();
            treeviewsCurrentlyLinked = false;

            applyCombineMultipleBinFilesToOneButton.Enabled = true;

            ClearLeftTreeView();

            PopulateTreeView(rightTreeView, processedROMs, TreeViewROMDisplayNameType.RelativeFilePath);

            Logger.Log($"Found {processedROMs.TotalEntries} matching roms");

            AfterFinishingOperation();
        }

        public void OnFinishedRemoveROMsFromMAMEExportFileOperation(string mameExportFilePath, MultipleGameROMGroup processedROMs)
        {
            ClearTotalsInfo();
            DisableSubOperationsButtons();
            treeviewsCurrentlyLinked = false;

            if (processedROMs.TotalEntries > 0)
            {
                removeROMsFromMAMEFileSubOperationButton.Enabled = true;
            }

            ClearLeftTreeView();

            PopulateTreeView(rightTreeView, processedROMs, TreeViewROMDisplayNameType.RelativeFilePath);

            Logger.Log($"Found {processedROMs.TotalEntries} matching MAME roms from {mameExportFilePath}");

            AfterFinishingOperation();
        }

        void AfterFinishingOperation()
        {
            var mainSettings = ProjectSettingsManager.MainSettings;
            if (mainSettings.AutoExpandTreeViewsAfterOperations)
            {
                ExpandLeftAndRightTreeViews();
            }
        }

        private void ClearTotalsInfo()
        {
            totalDuplicateRomsLabel.Text = "-";
            totalSingleRomsLabel.Text = "-";
            // totalAllRomsLabel.Text = "-";
        }

        private void DisableSubOperationsButtons()
        {
            applyDuplicatesResultsButton.Enabled = false;
            applySplitResultsButton.Enabled = false;
            applyMoveAllROMsToRootButton.Enabled = false;
            removeEmptyTopLevelDirectoriesSubOperationButton.Enabled = false;
            applyCombineMultipleBinFilesToOneButton.Enabled = false;
            removeROMsFromMAMEFileSubOperationButton.Enabled = false;

            findDuplicatesResultsToolStripMenuItem.Enabled = false;
            splitIntoDirectoriesResultsToolStripMenuItem.Enabled = false;
        }

        private void ToggleOperationsButtons(bool enabled)
        {
            findDuplicatesButton.Enabled = enabled;
            splitIntoDirectoriesButton.Enabled = enabled;
            moveROMsToRootOperationButton.Enabled = enabled;
            removeTopLevelDirectoriesOperationButton.Enabled = enabled;
            combineMultipleBinFilesToOneOperationButton.Enabled = enabled;
            removeROMsFromMAMEFileOperationButton.Enabled = enabled;
        }

        private void analyzeDirectoryButton_Click(object sender, EventArgs e)
        {
            var romsDirectory = romsDirectoryTextBox.Text;
            if (string.IsNullOrEmpty(romsDirectory))
            {
                return;
            }

            mainManager.AnalyzeROMsDirectory(romsDirectory);
        }

        private void romsDirectoryTextBox_Leave(object sender, EventArgs e)
        {
            var mainSettings = ProjectSettingsManager.MainSettings;
            mainSettings.ROMsDirectory = romsDirectoryTextBox.Text;
            ProjectSettingsManager.UpdateProgramSettings(ProgramSettingsType.Main);
        }

        //private void ShowExceptionError(Exception ex)
        //{
        //    var errorMessage = $"{ex.Message}\r\n\r\n{ex.StackTrace}";
        //    var innerException = ex.InnerException;
        //    if (innerException != null)
        //    {
        //        errorMessage += $"\r\n\r\n{innerException.Message}\r\n\r\n{innerException.StackTrace}";
        //    }

        //    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

#region Boilerplate
        private void ClearLeftTreeView()
        {
            ClearTreeView(leftTreeView);
        }

        private void ClearRightTreeView()
        {
            ClearTreeView(rightTreeView);
        }

        private void ClearTreeView(TreeView treeView)
        {
            treeView.Nodes.Clear();
        }
        private void expandAllButton_Click(object sender, EventArgs e)
        {
            ExpandLeftAndRightTreeViews();
        }

        private void collapseAllButton_Click(object sender, EventArgs e)
        {
            CollapseLeftAndRightTreeViews();
        }
        private void ExpandOtherTreeViewNode(TreeNode thisTreeNode, TreeView otherTreeView)
        {
            var nodeIndex = thisTreeNode.Index;
            if (otherTreeView.Nodes.Count <= nodeIndex)
            {
                return;
            }

            otherTreeView.Nodes[nodeIndex].Expand();
        }

        private void CollapseOtherTreeViewNode(TreeNode thisTreeNode, TreeView otherTreeView)
        {
            var nodeIndex = thisTreeNode.Index;
            if (otherTreeView.Nodes.Count <= nodeIndex)
            {
                return;
            }

            otherTreeView.Nodes[nodeIndex].Collapse();
        }

        private void originalROMs_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            CollapseOtherTreeViewNode(e.Node, rightTreeView);
        }

        private void originalROMs_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ExpandOtherTreeViewNode(e.Node, rightTreeView);
        }

        private void processedROMsTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            CollapseOtherTreeViewNode(e.Node, leftTreeView);
        }

        private void processedROMsTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ExpandOtherTreeViewNode(e.Node, leftTreeView);
        }

        private void unprocessedROMsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            HandleTreeViewNodeSelected(selectedNode);
        }

        private void originalROMsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            HandleTreeViewNodeSelected(selectedNode);
        }

        private void processedROMsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            HandleTreeViewNodeSelected(selectedNode);
        }

        private void leftTreeView_ScrollV(object sender, devio.Windows.Controls.ScrollEventArgs e)
        {
            if (treeviewsCurrentlyLinked)
            {
                rightTreeView.ScrollToPositionV(e.ScrollInfo);
            }
        }

        private void rightTreeView_ScrollV(object sender, devio.Windows.Controls.ScrollEventArgs e)
        {
            if (treeviewsCurrentlyLinked)
            {
                leftTreeView.ScrollToPositionV(e.ScrollInfo);
            }
        }

        private void ExpandLeftAndRightTreeViews()
        {
            // Since the left and right TreeView are linked, 
            // we have to expand the right tree manually if the left TreeView is not populated
            if (leftTreeView.Nodes.Count > 0)
            {
                leftTreeView.ExpandAll();
            }
            else
            {
                rightTreeView.ExpandAll();
            }
        }

        private void CollapseLeftAndRightTreeViews()
        {
            // Since the left and right TreeView are linked, 
            // we have to collapse the right tree manually if the left TreeView is not populated
            if (leftTreeView.Nodes.Count > 0)
            {
                leftTreeView.CollapseAll();
            }
            else
            {
                rightTreeView.CollapseAll();
            }
        }
#endregion

        //private void allowedSimilarityValueTextBox_Leave(object sender, EventArgs e)
        //{
        //    var allowedSimilarityValueTextBoxText = allowedSimilarityValueTextbox.Text;
        //    if (!double.TryParse(allowedSimilarityValueTextbox.Text, out var allowedSimilarityValue))
        //    {
        //        Logger.Log($"Unable to parse {allowedSimilarityValueTextBoxText} as an Allowed Similarity Value.  Numbers only.");

        //        // allowedSimilarityValueTextbox.Text = mainManager.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
        //        allowedSimilarityValueTextbox.Text = OperationsOptions.Instance.AllowedSimilarityValue.ToString(CultureInfo.InvariantCulture);
        //        return;
        //    }

        //    allowedSimilarityValue = DoubleExtensions.Clamp(allowedSimilarityValue, 0, 1);
        //    allowedSimilarityValueTextbox.Text = allowedSimilarityValue.ToString(CultureInfo.InvariantCulture);

        //    // mainManager.AllowedSimilarityValue = allowedSimilarityValue;
        //    OperationsOptions.Instance.AllowedSimilarityValue = allowedSimilarityValue;
        //    Logger.Log($"Updating the Allowed Similarity Value to {allowedSimilarityValueTextBoxText}.");
        //}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuitApplication();
        }

        private void QuitApplication()
        {
            Application.Exit();
        }

        private void exportAllROMsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = FormFileOperations.ShowSaveFileDialog_Text();
            ExportToFile(mainManager.ExportAllROMSListToFile, filename);
        }

        private void findDuplicatesResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = FormFileOperations.ShowSaveFileDialog_Text();
            ExportToFile(mainManager.ExportFindDuplicatesOperationResults, filename);
        }

        private void splitIntoDirectoriesResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = FormFileOperations.ShowSaveFileDialog_Text();
            ExportToFile(mainManager.ExportGroupByDirectoryOperationResults, filename);
        }

        private void ExportToFile(Func<string, bool> action, string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            var fileSaved = action(filename);
            if (!fileSaved)
            {
                return;
            }

            if (!FileUtilities.FileExists(filename))
            {
                // MessageBox.Show($"Successfully exported to {filename}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Process.Start(filename);
            }
        }

        private void removeTopLevelDirectoriesOperationButton_Click(object sender, EventArgs e)
        {
            mainManager.ExecuteRemoveEmptyTopLevelDirectoriesOperation();
        }

        private void removeEmptyTopLevelDirectoriesSubOperationButton_Click(object sender, EventArgs e)
        {
            if (!MessageBoxOperations.ShowConfirmation("Are you sure you want to delete all empty top-level directories in ROMs directory?", "Deleting empty top-level directories in the root directory"))
            {
                return;
            }

            var operationRan = mainManager.ExecuteRemoveEmptyTopLevelDirectoriesSubOperation(out int directoriesDeleted, out int directoriesNotDeleted);
            if (!operationRan)
            {
                return;
            }

            MessageBoxOperations.ShowInformation("Directories removed: {directoriesDeleted}.  Directories not removed: {directoriesNotDeleted}", "Directories removed");
        }

        private void combineMultipleBinFilesToOneOperationButton_Click(object sender, EventArgs e)
        {
            mainManager.ExecuteConvertMultipleBinsToOneOperation();
        }

        private void applyCombineMultipleBinFilesToOneButton_Click(object sender, EventArgs e)
        {
            if (!MessageBoxOperations.ShowConfirmation("Are you sure you want to convert multiple bin files into one?", "Converting multiple bin files into one"))
            {
                return;
            }

            var processedROMsGroup = mainManager.ExecuteConvertMultipleBinsToOneSubOperation();
            if (processedROMsGroup == null)
            {
                MessageBoxOperations.ShowError("Unable to convert the bin files", "Error");
                return;
            }

            MessageBoxOperations.ShowInformation($"Successfully combined {processedROMsGroup.TotalEntries} ROMs", "Finished combining bin files");
        }

        private void removeROMsFromMAMEFileOperationButton_Click(object sender, EventArgs e)
        {
            var mameExportFilePath = FormFileOperations.ShowOpenFileDialog_TextFiles();
            if (string.IsNullOrEmpty(mameExportFilePath))
            {
                return;
            }

            mainManager.ExecuteRemoveROMsFromMameFileOperation(mameExportFilePath);
        }

        private void removeROMsFromMAMEFileSubOperationButton_Click(object sender, EventArgs e)
        {
            if (!MessageBoxOperations.ShowConfirmation("Are you sure you want to remove the matching MAME ROM files?", "Removing matching MAME files"))
            {
                return;
            }

            var result = mainManager.ExecuteRemoveROMsFromMAMEExportFileSubOperation(out var outputDirectory);
            if (!result)
            {
                MessageBoxOperations.ShowError("Unable to move the MAME ROM files", "Error");
                return;
            }

            MessageBoxOperations.ShowInformation($"Successfully moved the MAME ROM files to {outputDirectory}", "Finished moving the MAME ROM files");
            // TODO: Open the directory where the ROMs where moved to here
        }

        private void logViewContextMenuItem_Copy_Click(object sender, EventArgs e)
        {
            var logsListViewSelectedItems = logsListView.SelectedItems;
            if (logsListViewSelectedItems.Count == 0)
            {
                return;
            }

            var selectedItem = logsListViewSelectedItems[0];
            Clipboard.SetText(selectedItem.Text);
        }

        private void logViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Don't allow the logger context menu to be opened if there are no items selected
            var logsListViewSelectedItems = logsListView.SelectedItems;
            e.Cancel = logsListViewSelectedItems.Count == 0;
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var preferencesForm = new MainPreferencesForm {StartPosition = FormStartPosition.Manual})
            {
                preferencesForm.Location = FormOperations.GetRelativeControlPoint(this, menuStrip1);
                var dialogResult = preferencesForm.ShowDialog();
            }
        }

        private void viewDATFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDATFileViewerForm();
        }

        private void OpenDATFileViewerForm()
        {
            var datFilePath = FormFileOperations.ShowOpenFileDialog(FormFileOperations.OpenDialog_DATFilesFilter);
            if (string.IsNullOrEmpty(datFilePath))
            {
                return;
            }

            bool datFileProcessed = false;
            bool wantsToOpenNewFileAfterClosingThisOne = false;

            using (var datFileViewerForm = new DATFileViewerForm {StartPosition = FormStartPosition.CenterParent})
            {
#if !DEBUG
                try
                {
#endif
                    // Process the inputted DAT file
                    datFileProcessed = datFileViewerForm.ProcessDATFile(datFilePath);

                    // If the DAT file was successfully processed, show the DAT File Viewer form.
                    if (datFileProcessed)
                    {
                        datFileViewerForm.ShowDialog();
                        wantsToOpenNewFileAfterClosingThisOne = datFileViewerForm.WantsToOpenNewFile;
                    }
#if !DEBUG
                }
                catch (Exception ex)
                {
                    MessageBoxOperations.ShowError($"Unable to parse {datFilePath}", "An error has been encountered while trying to parse the DAT file");
                    Logger.LogException(ex);
                }
#endif
            }

            var needToShowOpenFileDialogAgain = !datFileProcessed || wantsToOpenNewFileAfterClosingThisOne;
            if (needToShowOpenFileDialogAgain)
            {
                OpenDATFileViewerForm();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var aboutDialogForm = new AboutDialogForm(){StartPosition = FormStartPosition.Manual})
            {
                aboutDialogForm.Location = FormOperations.GetRelativeControlPoint(this, menuStrip1);
                aboutDialogForm.ShowDialog();
            }
        }

        private void launchboxManagePlatformsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var launchBoxPlatformsForm = new LaunchBoxPlatformsForm(){StartPosition = FormStartPosition.Manual})
            {
                var initialized = launchBoxPlatformsForm.Initialize();
                if (!initialized)
                {
                    return;
                }

                launchBoxPlatformsForm.Location = FormOperations.GetRelativeControlPoint(this, menuStrip1);
                launchBoxPlatformsForm.ShowDialog();
            }
        }

        private void launchboxManageViewMAMESetMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new LaunchBoxMameMetadataForm(){StartPosition = FormStartPosition.Manual})
            {
                var initialized = form.Initialize();
                if (!initialized)
                {
                    return;
                }

                form.Location = FormOperations.GetRelativeControlPoint(this, menuStrip1);
                form.ShowDialog();
            }
        }
    }
}
