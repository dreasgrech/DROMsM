public class DATFileMachine
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public string Manufacturer { get; set; }

    public override string ToString()
    {
        return $"{Name} {Description}";
    }
}