﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opulos.Core.IO;

namespace DRomsMUtils
{
    public static class FileUtilities
    {
        private static int WindowsMaxFilePathLength = 260;

        public static void MoveDirectory(string sourcePath, string destinationPath)
        {
            Alphaleonis.Win32.Filesystem.Directory.Move(sourcePath, destinationPath);
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

                var filePathsExceedMaxPath = sourceFilename.Length >= WindowsMaxFilePathLength || destFilename.Length >= WindowsMaxFilePathLength;
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

        public static string GetDirectoryName(string path)
        {
            var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
            return directoryInfo.Name;
        }

        public static string GetDirectoryFullname(string path)
        {
            var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
            return directoryInfo.FullName;
        }

        public static void GetDirectoryInfo(string path, out string name, out string fullName)
        {
            var directoryInfo = FileUtilities.CreateDirectoryInfo(path);
            name = directoryInfo.Name;
            fullName = directoryInfo.FullName;
        }

        public static string GetFileName(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Alphaleonis.Win32.Filesystem.Path.GetFileNameWithoutExtension(filePath);
        }

        public static string CombinePath(params string[] paths)
        {
            return Alphaleonis.Win32.Filesystem.Path.Combine(paths);
        }

        public static bool FileExists(string file)
        {
            return Alphaleonis.Win32.Filesystem.File.Exists(file);
        }

        public static bool DirectoryExists(string path)
        {
            return Alphaleonis.Win32.Filesystem.Directory.Exists(path);
        }

        public static IList<FastFileInfo> GetFiles(string directory, string searchPattern, System.IO.SearchOption directoryOptions)
        {
            return FastFileInfo.GetFiles(directory, searchPattern, directoryOptions);
        }

        public static void DeleteDirectory(string path)
        {
            Alphaleonis.Win32.Filesystem.Directory.Delete(path);
        }
    }
}
