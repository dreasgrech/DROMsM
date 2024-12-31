namespace Frontend
{
    public class RBGameListFolder
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}