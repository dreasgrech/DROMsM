
using System.Collections.Generic;

public enum DATFileMachineScreenOrientation
{
    Unknown,
    Horizontal,
    Vertical
}

public class DATFileMachine
{
    [CsvHelper.Configuration.Attributes.Name("Set")]
    public string Name { get; set; }

    [CsvHelper.Configuration.Attributes.Name("Name")]
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
    // public bool IsBIOS { get; set; }
    public string IsBIOS { get; set; }
    // public bool IsClone { get; set; }
    public string IsClone { get; set; }
    // public bool IsMechanical { get; set; }
    public string IsMechanical { get; set; }
    // public bool RequireCHDs { get; set; }
    public string RequireCHDs { get; set; }

    // public bool RequireSamples { get; set; } 
    public string RequireSamples { get; set; } 

    /*
    public bool RequireSamples
    {
        get => booleanValues[DATFileMachineBooleanField.RequireSamples];
        set => SetBooleanField(DATFileMachineBooleanField.RequireSamples, value);
    }

    public string RequireSamples_TextValue => booleanStringValues[DATFileMachineBooleanField.RequireSamples];

    // TODO: This will be too much for every single machine
    private readonly Dictionary<DATFileMachineBooleanField, bool> booleanValues = new Dictionary<DATFileMachineBooleanField, bool>(EqualityComparer<DATFileMachineBooleanField>.Default);
    private readonly Dictionary<DATFileMachineBooleanField, string> booleanStringValues = new Dictionary<DATFileMachineBooleanField, string>(EqualityComparer<DATFileMachineBooleanField>.Default);

    private void SetBooleanField(DATFileMachineBooleanField booleanField, bool value)
    {
        booleanValues[booleanField] = value;
        booleanStringValues[booleanField] = value ? TrueBooleanValue : FalseBooleanValue;
    }

    private const string TrueBooleanValue = "Yes";
    private const string FalseBooleanValue = "No";
    */

    // public string ParentRom { get; set; }

    public int MAMESortingIndex { get; set; }
    public bool IsDevice { get; set; }
    public string XMLValue { get; set; }

    public override string ToString()
    {
        return $"{Name} {Description}";
    }
}

//public enum DATFileMachineBooleanField
//{
//    RequireSamples
//}