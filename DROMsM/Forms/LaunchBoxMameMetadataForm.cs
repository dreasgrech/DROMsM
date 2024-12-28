using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
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
            olvCloneOfColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.CloneOf;
            olvStatusColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Status;
            // olvDeveloperColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Developer;
            olvPublisherColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Publisher;
            olvYearColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Year;
            olvVersionColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Version;
            olvRegionColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Region;
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
            olvSeriesColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Series;
            olvPlayModeColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Playmode;
            olvLanguageColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Language;
            olvSourceColumn.AspectGetter = rowObject => (rowObject as LaunchBoxMAMEMetadataFileEntry)?.Source;

            olvIsMechanicalColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsBootlegColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsPrototypeColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsHackColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsMatureColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsQuizColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsFruitColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsCasinoColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsRhythmColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsTableTopColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsPlayChoiceColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsMahjongColumn.AspectToStringConverter = BooleanAspectToStringConverter;
            olvIsNonArcadeColumn.AspectToStringConverter = BooleanAspectToStringConverter;
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

        private void closeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void howDoesThisWorkToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var form = new LaunchBoxMameMetadataAboutForm())
            {
                // Open the form at the top left of our form
                form.StartPosition = FormStartPosition.Manual;
                form.Location = FormOperations.GetRelativeControlPoint(this, topMenuStrip);
                form.ShowDialog();
            }
        }

        private void findToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (var findForm = new DATFileViewerFindForm("Search", ProjectSettingsManager.DATFileViewerSettings.FindDialogSettings))
            {
                // Open the form at the top left of our form
                findForm.StartPosition = FormStartPosition.Manual;
                findForm.Location = FormOperations.GetRelativeControlPoint(this, topMenuStrip);
                findForm.ShowDialog();

                var searchTerm = findForm.SearchTerm;
                var useRegularExpressions = findForm.UseRegularExpressions;

                var filter = useRegularExpressions ? TextMatchFilter.Regex(olvLaunchBoxPlatformsListView, searchTerm) : TextMatchFilter.Contains(olvLaunchBoxPlatformsListView, searchTerm);
                // filter.Columns = olvDatFileListView.AllColumns.ToArray();
                olvLaunchBoxPlatformsListView.AdditionalFilter = filter;
            }
        }

        private void olvLaunchBoxPlatformsListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            //if (!showingWorkingColors)
            //{
            //    return;
            //}

            var launchBoxMAMEMetadataFileEntry = e.Model as LaunchBoxMAMEMetadataFileEntry;
            if (launchBoxMAMEMetadataFileEntry == null)
            {
                return;
            }

            var status = launchBoxMAMEMetadataFileEntry.Status;
            if (string.IsNullOrEmpty(status))
            {
                return;
            }

            var olvListItem = e.Item;
            // var statusString = status.ToLowerInvariant();
            var statusString = status;
            switch (statusString)
            {
                case "good":
                {
                    olvListItem.BackColor = Color.LightGreen;
                }
                    break;
                case "preliminary":
                {
                    olvListItem.BackColor = Color.LightCoral;
                }
                    break;
                case "imperfect":
                case "protection":
                {
                    olvListItem.BackColor = Color.LightYellow;
                }
                    break;
            }
        }

        private string BooleanAspectToStringConverter(object cellValue)
        {
            return (bool)cellValue ? "Yes" : string.Empty;
        }
    }
}
