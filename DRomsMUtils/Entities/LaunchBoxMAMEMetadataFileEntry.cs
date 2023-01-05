namespace DROMsM.Forms
{
    /// <summary>
    /// Fields are used in this class instead of properties for performance reasons
    /// </summary>
    public class LaunchBoxMAMEMetadataFileEntry
    {
        public string FileName; // { get; set; }
        public string Name; // { get; set; }
        public string Status; // { get; set; }
        public string Developer; // { get; set; }
        public string Publisher; // { get; set; }
        public string Year; // { get; set; }
        public bool IsMechanical; // { get; set; }
        public bool IsBootleg; // { get; set; }
        public bool IsPrototype; // { get; set; }
        public bool IsHack; // { get; set; }
        public bool IsMature; // { get; set; }
        public bool IsQuiz; // { get; set; }
        public bool IsFruit; // { get; set; }
        public bool IsCasino; // { get; set; }
        public bool IsRhythm; // { get; set; }
        public bool IsTableTop; // { get; set; }
        public bool IsPlayChoice; // { get; set; }
        public bool IsMahjong; // { get; set; }
        public bool IsNonArcade; // { get; set; }
        public string Genre; // { get; set; }
        public string Playmode; // { get; set; }
        public string Language; // { get; set; }
        public string Source; // { get; set; }

        public override string ToString()
        {
            return $"{FileName} {Name} - {Status}";
        }
    }
}