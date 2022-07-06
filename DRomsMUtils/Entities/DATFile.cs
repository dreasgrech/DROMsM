using System.Collections.Generic;
using System.Text;
using Frontend;

public class DATFile
{
    public DATFileHeader Header;// { get; set; }
    public string Build;// { get; set; }
    public List<DATFileMachine> Machines;// { get; }

    public string XMLDeclaration;// { get; set; }
    public string XMLDocType;// { get; set; }
    public string XMLRootNodeName;// { get; set; }
    public Dictionary<string, string> XMLRootNodeAttributes;// { get; set; }

    public int TotalMachines => Machines.Count;

    public DATFile()
    {
        Machines = new List<DATFileMachine>();
        XMLRootNodeAttributes = new Dictionary<string, string>(EqualityComparer<string>.Default);
    }

    public void AddMachine(DATFileMachine machine)
    {
        Machines.Add(machine);
    }

    public void SortMachines()
    {
        // var comparer = new DatFileMachineComparer_ComparisonName();
        var comparer = new DatFileMachineComparer_MAMEIndex();
        Machines.Sort(comparer);
    }

    public string GetRootNodeXMLOpeningTag()
    {
        var fullStringBuilder = new StringBuilder();
        fullStringBuilder.Append($"<{XMLRootNodeName} ");
        using (var attributesEnumerator = XMLRootNodeAttributes.GetEnumerator())
        {
            while (attributesEnumerator.MoveNext())
            {
                var current = attributesEnumerator.Current;
                fullStringBuilder.Append($" {current.Key}=\"{current.Value}\"");
            }
        }

        fullStringBuilder.Append(">");
        return fullStringBuilder.ToString();
    }

    public string GetRootNodeXMLClosingTag()
    {
        return $"</{XMLRootNodeName}>";
    }

    public List<DATFileMachine>.Enumerator GetMachinesEnumerator()
    {
        return Machines.GetEnumerator();
    }
}