// #define CLICKONCE
#define SQUIRREL

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frontend;
using Squirrel;

namespace DROMsM
{
    public partial class AboutDialogForm : Form
    {
#if CLICKONCE
        private UpdateCheckInfo updateInfo;
        private ApplicationDeployment applicationDeployment;
        private string updateCheckErrorMessage;
#endif
        private AboutDialogUpdateStatus updateStatus;

        public AboutDialogForm()
        {
            InitializeComponent();
        }

        private void AboutDialogForm_Load(object sender, EventArgs e)
        {
#if CLICKONCE
            CheckForUpdates();
#endif

#if SQUIRREL
            CheckForUpdates();
#endif
        }

#if CLICKONCE
        private void CheckForUpdates()
        {
            SetUpdateStatus(AboutDialogUpdateStatus.CheckingForUpdates);

            var task = Task.Run(() =>
            {
                updateStatus = IsUpdateAvailable_ClickOnce(out updateInfo, out applicationDeployment, out updateCheckErrorMessage);
            });

            if (!string.IsNullOrEmpty(updateCheckErrorMessage))
            {
                Logger.LogError(updateCheckErrorMessage);
            }

            Task uiTask = task.ContinueWith(OnAfterFinishedCheckingForUpdates, TaskScheduler.FromCurrentSynchronizationContext());
        }
#endif

        private async void CheckForUpdates()
        {
            if (updateStatus == AboutDialogUpdateStatus.CheckingForUpdates)
            {
                return;
            }

            SetUpdateStatus(AboutDialogUpdateStatus.CheckingForUpdates);

            bool updateInstalled = false;

            using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/dreasgrech/DROMsM", "DROMsM", null, null, true))
            {
                var updateInfo = await updateManager.CheckForUpdate(false);
                bool updateAvailable = updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0;
                updateStatus = updateAvailable ? AboutDialogUpdateStatus.UpdateAvailable : AboutDialogUpdateStatus.NoUpdateAvailable;

                SetUpdateStatus(updateStatus);
                if (updateStatus != AboutDialogUpdateStatus.UpdateAvailable)
                {
                    return;
                }

                // string msg = "An update is available. Would you like to update DROMsM now?";
                string confirmationMessage = GetUpdateConfirmationMessage(updateInfo);

                // var wantsToUpdate = MessageBoxOperations.ShowConfirmation("An update is available. Would you like to update DROMsM now?", "Update Available");
                var wantsToUpdate = MessageBoxOperations.ShowConfirmation(confirmationMessage, "Update Available");
                if (wantsToUpdate)
                {
                    SetUpdateStatus(AboutDialogUpdateStatus.InstallingUpdate);
                    // Update the app
                    var releaseEntry = await updateManager.UpdateApp();
                    //Logger.Log($"BaseUrl: {releaseEntry.BaseUrl}, " +
                    //           $"EntryAsString: {releaseEntry.EntryAsString}, " +
                    //           $"Filename: {releaseEntry.Filename}, " +
                    //           $"Filesize: {releaseEntry.Filesize}, " +
                    //           $"IsDelta: {releaseEntry.IsDelta}, " +
                    //           $"PackageName: {releaseEntry.PackageName}, " +
                    //           $"Query: {releaseEntry.Query}, " +
                    //           $"Version: {releaseEntry.Version}, " +
                    //           $"");

                    ProjectSettingsManager.BackupSettings();

                    updateInstalled = true;
                }
            }

            if (updateInstalled)
            {
                SetUpdateStatus(AboutDialogUpdateStatus.UpdateInstalledWaitingForRestart);

                MessageBoxOperations.ShowInformation("DROMsM has been updated and will now restart.", "Update Complete");
                // Application.Restart();
                UpdateManager.RestartApp();
                return;
            }
        }

        private static string GetUpdateConfirmationMessage(UpdateInfo updateInfo)
        {
            string confirmationMessage = "An update is available.";
            var currentlyInstalledVersion = updateInfo.CurrentlyInstalledVersion;
            if (currentlyInstalledVersion != null)
            {
                confirmationMessage += "\r\nCurrent version: " + currentlyInstalledVersion.Version;
            }

            var futureReleaseEntry = updateInfo.FutureReleaseEntry;
            if (futureReleaseEntry != null)
            {
                confirmationMessage += "\r\nNew version: " + futureReleaseEntry.Version;
            }

            confirmationMessage += "\r\n\r\nWould you like to update DROMsM now?";

            return confirmationMessage;
        }

#if CLICKONCE
        private AboutDialogUpdateStatus IsUpdateAvailable_ClickOnce(out UpdateCheckInfo updateCheckInfo, out ApplicationDeployment ad, out string errorMessage)
        {
            errorMessage = null;
            updateCheckInfo = null;
            ad = null;

            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }

