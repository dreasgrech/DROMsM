using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

            var datFileMachineCollection_threaded = new ConcurrentBag<DATFileMachine>();

            using (var xml = XmlParser.ParseFile(filePath))
            {
                var rootNode = xml.Root;
                if (rootNode.TryFindAttribute("build", out var buildAttribute))
                {
                    datFile.Build = NormalizeText(buildAttribute.Value.ToString());
                }

                var machineNodeChildren = rootNode.Children;
                //var machineNodeChildrenCount = machineNodeChildren.Count;
                //Parallel.For(0, machineNodeChildrenCount, i =>
                //{
                //    var machineNode = machineNodeChildren.

                //});

                Parallel.ForEach(machineNodeChildren, (machineNode, parallelLoopState, index) =>
                // foreach (var machineNode in rootNode.Children)
                {
                    var datFileMachine = new DATFileMachine();
                    datFileMachine.MAMESortingIndex = (int) index;

                    if (machineNode.TryFindAttribute("name", out var nameAttribute))
                    {
                        datFileMachine.Name = NormalizeText(nameAttribute.Value.ToString());
                    }

                    var machineNodeChildNodes = machineNode.Children;
                    foreach (var machineNodeChildNode in machineNodeChildNodes)
                    {
                        switch (machineNodeChildNode.Name.ToString())
                        {
                            case "description":
                                datFileMachine.Description = NormalizeText(machineNodeChildNode.InnerText.ToString());
                                break;
                            case "year":
                                datFileMachine.Year = NormalizeText(machineNodeChildNode.InnerText.ToString());
                                break;
                            case "manufacturer":
                                datFileMachine.Manufacturer = NormalizeText(machineNodeChildNode.InnerText.ToString());
                                break;
                            case "driver":
                            {
                                if (machineNodeChildNode.TryFindAttribute("status", out var statusAttribute))
                                {
                                    datFileMachine.Status = NormalizeText(statusAttribute.Value.ToString());
                                }

                                if (machineNodeChildNode.TryFindAttribute("emulation", out var emulationAttribute))
                                {
                                    datFileMachine.Emulation = NormalizeText(emulationAttribute.Value.ToString());
                                }

                                if (machineNodeChildNode.TryFindAttribute("savestate", out var saveStateAttribute))
                                {
                                    datFileMachine.SaveStates = NormalizeText(saveStateAttribute.Value.ToString());
                                }
                            }
                                break;
                            case "input":
                            {
                                if (machineNodeChildNode.TryFindAttribute("players", out var playersAttribute))
                                {
                                    datFileMachine.Players = NormalizeText(playersAttribute.Value.ToString());
                                }

                                if (machineNodeChildNode.TryFindAttribute("coins", out var coinsAttribute))
                                {
                                    datFileMachine.Coins = NormalizeText(coinsAttribute.Value.ToString());
                                }

                                var controlTypesList = new List<string>();
                                var inputNodeChildren = machineNodeChildNode.Children;
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
                                break;
                            case "dipswitch":
                            {

                            }
                                break;
                        }
                    }

                    // datFile.AddMachine(datFileMachine);
                    datFileMachineCollection_threaded.Add(datFileMachine);
                // }
                });
            }

            foreach (var datFileMachine in datFileMachineCollection_threaded)
            {
                 datFile.AddMachine(datFileMachine);
            }
            
            datFile.SortMachines();

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