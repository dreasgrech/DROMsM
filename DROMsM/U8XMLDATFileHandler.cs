using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRomsMUtils;
using U8Xml;
//using System.Xml;
//using System.Xml.Linq;

//using XmlAttribute = System.Xml.XmlAttribute;
//using XmlNode = System.Xml.XmlNode;

namespace DROMsM
{
    public enum DATFileMachineField
    {
        Name,
        Description,
        Year,
        Manufacturer,
        Status,
        Emulation,
        SaveStates,
        Players,
        Coins,
        Controls,
        ScreenType,
        ScreenOrientation,
        ScreenRefreshRate,
        IsBIOS,
        IsClone,
        IsMechanical,
        RequireCHDs,
        RequireSamples,
        IsDevice,
    }

    public class U8XMLDATFileHandler : IDATFileHandler
    {
        private const string TrueBooleanValue = "Yes";
        private const string FalseBooleanValue = "No";

        private static readonly DATFileMachineField[] datFileMachineFieldValues = Enum.GetValues(typeof(DATFileMachineField)).Cast<DATFileMachineField>().ToArray();

        public DATFile ParseDATFile(string filePath, out HashSet<DATFileMachineField> usedFields)
        {
            var datFile = new DATFile();

            var usedFieldsCollection = new bool[datFileMachineFieldValues.Length];

            var datFileMachineCollection_threaded = new ConcurrentBag<DATFileMachine>();

            /*
             * This is a workaround I wrote to handle the issue of U8XmlParser not currently being able to handle external doctypes.
             *
             * Here I am trying to parse the XML file normally, and if that fails, I assume that an external doctype is being used
             * so I then try to parse it by ignoring the external doctype.
             */
            bool tryExternalDocType = false;
            XmlObject xml = null;
            try
            {
                XmlParser.containsExternalDocType = false;
                xml = XmlParser.ParseFile(filePath);
            }
            catch (FormatException)
            {
                tryExternalDocType = true;
            }

            if (tryExternalDocType)
            {
                XmlParser.containsExternalDocType = true;
                xml = XmlParser.ParseFile(filePath);
            }

            if (xml.Declaration.TryGetValue(out var xmlDeclaration))
            {
                datFile.XMLDeclaration = xmlDeclaration.ToString();
            }

            if (xml.DocumentType.TryGetValue(out var xmlDocType))
            {
                datFile.XMLDocType = xmlDocType.ToString();
            }

            var rootNode = xml.Root;
            datFile.XMLRootNodeName = rootNode.Name.ToString();

            // Save the root node's attributes just in case we need to rewrite the file later on
            var datFileXMLRootNodeAttributes = datFile.XMLRootNodeAttributes;
            var rootNodeAttributes = rootNode.Attributes;
            foreach (var rootNodeAttribute in rootNodeAttributes)
            {
                if (rootNodeAttribute.IsNull)
                {
                    continue;
                }

                datFileXMLRootNodeAttributes[rootNodeAttribute.Name.ToString()] = rootNodeAttribute.Value.ToString();
            }

            if (rootNode.TryFindAttribute("build", out var buildAttribute))
            {
                datFile.Build = NormalizeText(buildAttribute.Value.ToString());
            }

            var machineNodeChildren = rootNode.Children;

            Parallel.ForEach(machineNodeChildren, (machineNode, parallelLoopState, index) =>
            {
                var datFileMachine = new DATFileMachine
                {
                    MAMESortingIndex = (int) index,
                    ScreenOrientation = DATFileMachineScreenOrientation.Horizontal, // By default, set the orientation to Horizontal because any machines that have no reference to orientation are categorized as Horizontal in the MAME emulator
                    RequireSamples = FalseBooleanValue,
                    IsMechanical = FalseBooleanValue,
                    IsClone = FalseBooleanValue,
                    RequireCHDs = FalseBooleanValue,
                    IsBIOS = FalseBooleanValue,
                    // IsDevice = false,
                    IsDevice = FalseBooleanValue,
                    XMLValue = machineNode.AsRawString().ToString(),
                };

                if (machineNode.TryFindAttribute("name", out var nameAttribute))
                {
                    datFileMachine.Name = NormalizeText(nameAttribute.Value.ToString());
                    usedFieldsCollection[(int) DATFileMachineField.Name] = true;
                }

                if (machineNode.TryFindAttribute("isbios", out var isBIOSAttribute))
                {
                    if (string.Equals(isBIOSAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                    {
                        datFileMachine.IsBIOS = TrueBooleanValue;
                        usedFieldsCollection[(int) DATFileMachineField.IsBIOS] = true;
                    }
                }

                if (machineNode.TryFindAttribute("ismechanical", out var isMechanicalAttribute))
                {
                    if (string.Equals(isMechanicalAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                    {
                        datFileMachine.IsMechanical = TrueBooleanValue;
                        usedFieldsCollection[(int) DATFileMachineField.IsMechanical] = true;
                    }
                }

                if (machineNode.TryFindAttribute("cloneof", out var cloneOfAttribute))
                {
                    datFileMachine.IsClone = TrueBooleanValue;
                    usedFieldsCollection[(int) DATFileMachineField.IsClone] = true;
                }

                if (machineNode.TryFindAttribute("isdevice", out var isDeviceAttribute))
                {
                    if (string.Equals(isDeviceAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                    {
                        // datFileMachine.IsDevice = true;
                        datFileMachine.IsDevice = TrueBooleanValue;
                        usedFieldsCollection[(int) DATFileMachineField.IsDevice] = true;
                    }
                }

                if (machineNode.TryFindAttribute("runnable", out var runnableAttribute))
                {
                    if (string.Equals(runnableAttribute.Value.ToString(), "no", StringComparison.OrdinalIgnoreCase))
                    {
                        // datFileMachine.IsDevice = true;
                        datFileMachine.IsDevice = TrueBooleanValue;
                        usedFieldsCollection[(int) DATFileMachineField.IsDevice] = true;
                    }
                }

                var machineNodeChildNodes = machineNode.Children;
                Parallel.ForEach(machineNodeChildNodes, machineNodeChildNode =>
                {
                    switch (machineNodeChildNode.Name.ToString())
                    {
                        case "description":
                            datFileMachine.Description = NormalizeText(machineNodeChildNode.InnerText.ToString());
                            usedFieldsCollection[(int) DATFileMachineField.Description] = true;
                            break;
                        case "year":
                            datFileMachine.Year = NormalizeText(machineNodeChildNode.InnerText.ToString());
                            usedFieldsCollection[(int) DATFileMachineField.Year] = true;
                            break;
                        case "manufacturer":
                            datFileMachine.Manufacturer = NormalizeText(machineNodeChildNode.InnerText.ToString());
                            usedFieldsCollection[(int) DATFileMachineField.Manufacturer] = true;
                            break;
                        case "driver":
                        {
                            HandleDriverNode(ref machineNodeChildNode, datFileMachine, usedFieldsCollection);
                        }
                            break;
                        case "input":
                        {
                            HandleInputNode(ref machineNodeChildNode, datFileMachine, usedFieldsCollection);
                        }
                            break;
                        case "display":
                        {
                            HandleDisplayNode(ref machineNodeChildNode, datFileMachine, usedFieldsCollection);
                        }
                            break;
                        case "disk":
                        {
                            datFileMachine.RequireCHDs = TrueBooleanValue;
                            usedFieldsCollection[(int) DATFileMachineField.RequireCHDs] = true;
                        }
                            break;
                        case "sample":
                        {
                            datFileMachine.RequireSamples = TrueBooleanValue;
                            usedFieldsCollection[(int) DATFileMachineField.RequireSamples] = true;
                        }
                            break;
                        //case "dipswitch":
                        //{
                        //    HandleDipSwitchNode(ref machineNodeChildNode, datFileMachine);
                        //}
                        // break;
                    }

                });

                /*
                // Add the machine to the entries list if it's not considered a DEVICE.
                if (!datFileMachine.IsDevice)
                {
                    datFileMachineCollection_threaded.Add(datFileMachine);
                }
                */
                datFileMachineCollection_threaded.Add(datFileMachine);
            });

            xml.Dispose();

            foreach (var datFileMachine in datFileMachineCollection_threaded)
            {
                datFile.AddMachine(datFileMachine);
            }

            datFile.SortMachines();

            usedFields = new HashSet<DATFileMachineField>(EqualityComparer<DATFileMachineField>.Default);
            // usedFields = new List<DATFileMachineField>();
            var usedFieldsCollectionTotal = (byte) usedFieldsCollection.Length;
            for (byte i = 0; i < usedFieldsCollectionTotal; i++)
            {
                var value = usedFieldsCollection[i];
                if (value)
                {
                    var field = (DATFileMachineField) i;
                    usedFields.Add(field);
                }
            }

            return datFile;
        }

        private static void HandleDisplayNode(ref XmlNode displayNode, DATFileMachine datFileMachine, bool[] usedFieldsCollection)
        {
            if (displayNode.TryFindAttribute("type", out var typeAttribute))
            {
                var typeValue = NormalizeTextAndCapitalizeFirstLetter(typeAttribute.Value.ToString());
                datFileMachine.ScreenType = typeValue;
                usedFieldsCollection[(int) DATFileMachineField.ScreenType] = true;
            }

            if (displayNode.TryFindAttribute("refresh", out var refreshAttribute))
            {
                var refreshValue = refreshAttribute.Value.ToString();
                datFileMachine.ScreenRefreshRate = refreshValue;
                usedFieldsCollection[(int) DATFileMachineField.ScreenRefreshRate] = true;
            }

            HandleRotateAttribute(displayNode, datFileMachine, usedFieldsCollection);
        }

        private static void HandleRotateAttribute(XmlNode displayNode, DATFileMachine datFileMachine, bool[] usedFieldsCollection)
        {
            if (!displayNode.TryFindAttribute("rotate", out var rotateAttribute))
            {
                // datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Unknown;
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Horizontal;
                usedFieldsCollection[(int) DATFileMachineField.ScreenOrientation] = true;
                return;
            }

            var rotateValue = rotateAttribute.Value.ToString();
            if (!int.TryParse(rotateValue, out var rotateNumber))
            {
                // datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Unknown;
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Horizontal;
                usedFieldsCollection[(int) DATFileMachineField.ScreenOrientation] = true;
                return;
            }

            if (rotateNumber == 0 || rotateNumber == 180)
            {
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Horizontal;
                usedFieldsCollection[(int) DATFileMachineField.ScreenOrientation] = true;
                return;
            }

            if (rotateNumber == 90 || rotateNumber == 270)
            {
                datFileMachine.ScreenOrientation = DATFileMachineScreenOrientation.Vertical;
                usedFieldsCollection[(int) DATFileMachineField.ScreenOrientation] = true;
                return;
            }
        }

        private static void HandleInputNode(ref XmlNode machineNodeChildNode, DATFileMachine datFileMachine, bool[] usedFieldsCollection)
        {
            if (machineNodeChildNode.TryFindAttribute("players", out var playersAttribute))
            {
                datFileMachine.Players = NormalizeText(playersAttribute.Value.ToString());
                usedFieldsCollection[(int) DATFileMachineField.Players] = true;
            }

            if (machineNodeChildNode.TryFindAttribute("coins", out var coinsAttribute))
            {
                datFileMachine.Coins = NormalizeText(coinsAttribute.Value.ToString());
                usedFieldsCollection[(int) DATFileMachineField.Coins] = true;
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

                    usedFieldsCollection[(int) DATFileMachineField.Controls] = true;
                }
            }

            datFileMachine.Controls = string.Join(",", controlTypesList);
        }

        /*
        private static void HandleDipSwitchNode(ref XmlNode dipSwitchNode, DATFileMachine datFileMachine, bool[] usedFieldsCollection)
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
                    HandleDipSwitchOrientationNode(dipSwitchNode, datFileMachine, usedFieldsCollection);
                }
                    break;
            }
        }
        */

        /*
        private static void HandleDipSwitchOrientationNode(XmlNode orientationNode, DATFileMachine datFileMachine, bool[] usedFieldsCollection)
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
        */

        private static void HandleDriverNode(ref XmlNode machineNodeChildNode, DATFileMachine datFileMachine, bool[] usedFieldsCollection)
        {
            if (machineNodeChildNode.TryFindAttribute("status", out var statusAttribute))
            {
                datFileMachine.Status = NormalizeTextAndCapitalizeFirstLetter(statusAttribute.Value.ToString());
                usedFieldsCollection[(int) DATFileMachineField.Status] = true;
            }

            if (machineNodeChildNode.TryFindAttribute("emulation", out var emulationAttribute))
            {
                datFileMachine.Emulation = NormalizeTextAndCapitalizeFirstLetter(emulationAttribute.Value.ToString());
                usedFieldsCollection[(int) DATFileMachineField.Emulation] = true;
            }

            if (machineNodeChildNode.TryFindAttribute("savestate", out var saveStateAttribute))
            {
                datFileMachine.SaveStates = NormalizeTextAndCapitalizeFirstLetter(saveStateAttribute.Value.ToString());
                usedFieldsCollection[(int) DATFileMachineField.SaveStates] = true;
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