using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frontend;

namespace DROMsM.Forms
{
    public partial class LaunchBoxMameMetadataForm : Form
    {
        private LaunchBoxManager launchBoxManager;

        private LaunchBoxMAMEMetadataFile mameMetadataFile;

        private MAMEMetadataFileEntryVirtualListDataSource mameMetadataFileEntryVirtualListDataSource;

        public LaunchBoxMameMetadataForm()
        {
            InitializeComponent();

            olvFileNameColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.FileName;
            olvNameColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Name;
            olvStatusColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Status;
            olvDeveloperColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Developer;
            olvPublisherColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Publisher;
            olvYearColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Year;
            olvIsMechanicalColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsMechanical;
            olvIsBootlegColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsBootleg;
            olvIsPrototypeColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsPrototype;
            olvIsHackColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsHack;
            olvIsMatureColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsMature;
            olvIsQuizColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsQuiz;
            olvIsFruitColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsFruit;
            olvIsCasinoColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsCasino;
            olvIsRhythmColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsRhythm;
            olvIsTableTopColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsTableTop;
            olvIsPlayChoiceColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsPlayChoice;
            olvIsMahjongColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsMahjong;
            olvIsNonArcadeColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.IsNonArcade;
            olvGenreColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Genre;
            olvPlayModeColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Playmode;
            olvLanguageColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Language;
            olvSourceColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Source;

            /*
            olvFileNameColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).FileName;
            olvNameColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Name;
            olvStatusColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Status;
            olvDeveloperColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Developer;
            olvPublisherColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Publisher;
            olvYearColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Year;
            olvIsMechanicalColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsMechanical;
            olvIsBootlegColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsBootleg;
            olvIsPrototypeColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsPrototype;
            olvIsHackColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsHack;
            olvIsMatureColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsMature;
            olvIsQuizColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsQuiz;
            olvIsFruitColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsFruit;
            olvIsCasinoColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsCasino;
            olvIsRhythmColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsRhythm;
            olvIsTableTopColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsTableTop;
            olvIsPlayChoiceColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsPlayChoice;
            olvIsMahjongColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsMahjong;
            olvIsNonArcadeColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).IsNonArcade;
            olvGenreColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Genre;
            olvPlayModeColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Playmode;
            olvLanguageColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Language;
            olvSourceColumn.AspectGetter = rowObject => ((LaunchBoxMAMEMetadataFileEntry) rowObject).Source;
            */
        }

        public bool Initialize()
        {
            var launchBoxSettings = ProjectSettingsManager.LaunchBoxSettings;
            var launchBoxDirectory = launchBoxSettings.LaunchBoxPath;
            if (string.IsNullOrEmpty(launchBoxDirectory))
            {
                MessageBoxOperations.ShowError("Please set the LaunchBox directory from Edit -> Preferences", "Unable to find LaunchBox");
                return false;
            }

            var platformsDirectoryExists = LaunchBoxManager.MAMEMetadataFileExists(launchBoxDirectory, out string searchForLaunchBoxMAMEMetadataFilePath);
            if (!platformsDirectoryExists)
            {
                MessageBoxOperations.ShowError($"Unable to find the MAME metadata XML file at {searchForLaunchBoxMAMEMetadataFilePath}\r\n\r\nPlease make sure the correct path to LaunchBox is specified from Edit -> Preferences", $"Unable to find the LaunchBox MAME metadata XML file");
                return false;
            }

            launchBoxManager = new LaunchBoxManager(launchBoxDirectory);
            mameMetadataFile = launchBoxManager.ParseMAMEMetadataXMLFile();

            // Bind the platforms list to the list view
            // olvLaunchBoxPlatformsListView.SetObjects(mameMetadataFile.Entries);

            mameMetadataFileEntryVirtualListDataSource = new MAMEMetadataFileEntryVirtualListDataSource(olvLaunchBoxPlatformsListView);
            mameMetadataFileEntryVirtualListDataSource.AddObjects(mameMetadataFile.Entries);
            olvLaunchBoxPlatformsListView.VirtualListDataSource = mameMetadataFileEntryVirtualListDataSource;

            return true;
        }

        private void olvLaunchBoxPlatformsListView_SubItemChecking(object sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
        {
            var listViewItem = e.ListViewItem;
            var platform = (LaunchBoxPlatform) listViewItem.RowObject;
            var newVisibilityValue = e.NewValue == CheckState.Checked;

            // Change the platform's visibility
            var platformVisibilityChanged = SetPlatformVisibility(platform, newVisibilityValue);

            // If we weren't able to successfully change the visibility of a platform, leave the list view item checkbox unchanged
            if (!platformVisibilityChanged)
            {
                e.Canceled = true;
                return;
            }

            // Refresh the model in the list view since it's now been updated
            olvLaunchBoxPlatformsListView.RefreshObject(platform);
        }

        private void SetAllPlatformsVisibility(bool newVisibility)
        {
            /*
            var totalPlatforms = launchBoxPlatforms.Count;
            var visibleVerb = GetVisibilityVerb(newVisibility);
            var confirmResult = MessageBoxOperations.ShowConfirmation($"Are you sure you want set the visibility of all {totalPlatforms} platforms to {visibleVerb}?", $"Setting {totalPlatforms} platforms to {visibleVerb}");
            if (!confirmResult)
            {
                return;
            }

            int totalPlatformsSet = 0;
            Parallel.ForEach(launchBoxPlatforms, platform =>
            {
                var platformVisibilityChanged = SetPlatformVisibility(platform, newVisibility);
                if (platformVisibilityChanged)
                {
                    Interlocked.Increment(ref totalPlatformsSet);
                }
            });

            olvLaunchBoxPlatformsListView.RefreshObjects(launchBoxPlatforms);

            Logger.Log($"Set the visibility of {totalPlatformsSet} LaunchBox platforms to {visibleVerb}.");
            */
        }

        private bool SetPlatformVisibility(LaunchBoxPlatform platform, bool visibility)
        {
            var platformVisibilityChanged = launchBoxManager.SetPlatformVisibility(platform, visibility);
            if (platformVisibilityChanged)
            {
                Logger.Log($"Changed LaunchBox platform {platform.Name}'s visibility to {GetVisibilityVerb(visibility)}.");
                return true;
            }


            Logger.LogError($"Unable to change LaunchBox platform {platform.Name}'s visibility to {GetVisibilityVerb(visibility)}");

            return false;
        }

        private void showAllToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // SetAllPlatformsVisibility(true);
        }

        private void hideAllToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // SetAllPlatformsVisibility(false);
        }

        private static string GetVisibilityVerb(bool visibility)
        {
            return visibility ? "visible" : "hidden";
        }

        private void closeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void howDoesThisWorkToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var text = @"Launchbox keeps a record of your platforms in XML files in the Data\Platforms directory.

By changing the file extension of these individual files, we can hide or show platforms in LaunchBox and that's what this tool does.";

            MessageBoxOperations.ShowInformation(text, "How are the LaunchBox platforms changing visibility?");
        }
    }
}
