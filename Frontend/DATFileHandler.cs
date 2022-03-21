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
                if (rootNode.TryFindAttribute("build", out var buildAttribute))
                {
                    datFile.Build = buildAttribute.Value.ToString();
                }

                foreach (var machineNode in rootNode.Children)
                {
                    var datFileMachine = new DATFileMachine();
                    if (machineNode.TryFindAttribute("name", out var nameAttribute))
                    {
                        datFileMachine.Name = nameAttribute.Value.ToString();
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