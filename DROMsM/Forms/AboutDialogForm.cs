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
#endif
        private AboutDialogUpdateStatus updateStatus;
        private string updateCheckErrorMessage;

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

        private UpdateManager currentUpdateManager;

        async void CheckForUpdates()
            // void CheckForUpdates()
        {
            SetUpdateStatus(AboutDialogUpdateStatus.CheckingForUpdates);

            var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/dreasgrech/DROMsM", "DROMsM", null, null, true);
            var updateInfo = await updateManager.CheckForUpdate(false);
            bool updateAvailable = updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0;
            updateStatus = updateAvailable ? AboutDialogUpdateStatus.UpdateAvailable : AboutDialogUpdateStatus.NoUpdateAvailable;

            SetUpdateStatus(updateStatus);
            if (updateStatus != AboutDialogUpdateStatus.UpdateAvailable)
            {
                // Dispose the UpdateManager
                updateManager.Dispose();

                return;
            }

            var wantsToUpdate = MessageBoxOperations.ShowConfirmation("An update is available. Would you like to update DROMsM now?", "Update Available");
            if (wantsToUpdate)
            {
                // Update the app
                var releaseEntry = await currentUpdateManager.UpdateApp();

                // Dispose the UpdateManager since we're done with it now
                updateManager.Dispose();

                MessageBoxOperations.ShowInformation("The application has been upgraded, and will now restart.", "DROMsM has been updated");
                Application.Restart();
                return;
            }

            /*
            // This will give us a Task that fetches the UpdateManager
            var updateManagerTask = UpdateManager.GitHubUpdateManager("https://github.com/dreasgrech/DROMsM", "DROMsM", null, null, true);
            updateManagerTask.ContinueWith(updateManagerTaskContinuationAction =>
            {
                // currentUpdateManager = continuationAction.Result;
                var updateManager = updateManagerTaskContinuationAction.Result;

                var checkForUpdateTask = updateManager.CheckForUpdate(false);
                checkForUpdateTask.ContinueWith(checkForUpdateTaskContinuationTask =>
                {
                    var updateInfo = checkForUpdateTaskContinuationTask.Result;
                    bool updateAvailable = updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0;
                    updateStatus = updateAvailable ? AboutDialogUpdateStatus.UpdateAvailable : AboutDialogUpdateStatus.NoUpdateAvailable;

                    SetUpdateStatus(updateStatus);
                    if (updateStatus == AboutDialogUpdateStatus.UpdateAvailable)
                    {
                        // ReleaseEntry releaseEntry = currentUpdateManager.UpdateApp();
                        MessageBoxOperations.ShowConfirmation("Update available", "Update available");
                    }

                    // Dispose the UpdateManager
                    updateManager.Dispose();

                }, TaskScheduler.FromCurrentSynchronizationContext()).Start();
            });
            */

            /*
            var task = Task.Run(async () =>
            {
                currentUpdateManager = await UpdateManager.GitHubUpdateManager("https://github.com/dreasgrech/DROMsM", "DROMsM", null, null, true);

                var updateInfo = await currentUpdateManager.CheckForUpdate();
                bool updateAvailable = updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0;
                updateStatus = updateAvailable ? AboutDialogUpdateStatus.UpdateAvailable : AboutDialogUpdateStatus.NoUpdateAvailable;
            });

            Task uiTask = task.ContinueWith(OnAfterFinishedCheckingForUpdates, TaskScheduler.FromCurrentSynchronizationContext());
            */

            // await SquirrelUpdateApp();
        }

        //private void OnAfterFinishedCheckingForUpdates(Task task)
        //{
        //    SetUpdateStatus(updateStatus);

        //    if (updateStatus == AboutDialogUpdateStatus.UpdateAvailable)
        //    {
        //        ReleaseEntry releaseEntry = currentUpdateManager.UpdateApp();
        //    }
        //}
        
        //static async Task SquirrelUpdateApp()
        //{
        //    using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/dreasgrech/DROMsM", "DROMsM", null, null, true))
        //    {
        //        // await mgr.Result.UpdateApp();
        //        var updateInfo = await mgr.CheckForUpdate();
        //        Logger.Log($"Currently Installed Version: {updateInfo.CurrentlyInstalledVersion}, Releases to apply: {updateInfo.ReleasesToApply}, FutureReleaseEntry: {updateInfo.FutureReleaseEntry}");

        //        ReleaseEntry releaseEntry = await mgr.UpdateApp();
        //        if (releaseEntry != null)
        //        {
        //            Logger.Log($"" +
        //                       $"BaseUrl: {releaseEntry.BaseUrl}, " +
        //                       $"EntryAsString: {releaseEntry.EntryAsString}, " +
        //                       $"Filename: {releaseEntry.Filename}, " +
        //                       $"Filesize: {releaseEntry.Filesize}, " +
        //                       $"IsDelta: {releaseEntry.IsDelta}, " +
        //                       $"PackageName: {releaseEntry.PackageName}, " +
        //                       $"Query: {releaseEntry.Query}, " +
        //                       $"Version: {releaseEntry.Version}, " +
        //                       $"");
        //        }
        //        else
        //        {
        //            Logger.Log($"releaseEntry is null");
        //        }
        //    }
        //}


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
            }

            updatesLabel.LinkColor = color;
            updatesLabel.Text = text;
        }

        private void updatesLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckForUpdates();

            /*
            switch (updateStatus)
            {
                case AboutDialogUpdateStatus.ErrorCheckingForUpdates:
                case AboutDialogUpdateStatus.NoUpdateAvailable:
                {
                    CheckForUpdates();
                }
                    break;
                case AboutDialogUpdateStatus.UpdateAvailable:
                {
                        DownloadUpdate();
                }
                    break;
            }
            */
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
        ErrorCheckingForUpdates
    }
}
