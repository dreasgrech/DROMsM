using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Frontend;

namespace DROMsM.Forms
{
    public partial class LaunchBoxPlatformsForm : Form
    {
        private LaunchBoxManager launchBoxManager;

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
            var platformsCollection = launchBoxManager.GetPlatforms();

            olvLaunchBoxPlatformsListView.SetObjects(platformsCollection);

            return true;
        }

        private void olvLaunchBoxPlatformsListView_SubItemChecking(object sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
        {
            var listViewItem = e.ListViewItem;
            var platform = (LaunchBoxPlatform) listViewItem.RowObject;
            var newVisibilityValue = e.NewValue == CheckState.Checked;

            // Change the platform's visibility
            var platformVisibilityChanged = launchBoxManager.SetPlatformVisibility(platform, newVisibilityValue);

            // If we weren't able to successfully change the visibility of a platform, leave the list view item checkbox unchanged
            if (!platformVisibilityChanged)
            {
                e.Canceled = true;
                Logger.LogError($"Unable to change platform {platform.Name}'s visibility to {GetVisibilityVerb(newVisibilityValue)}");

                return;
            }

            // Refresh the model in the list view since it's now been updated
            olvLaunchBoxPlatformsListView.RefreshObject(platform);

            Logger.Log($"Changed platform {platform.Name}'s visibility to {GetVisibilityVerb(newVisibilityValue)}.");
        }

        private static string GetVisibilityVerb(bool visibility)
        {
            return visibility ? "visible" : "hidden";
        }
    }
}
