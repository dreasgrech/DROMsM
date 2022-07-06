// #define USE_STRINGS

public enum DATFileMachineScreenOrientation
{
    Unknown,
    Horizontal,
    Vertical
}

/// <summary>
/// Fields are used in this class instead of properties for performance reasons
/// </summary>
public class DATFileMachine
{
    [CsvHelper.Configuration.Attributes.Name("Set")]
    public string Name; // { get; set; }

    [CsvHelper.Configuration.Attributes.Name("Name")]
    public string Description; // { get; set; }

    public string Year; // { get; set; }
    public string Manufacturer; // { get; set; }
    public string Status; // { get; set; }
    public string Emulation; // { get; set; }
    public string SaveStates; // { get; set; }
    public string Players; // { get; set; }
    public string Coins; // { get; set; }
    public string Controls; // { get; set; }
    public string ScreenType; // { get; set; }
    public DATFileMachineScreenOrientation ScreenOrientation; // { get; set; }
    public string ScreenRefreshRate; // { get; set; }
#if USE_STRINGS
    public string IsBIOS; // { get; set; }
#else
    public bool IsBIOS; // { get; set; }
#endif
#if USE_STRINGS
    public string IsClone; // { get; set; }
#else
    public bool IsClone; // { get; set; }
#endif
#if USE_STRINGS
    public string IsMechanical; // { get; set; }
#else
    public bool IsMechanical; // { get; set; }
#endif
#if USE_STRINGS
    public string RequireCHDs; // { get; set; }
#else
    public bool RequireCHDs; // { get; set; }
#endif
#if USE_STRINGS
    public string RequireSamples; // { get; set; } 
#else
    public bool RequireSamples; // { get; set; } 
#endif
#if USE_STRINGS
    public string IsDevice; // { get; set; }
#else
    public bool IsDevice; // { get; set; }
#endif

    public int MAMESortingIndex; // { get; set; }
    public string XMLValue; // { get; set; }

    public override string ToString()
    {
        return $"{Name} {Description}";
    }
}