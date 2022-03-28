using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DRomsMUtils;
using U8Xml;
//using System.Xml;
//using System.Xml.Linq;

//using XmlAttribute = System.Xml.XmlAttribute;
//using XmlNode = System.Xml.XmlNode;

namespace DROMsM
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

                Parallel.ForEach(machineNodeChildren, (machineNode, parallelLoopState, index) =>
                // foreach (var machineNode in rootNode.Children)
                {
                    var datFileMachine = new DATFileMachine
                    {
                        MAMESortingIndex = (int) index,
                        ScreenOrientation = DATFileMachineScreenOrientation.Horizontal, // By default, set the orientation to Horizontal because any machines that have no reference to orientation are categorized as Horizontal in the MAME emulator
                        IsDevice = false
                    };

                    if (machineNode.TryFindAttribute("name", out var nameAttribute))
                    {
                        datFileMachine.Name = NormalizeText(nameAttribute.Value.ToString());
                    }

                    if (machineNode.TryFindAttribute("isbios", out var isBIOSAttribute))
                    {
                        if (string.Equals(isBIOSAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                        {
                            datFileMachine.IsBIOS = true;
                        }
                    }

                    if (machineNode.TryFindAttribute("ismechanical", out var isMechanicalAttribute))
                    {
                        if (string.Equals(isMechanicalAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                        {
                            datFileMachine.IsMechanical = true;
                        }
                    }

                    if (machineNode.TryFindAttribute("cloneof", out var cloneOfAttribute))
                    {
                        datFileMachine.IsClone = true;
                    }

                    if (machineNode.TryFindAttribute("isdevice", out var isDeviceAttribute))
                    {
                        if (string.Equals(isDeviceAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                        {
                            datFileMachine.IsDevice = true;
                        }
                    }

                    if (machineNode.TryFindAttribute("runnable", out var runnableAttribute))
                    {
                        if (string.Equals(runnableAttribute.Value.ToString(), "no", StringComparison.OrdinalIgnoreCase))
                        {
                            datFileMachine.IsDevice = true;
                        }
                    }

                    var machineNodeChildNodes = machineNode.Children;
                    // foreach (var machineNodeChildNode in machineNodeChildNodes)
                    Parallel.ForEach(machineNodeChildNodes, machineNodeChildNode =>
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
                                HandleDriverNode(ref machineNodeChildNode, datFileMachine);
                            }
                                break;
                            case "input":
                            {
                                HandleInputNode(ref machineNodeChildNode, datFileMachine);
                            }
                                break;
                            case "display":
                            {
                                HandleDisplayNode(ref machineNodeChildNode, datFileMachine);
                            }
                                break;
                            //case "dipswitch":
                            //{
                            //    HandleDipSwitchNode(ref machineNodeChildNode, datFileMachine);
                            //}
                                // break;
                        }

                        // }
                    });

                    // Add the machine to the entries list if it's not considered a DEVICE.
                    if (!datFileMachine.IsDevice)
                    {
                        datFileMachineCollection_threaded.Add(datFileMachine);
                    }
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

        private static void HandleDisplayNode(ref XmlNode displayNode, DATFileMachine datFileMachine)
        {
            if (displayNode.TryFindAttribute("type", out var typeAttribute))
            {
                var typeValue = NormalizeTextAndCapitalizeFirstLetter(typeAttribute.Value.ToString());
                datFileMachine.ScreenType = typeValue;
            }

            if (displayNode.TryFindAttribute("refresh", out var refreshAttribute))
            {
                var refreshValue = refreshAttribute.Value.ToString();
                datFileMachine.ScreenRefreshRate = refreshValue;
            }

            HandleRotateAttribute(displayNode, datFileMachine);
        }

        private static void HandleRotateAttribute(XmlNode displayNode, DATFileMachine datFileMachine)
        {
            if (!displayNode.TryFindAttribute("rotate", out var rotateAttribute))
            {
                // datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Unknown;
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Horizontal;
                return;
            }

            var rotateValue = rotateAttribute.Value.ToString();
            if (!int.TryParse(rotateValue, out var rotateNumber))
            {
                // datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Unknown;
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Horizontal;
                return;
            }

            if (rotateNumber == 0 || rotateNumber == 180)
            {
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Horizontal;
                return;
            }

            if (rotateNumber == 90 || rotateNumber == 270)
            {
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Vertical;
                return;
            }
        }

        private static void HandleInputNode(ref XmlNode machineNodeChildNode, DATFileMachine datFileMachine)
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

            datFileMachine.Controls = string.Join(",", controlTypesList);
        }

        private static void HandleDipSwitchNode(ref XmlNode dipSwitchNode, DATFileMachine datFileMachine)
        {
            if (!dipSwitchNode.TryFindAttribute("name", out var dipSwitchChildNodeNameAttribute))
            {
                return;
            }

            var dipSwitchNodeName = dipSwitchChildNodeNameAttribute.Value.ToString();
            switch (dipSwitchNodeName)
            {
                case "Orientation":
                {
                    HandleDipSwitchOrientationNode(dipSwitchNode, datFileMachine);
                }
                    break;
            }
        }

        private static void HandleDipSwitchOrientationNode(XmlNode orientationNode, DATFileMachine datFileMachine)
        {
            var dipSwitchNodeChildren = orientationNode.Children;
            foreach (var dipSwitchNodeChild in dipSwitchNodeChildren)
            {
                if (!dipSwitchNodeChild.TryFindAttribute("name", out var dipSwitchNodeChildNameAttribute))
                {
                    continue;
                }

                var dipNodeName = dipSwitchNodeChildNameAttribute.Value.ToString();
                switch (dipNodeName)
                {
                    case "Horizontal":
                    {
                        if (!dipSwitchNodeChild.TryFindAttribute("value", out var dipSwitchNodeChildValueAttribute))
                        {
                            continue;
                        }

                        var dipSwitchNodeChildValue = dipSwitchNodeChildValueAttribute.Value.ToString();
                        var x = dipSwitchNodeChildValue;
                    }
                        break;
                }
            }
        }

        private static void HandleDriverNode(ref XmlNode machineNodeChildNode, DATFileMachine datFileMachine)
        {
            if (machineNodeChildNode.TryFindAttribute("status", out var statusAttribute))
            {
                datFileMachine.Status = NormalizeTextAndCapitalizeFirstLetter(statusAttribute.Value.ToString());
            }

            if (machineNodeChildNode.TryFindAttribute("emulation", out var emulationAttribute))
            {
                datFileMachine.Emulation = NormalizeTextAndCapitalizeFirstLetter(emulationAttribute.Value.ToString());
            }

            if (machineNodeChildNode.TryFindAttribute("savestate", out var saveStateAttribute))
            {
                datFileMachine.SaveStates = NormalizeTextAndCapitalizeFirstLetter(saveStateAttribute.Value.ToString());
            }
        }

        private static string NormalizeTextAndCapitalizeFirstLetter(string text)
        {
            var normalizedText = NormalizeText(text);
            normalizedText = StringUtilities.CapitalizeFirstLetter(normalizedText);

            return normalizedText;
        }

        private static string NormalizeText(string text)
        {
            return StringUtilities.ReplaceHTMLEncoding(text);
        }
    }
}