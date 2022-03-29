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

namespace DROMsM
{
    public partial class AboutDialogForm : Form
    {
        private UpdateCheckInfo updateInfo;
        private ApplicationDeployment applicationDeployment;
        private AboutDialogUpdateStatus updateStatus;
        private string updateCheckErrorMessage;

        public AboutDialogForm()
        {
            InitializeComponent();
        }

        private void AboutDialogForm_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            SetUpdateStatus(AboutDialogUpdateStatus.CheckingForUpdates);

            var task = Task.Run(() =>
            {
                updateStatus = IsUpdateAvailable(out updateInfo, out applicationDeployment, out updateCheckErrorMessage);
            });

            if (!string.IsNullOrEmpty(updateCheckErrorMessage))
            {
                Logger.LogError(updateCheckErrorMessage);
            }

            Task uiTask = task.ContinueWith(OnAfterFinishedCheckingForUpdates, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnAfterFinishedCheckingForUpdates(Task task)
        {
            SetUpdateStatus(updateStatus);

            if (updateStatus == AboutDialogUpdateStatus.UpdateAvailable)
            {
                DownloadUpdate();
            }
        }

        private AboutDialogUpdateStatus IsUpdateAvailable(out UpdateCheckInfo updateCheckInfo, out ApplicationDeployment ad, out string errorMessage)
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
                errorMessage = $"The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: {dde.Message}";
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }
            catch (InvalidDeploymentException ide)
            {
                errorMessage = $"Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: {ide.Message}";
                return AboutDialogUpdateStatus.ErrorCheckingForUpdates;
            }
            catch (InvalidOperationException ioe)
            {
                errorMessage = $"This application cannot be updated. It is likely not a ClickOnce application. Error: {ioe.Message}";
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
