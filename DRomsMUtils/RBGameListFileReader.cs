using System;
using System.IO;
using DIOUtils;
using Frontend;
using U8Xml;

namespace DROMsM
{
    public class RBGameListFileReader
    {
        private RBGameListFile currentGameListFile;

        private string FindGameListFile(string rbGameListFilePath)
        {
            var fileExists = FileUtilities.FileExists(rbGameListFilePath);
            if (!fileExists)
            {
                // var fileDirectoryPath = FileUtilities.GetDirectoryName(rbGameListFilePath);
                var fileDirectoryPath = FileUtilities.GetDirectory(rbGameListFilePath);
                if (string.IsNullOrEmpty(fileDirectoryPath))
                {
                    return null;
                }

                var parentDirectory = Directory.GetParent(fileDirectoryPath);
                if (parentDirectory == null)
                {
                    return null;
                }

                var filePath = GetFilePath(parentDirectory.FullName);
                return FindGameListFile(filePath);
            }

            return rbGameListFilePath;
        }

        public RBGameListFile ReadGameListFile(string romsPath)
        {
            // var gameListFilePath = Path.Combine(romsPath, "gamelist.xml");
            var rbGameListFilePath = FindGameListFile(GetFilePath(romsPath));

            var fileExists = FileUtilities.FileExists(rbGameListFilePath);
            if (!fileExists)
            {
                return null;
            }

            currentGameListFile = new RBGameListFile
            {
                FilePath = rbGameListFilePath,
                // FileBaseDirectoryPath = FileUtilities.GetDirectoryName(rbGameListFilePath)
                FileBaseDirectoryPath = FileUtilities.GetDirectory(rbGameListFilePath)
            };

            using (var xml = XmlParser.ParseFile(rbGameListFilePath))
            {
                var root = xml.Root;
                foreach (var node in root.Children)
                {
                    var nodeName = node.Name.ToString();
                    switch (nodeName)
                    {
                        case "folder":ReadFolder(node); break;
                        case "game":ReadGame(node); break;
                    }
                }
            }

            return currentGameListFile;
        }

        private void ReadFolder(XmlNode folderXMLNode)
        {
            var rbGameListFolder = new RBGameListFolder();
            foreach (var node in folderXMLNode.Children)
            {
                var nodeName = node.Name.ToString();
                var nodeValue = node.InnerText.ToString();
                switch (nodeName)
                {
                    case "path": rbGameListFolder.Path = nodeValue; break;
                    case "name": rbGameListFolder.Name = nodeValue; break;
                }
            }

            currentGameListFile.AddFolder(rbGameListFolder);
        }

        private void ReadGame(XmlNode gameXMLNode)
        {
            var rbGameListGame = new RBGameListGame();
            foreach (var node in gameXMLNode.Children)
            {
                var nodeName = node.Name.ToString();
                var nodeValue = node.InnerText.ToString();
                switch (nodeName)
                {
                    case "path": rbGameListGame.Path = nodeValue; break;
                    case "hash": rbGameListGame.Hash = nodeValue; break;
                    case "lastplayed": rbGameListGame.LastPlayed = nodeValue; break;
                    case "playcount": rbGameListGame.PlayCount = Convert.ToInt32(nodeValue); break;
                    case "region": rbGameListGame.Region = Convert.ToInt32(nodeValue); break;
                    case "genreid": rbGameListGame.GenreID = Convert.ToInt32(nodeValue); break;
                    case "genre": rbGameListGame.Genre = nodeValue; break;
                    case "publisher": rbGameListGame.Publisher = nodeValue; break;
                    case "developer": rbGameListGame.Developer = nodeValue; break;
                    case "releasedate": rbGameListGame.ReleaseDate = nodeValue; break;
                    case "video": rbGameListGame.Video = nodeValue; break;
                    case "thumbnail": rbGameListGame.Thumbnail = nodeValue; break;
                    case "image": rbGameListGame.Image = nodeValue; break;
                    case "desc": rbGameListGame.Description = nodeValue; break;
                    case "rating": rbGameListGame.Rating = Convert.ToSingle(nodeValue); break;
                    case "name": rbGameListGame.Name = nodeValue; break;
                }
            }

            rbGameListGame.Filename = FileUtilities.GetFileName(rbGameListGame.Path);

            currentGameListFile.AddGame(rbGameListGame);

            // Logger.Log($"Read game from gamelist.xml.  Path: {rbGameListGame.Path}");
        }

        private string GetFilePath(string directory)
        {
            return FileUtilities.CombinePath(directory, "gamelist.xml");
        }
    }
}