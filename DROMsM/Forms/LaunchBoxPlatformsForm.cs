using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frontend;

namespace DROMsM.Forms
{
    public partial class LaunchBoxPlatformsForm : Form
    {
        private LaunchBoxManager launchBoxManager;

        private List<LaunchBoxPlatform> launchBoxPlatforms;

        public LaunchBoxPlatformsForm()
        {
            InitializeComponent();
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

            var platformsDirectoryExists = LaunchBoxManager.PlatformsDirectoryExists(launchBoxDirectory, out string searchForLaunchBoxPlatformsDirectory);
            if (!platformsDirectoryExists)
            {
                MessageBoxOperations.ShowError($"Unable to find the platforms at {searchForLaunchBoxPlatformsDirectory}\r\n\r\nPlease make sure the correct path to LaunchBox is specified from Edit -> Preferences", $"Unable to find the LaunchBox platforms");
                return false;
            }

            launchBoxManager = new LaunchBoxManager(launchBoxDirectory);
            launchBoxPlatforms = launchBoxManager.GetPlatforms();

            // Bind the platforms list to the list view
            olvLaunchBoxPlatformsListView.SetObjects(launchBoxPlatforms);

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
            SetAllPlatformsVisibility(true);
        }

        private void hideAllToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SetAllPlatformsVisibility(false);
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
