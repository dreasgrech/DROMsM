using System;
using System.Collections.Generic;

namespace DIOUtils
{
    public static class FileUtilities
    {
        public const char DirectoryBackSlash = '\\';
        public const char DirectoryForwardSlash = '/';

        private static int WindowsMaxFilePathLength = 260;

        public static bool MoveDirectory(string sourcePath, string destinationPath)
        {
            try
            {
                Alphaleonis.Win32.Filesystem.Directory.Move(sourcePath, destinationPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CopyFile(string sourceFilename, string destFilename, bool overwrite)
        {
            try
            {
                var destFileExists = FileExists(destFilename);
                if (destFileExists)
                {
                    if (!overwrite)
                    {
                        return false;
                    }

                    // Delete the destination file since it already exists and we're overwriting
                    DeleteFile(destFilename);
                }

                //// Make sure there isn't a file that already exists at the destination location
                //if (!overwrite && FileExists(destFilename))
                //{
                //    return false;
                //}

                // var filePathsExceedMaxPath = sourceFilename.Length >= WindowsMaxFilePathLength || destFilename.Length >= WindowsMaxFilePathLength;
                var filePathsExceedMaxPath = PathExceedsWindowsMaxFilePathLength(sourceFilename) || PathExceedsWindowsMaxFilePathLength(destFilename);
                if (filePathsExceedMaxPath)
                {
                    Alphaleonis.Win32.Filesystem.File.Copy(sourceFilename, destFilename);
                }
                else
                {
                    System.IO.File.Copy(sourceFilename, destFilename);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool MoveFile(string sourceFilename, string destFilename)
        {
            try
            {
                // Make sure there isn't a file that already exists at the destination location
                if (FileExists(destFilename))
                {
                    return false;
                }

                // var filePathsExceedMaxPath = sourceFilename.Length >= WindowsMaxFilePathLength || destFilename.Length >= WindowsMaxFilePathLength;
                var filePathsExceedMaxPath = PathExceedsWindowsMaxFilePathLength(sourceFilename) || PathExceedsWindowsMaxFilePathLength(destFilename);
                if (filePathsExceedMaxPath)
                {
                    Alphaleonis.Win32.Filesystem.File.Move(sourceFilename, destFilename);
                }
                else
                {
                    System.IO.File.Move(sourceFilename, destFilename);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CreateDirectory(string path)
        {
            try
            {
                // Make sure the directory doesn't already exist
                if (DirectoryExists(path))
                {
                    return false;
                }

                // Create the directory
                Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(path);

                // Make sure the directory was created
                if (!DirectoryExists(path))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string[] GetDirectories(string path, string pattern, System.IO.SearchOption option)
        {
            try
            {
                // Make sure the directory already exists
                if (!DirectoryExists(path))
                {
                    return new string[0];
                }

                return Alphaleonis.Win32.Filesystem.Directory.GetDirectories(path, pattern, option);
            }
            catch (Exception ex)
            {
                return new string[0];
            }
        }

        public static Alphaleonis.Win32.Filesystem.DirectoryInfo CreateDirectoryInfo(string path)
        {
            return new Alphaleonis.Win32.Filesystem.DirectoryInfo(path);
        }

        public static string GetNameOfDirectory(string path)
        {
            var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
            return directoryInfo.Name;
        }

        public static string GetDirectoryFullname(string path)
        {
            var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
            return directoryInfo.FullName;
        }

        public static string GetDirectory(string path)
        {
            return Alphaleonis.Win32.Filesystem.Path.GetDirectoryName(path);
        }

        public static void GetDirectoryInfo(string path, out string name, out string fullName)
        {
            var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
            name = directoryInfo.Name;
            fullName = directoryInfo.FullName;
        }

        public static string GetDirectoryParent(string path)
        {
            var directoryInfo = Alphaleonis.Win32.Filesystem.Directory.GetParent(path);
            if (directoryInfo == null)
            {
                return null;
            }

            return directoryInfo.FullName;
        }

        public static string GetFileName(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetExtension(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.Path.GetExtension(filePath);
        }

        public static string CombinePath(params string[] paths)
        {
            return Alphaleonis.Win32.Filesystem.Path.Combine(paths);
        }

        public static bool FileExists(string file)
        {
            return Alphaleonis.Win32.Filesystem.File.Exists(file);
        }

        public static void WriteAllText(string path, string contents)
        {
            Alphaleonis.Win32.Filesystem.File.WriteAllText(path, contents);
        }

        public static bool DirectoryExists(string path)
        {
            return Alphaleonis.Win32.Filesystem.Directory.Exists(path);
        }

        public static IList<Opulos.Core.IO.FastFileInfo> GetFiles(string directory, string searchPattern, System.IO.SearchOption directoryOptions)
        {
            return Opulos.Core.IO.FastFileInfo.GetFiles(directory, searchPattern, directoryOptions);
        }

        public static void DeleteDirectory(string path)
        {
            Alphaleonis.Win32.Filesystem.Directory.Delete(path);
        }

        public static bool DeleteFile(string path)
        {
            if (!FileExists(path))
            {
                return false;
            }

            Alphaleonis.Win32.Filesystem.File.Delete(path);

            return true;
        }

        public static bool PathContainsDirectory(string path, string directoryName)
        {
            var normalizedPath = NormalizePathSlashes(path);
            var pathStartsWithDirectory = normalizedPath.StartsWith(directoryName, StringComparison.OrdinalIgnoreCase);
            if (pathStartsWithDirectory)
            {
                return true;
            }

            var pathContainsDirectory =
                normalizedPath.Contains($"{directoryName}{DirectoryBackSlash}")
                ||
                normalizedPath.Contains($"{DirectoryBackSlash}{directoryName}");
            if (pathContainsDirectory)
            {
                return true;
            }

            return false;
        }

        public static string NormalizePathSlashes(string path)
        {
            // Normalize the slashes in the roms path
            return path.Replace(DirectoryForwardSlash, DirectoryBackSlash);
        }

        public static string[] ReadAllLines(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.File.ReadAllLines(filePath, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
        }

        public static IEnumerable<string> ReadLines(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.File.ReadLines(filePath, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
        }

        public static bool PathExceedsWindowsMaxFilePathLength(string filePath)
        {
            return filePath.Length >= WindowsMaxFilePathLength;
        }
    }
}
