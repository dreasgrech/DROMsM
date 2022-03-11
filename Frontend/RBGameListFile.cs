using System.Collections.Generic;

namespace Frontend
{
    public class RBGameListFile
    {
        public string FilePath { get; set; }
        public string FileBaseDirectoryPath { get; set; }
        public List<RBGameListFolder> Folders { get; }
        public List<RBGameListGame> Games { get; }

        private readonly Dictionary<string, RBGameListGame> filenameGameMap;

        public RBGameListFile()
        {
            Folders = new List<RBGameListFolder>();
            Games = new List<RBGameListGame>();
            filenameGameMap = new Dictionary<string, RBGameListGame>(/* icompa */);
        }

        public void AddFolder(RBGameListFolder folder)
        {
            Folders.Add(folder);
        }

        public void AddGame(RBGameListGame game)
        {
            Games.Add(game);
            // filenameGameMap[game.Path] = game;
            filenameGameMap[game.Filename] = game;
        }

        public RBGameListGame ResolveGame(string filePath)
        {
            // return filenameGameMap.TryGetValue(filePath, out var gameListGame) ? gameListGame : null;
            if (filenameGameMap.TryGetValue(filePath, out var gameListGame))
            {
                // Logger.Log($"Found entry in gamelist.xml: {filePath}");
                return gameListGame;
            }

            Logger.Log($"Unable to find entry in gamelist.xml: {filePath}");
            return null;
        }
    }
}