            ad = ApplicationDeployment.CurrentDeployment;

            try
            {
                updateCheckInfo = ad.CheckForDetailedUpdate();
            }
            catch (DeploymentDownloadException dde)
            {
                errorMessage = $"The new version of DROMsM cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: {dde.Message}";
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }
            catch (InvalidDeploymentException ide)
            {
                errorMessage = $"Cannot check for a new version of DROMsM. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: {ide.Message}";
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }
            catch (InvalidOperationException ioe)
            {
                errorMessage = $"DROMsM cannot be updated. It is likely not a ClickOnce application. Error: {ioe.Message}";
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occured while trying to check for updates.  Error: {ex.Message}";
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }

            if (!updateCheckInfo.UpdateAvailable)
            {
                return AboutDialogUpdateStatus.NoUpdateAvailable;
            }

            return AboutDialogUpdateStatus.UpdateAvailable;
        }

        private void OnAfterFinishedCheckingForUpdates(Task task)
        {
            SetUpdateStatus(updateStatus);

            if (updateStatus == AboutDialogUpdateStatus.UpdateAvailable)
            {
                DownloadUpdate_ClickOnce();
            }
        }

        private void DownloadUpdate()
        {
            if (!updateInfo.UpdateAvailable)
            {
                return;
            }

            var doUpdate = true;

            if (!updateInfo.IsUpdateRequired)
            {
                var wantsToUpdate = MessageBoxOperations.ShowConfirmation("An update is available. Would you like to update DROMsM now?", "Update Available");
                if (!wantsToUpdate)
                {
                    doUpdate = false;
                }
            }
            else
            {
                // Display a message that the app MUST reboot. Display the minimum required version.
                MessageBoxOperations.ShowInformation($"This application has detected a mandatory update from your current version to version {updateInfo.MinimumRequiredVersion}. The application will now install the update and restart.", "Update Available");
            }

            if (doUpdate)
            {
                try
                {
                    applicationDeployment.Update();
                    MessageBoxOperations.ShowInformation("The application has been upgraded, and will now restart.", "DROMsM has been updated");
                    Application.Restart();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBoxOperations.ShowError($"Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: {dde}", "Unable to update DROMsM");
                }
            }
        }
#endif

        private void SetUpdateStatus(AboutDialogUpdateStatus status)
        {
            updateStatus = status;
            SetUpdatesLabel(updateStatus);
        }

        private void SetUpdatesLabel(AboutDialogUpdateStatus status)
        {
            Color color = updatesLabel.ForeColor;
            string text = string.Empty;
            switch (status)
            {
                case AboutDialogUpdateStatus.CheckingForUpdates:
                {
                    color = Color.Sienna;
                    text = "Checking for updates...";
                } break;
                case AboutDialogUpdateStatus.NoUpdateAvailable:
                {
                    color = Color.Green;
                    text = "No update available";
                } break;
                case AboutDialogUpdateStatus.UpdateAvailable:
                {
                    color = Color.BlueViolet; 
                    text = "Update available!";
                } break;
                case AboutDialogUpdateStatus.ErrorCheckingForUpdates:
                {
                    color = Color.Red; 
                    text = "Error checking for updates";
                } break;
                case AboutDialogUpdateStatus.InstallingUpdate:
                {
                    color = Color.Brown; 
                    text = "Installing update...";
                } break;
                case AboutDialogUpdateStatus.UpdateInstalledWaitingForRestart:
                {
                    color = Color.DarkGreen; 
                    text = "Update Installed!";
                } break;
            }

            updatesLabel.LinkColor = color;
            updatesLabel.Text = text;
        }

        private void updatesLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckForUpdates();
        }

        private void AboutDialogForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void AboutDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }

    public enum AboutDialogUpdateStatus
    {
        None,
        CheckingForUpdates,
        UpdateAvailable,
        NoUpdateAvailable,
        ErrorCheckingForUpdates,
        InstallingUpdate,
        UpdateInstalledWaitingForRestart
    }
}
