using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DRomsMUtils;
using Frontend;

namespace DROMsM.Forms
{
    public class LaunchBoxManager
    {
        private const string VisiblePlatformFileExtension = ".xml";
        private const string HiddenPlatformFileExtension = ".hidden";

        private const string RelativeCorePath = @"Core\";
        private const string RelativePlatformsPath = @"Data\Platforms\";

        private const string LaunchBoxExecutableFilename = @"LaunchBox.exe";

        private readonly string baseLaunchBoxPath;
        private readonly string platformsDirectory;

        public LaunchBoxManager(string launchBoxPath)
        {
            baseLaunchBoxPath = launchBoxPath;
            platformsDirectory = Path.Combine(baseLaunchBoxPath, RelativePlatformsPath);
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
}