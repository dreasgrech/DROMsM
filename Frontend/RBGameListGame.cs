namespace Frontend
{
    public class RBGameListGame
    {
        public string Path { get; set; }
        public string Filename { get; set; }
        public string Hash { get; set; }
        public string LastPlayed { get; set; }
        public int PlayCount { get; set; }
        public int Region { get; set; }
        public int GenreID { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string ReleaseDate { get; set; }
        public string Video { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}