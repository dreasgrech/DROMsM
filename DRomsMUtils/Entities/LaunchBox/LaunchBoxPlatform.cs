namespace DROMsM.Forms
{
    /// <summary>
    /// Fields are used in this class instead of properties for performance reasons
    /// </summary>
    public class LaunchBoxPlatform
    {
        public string Name; // { get; set; }
        public string FilePath; // { get; set; }
        public bool Visible; // { get; set; }

        public override string ToString()
        {
            return $"{Name} {FilePath}";
        }
    }
}