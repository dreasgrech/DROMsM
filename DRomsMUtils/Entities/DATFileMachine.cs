public enum DATFileMachineScreenOrientation
{
    Unknown,
    Horizontal,
    Vertical
}

public class DATFileMachine
{
    public string Name { get; set; }
    public bool IsDevice { get; set; }
    public string Description { get; set; }
    public string Year { get; set; }
    public string Manufacturer { get; set; }
    public string Status { get; set; }
    public string Emulation { get; set; }
    public string SaveStates { get; set; }
    public string Players { get; set; }
    public string Coins { get; set; }
    public string Controls { get; set; }
    public string ScreenType { get; set; }
    public DATFileMachineScreenOrientation ScreenOrientation { get; set; }
    public string ScreenRefreshRate { get; set; }

    public int MAMESortingIndex { get; set; }

    public override string ToString()
    {
        return $"{Name} {Description}";
    }
}