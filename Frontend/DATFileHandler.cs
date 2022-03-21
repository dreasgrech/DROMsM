using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
//using System.Xml;
//using System.Xml.Linq;
using U8Xml;
//using XmlAttribute = System.Xml.XmlAttribute;
//using XmlNode = System.Xml.XmlNode;

namespace Frontend
{
    public class DATFileHandler
    {
        public DATFile ParseDATFile(string filePath)
        {
            var datFile = new DATFile();

            using (var xml = XmlParser.ParseFile(filePath))
            {
                var rootNode = xml.Root;

                foreach (var machineNode in rootNode.Children)
                {
                    var datFileMachine = new DATFileMachine();
                    if (machineNode.TryFindAttribute("name", out var nameAttributeNode))
                    {
                        datFileMachine.Name = nameAttributeNode.Value.ToString();
                    }

                    if (machineNode.TryFindChild("description", out var descriptionNode))
                    {
                        datFileMachine.Description = descriptionNode.InnerText.ToString();
                    }

                    if (machineNode.TryFindChild("year", out var yearNode))
                    {
                        if (int.TryParse(yearNode.InnerText.ToString(), out var yearValue))
                        {
                            datFileMachine.Year = yearValue;
                        }
                    }

                    if (machineNode.TryFindChild("manufacturer", out var manufacturerNode))
                    {
                        datFileMachine.Manufacturer = manufacturerNode.InnerText.ToString();
                    }

                    datFile.AddMachine(datFileMachine);
                }
            }

            return datFile;
        }
    }
}

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

public class DATFile
{
    private List<DATFileMachine> Machines { get; }

    public DATFile()
    {
        Machines = new List<DATFileMachine>();
    }

    public void AddMachine(DATFileMachine machine)
    {
        Machines.Add(machine);
    }

    public List<DATFileMachine>.Enumerator GetMachinesEnumerator()
    {
        return Machines.GetEnumerator();
    }
}