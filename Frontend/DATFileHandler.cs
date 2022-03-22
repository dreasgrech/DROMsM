using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                    datFile.Build = NormalizeText(buildAttribute.Value.ToString());
                }

                foreach (var machineNode in rootNode.Children)
                {
                    var datFileMachine = new DATFileMachine();
                    if (machineNode.TryFindAttribute("name", out var nameAttribute))
                    {
                        datFileMachine.Name = NormalizeText(nameAttribute.Value.ToString());
                    }

                    if (machineNode.TryFindChild("description", out var descriptionNode))
                    {
                        datFileMachine.Description = NormalizeText(descriptionNode.InnerText.ToString());
                    }

                    if (machineNode.TryFindChild("year", out var yearNode))
                    {
                        datFileMachine.Year = NormalizeText(yearNode.InnerText.ToString());
                    }

                    if (machineNode.TryFindChild("manufacturer", out var manufacturerNode))
                    {
                        datFileMachine.Manufacturer = NormalizeText(manufacturerNode.InnerText.ToString());
                    }

                    if (machineNode.TryFindChild("driver", out var driverNode))
                    {
                        if (driverNode.TryFindAttribute("status", out var statusAttribute))
                        {
                            datFileMachine.Status = NormalizeText(statusAttribute.Value.ToString());
                        }

                        if (driverNode.TryFindAttribute("emulation", out var emulationAttribute))
                        {
                            datFileMachine.Emulation = NormalizeText(emulationAttribute.Value.ToString());
                        }

                        if (driverNode.TryFindAttribute("savestate", out var saveStateAttribute))
                        {
                            datFileMachine.SaveStates = NormalizeText(saveStateAttribute.Value.ToString());
                        }
                    }

                    if (machineNode.TryFindChild("input", out var inputNode))
                    {
                        if (inputNode.TryFindAttribute("players", out var playersAttribute))
                        {
                            datFileMachine.Players = NormalizeText(playersAttribute.Value.ToString());
                        }

                        if (inputNode.TryFindAttribute("coins", out var coinsAttribute))
                        {
                            datFileMachine.Coins = NormalizeText(coinsAttribute.Value.ToString());
                        }

                        var controlTypesList = new List<string>();
                        var inputNodeChildren = inputNode.Children;
                        foreach (var inputNodeChild in inputNodeChildren)
                        {
                            if (inputNodeChild.Name == "control")
                            {
                                if (inputNodeChild.TryFindAttribute("type", out var controlTypeAttribute))
                                {
                                    controlTypesList.Add(controlTypeAttribute.Value.ToString());
                                }
                            }
                        }

                        datFileMachine.Controls = string.Join(" ", controlTypesList);
                    }

                    datFile.AddMachine(datFileMachine);
                }
            }

            return datFile;
        }

        private static string NormalizeText(string text)
        {
            return ReplaceHTMLEncoding(text);
        }

        private static string ReplaceHTMLEncoding(string text)
        {
            return System.Web.HttpUtility.HtmlDecode(text);
        }
    }
}