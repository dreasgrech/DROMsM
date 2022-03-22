using System.Collections.Generic;
using Frontend;

public class DATFile
{
    public string Build { get; set; }
    private List<DATFileMachine> Machines { get; }

    public int TotalMachines
    {
        get { return Machines.Count; }
    }

    public DATFile()
    {
        Machines = new List<DATFileMachine>();
    }

    public void AddMachine(DATFileMachine machine)
    {
        Machines.Add(machine);
    }

    public void SortMachines()
    {
        var comparer = new DatFileMachineComparer_ComparisonName();
        Machines.Sort(comparer);
    }

    public List<DATFileMachine>.Enumerator GetMachinesEnumerator()
    {
        return Machines.GetEnumerator();
    }
}