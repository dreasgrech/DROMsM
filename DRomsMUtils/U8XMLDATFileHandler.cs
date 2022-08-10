// #define USE_STRINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRomsMUtils;
using U8Xml;

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
        JoystickWay,
        ScreenType,
        ScreenOrientation,
        ScreenRefreshRate,
        ScreenWidth,
        ScreenHeight,
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

            var xml = XmlParser.ParseFile(filePath);

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
            var machineNodeChildrenCount = machineNodeChildren.Count;
            var machineNodeChildrenEnumerable = (IEnumerable<XmlNode>) machineNodeChildren;
            if (machineNodeChildrenCount > 0)
            {
                // Some dat files use the first node as a header
                var firstMachineNode = machineNodeChildrenEnumerable.First();
                if (string.Equals(firstMachineNode.Name.ToString(), "header", StringComparison.OrdinalIgnoreCase))
                {
                    var datFileHeader = new DATFileHeader();
                    var firstMachineNodeChildren = firstMachineNode.Children;
                    foreach (var firstMachineNodeChild in firstMachineNodeChildren)
                    {
                        var firstMachineNodeChildName = firstMachineNodeChild.Name.ToString().ToLowerInvariant();
                        var firstMachineNodeChildValue = firstMachineNodeChild.InnerText.ToString();

                        switch (firstMachineNodeChildName)
                        {
                            case "name": { datFileHeader.Name = firstMachineNodeChildValue; break; }
                            case "description": { datFileHeader.Description = firstMachineNodeChildValue; break; }
                            case "version": { datFileHeader.Version = firstMachineNodeChildValue; break; }
                            case "author": { datFileHeader.Author = firstMachineNodeChildValue; break; }
                            case "homepage": { datFileHeader.Homepage = firstMachineNodeChildValue; break; }
                            case "url": { datFileHeader.URL = firstMachineNodeChildValue; break; }
                        }
                    }

                    // Save the header XML node 
                    datFileHeader.XMLValue = firstMachineNode.AsRawString().ToString();

                    datFile.Header = datFileHeader;


                    // Since we found a <Version> in a <Header>, use that as our build version
                    datFile.Build = datFileHeader.Version;

                    // Skip the first node since we've now already processed it and determined it's a header node
                    machineNodeChildrenEnumerable = machineNodeChildrenEnumerable.Skip(1);
                }
            }

            var datFileMachineCollection_threaded = new ConcurrentQueue<DATFileMachine>();

            Parallel.ForEach(machineNodeChildrenEnumerable, (machineNode, parallelLoopState, index) =>
            {
                var datFileMachine = new DATFileMachine
                {
                    MAMESortingIndex = (int) index,
                    ScreenOrientation = DATFileMachineScreenOrientation.Horizontal, // By default, set the orientation to Horizontal because any machines that have no reference to orientation are categorized as Horizontal in the MAME emulator
#if USE_STRINGS
                    RequireSamples = FalseBooleanValue,
#else
                    RequireSamples = false,
#endif
#if USE_STRINGS
                    IsMechanical = FalseBooleanValue,
#else
                    IsMechanical = false,
#endif
#if USE_STRINGS
                    IsClone = FalseBooleanValue,
#else
                    IsClone = false,
#endif
#if USE_STRINGS
                    RequireCHDs = FalseBooleanValue,
#else
                    RequireCHDs = false,
#endif
#if USE_STRINGS
                    IsBIOS = FalseBooleanValue,
#else
                    IsBIOS = false,
#endif
#if USE_STRINGS
                    IsDevice = FalseBooleanValue,
#else
                    IsDevice = false,
#endif
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
#if USE_STRINGS
                        datFileMachine.IsBIOS = TrueBooleanValue;
#else
                        datFileMachine.IsBIOS = true;
#endif
                        usedFieldsCollection[(int) DATFileMachineField.IsBIOS] = true;
                    }
                }

                if (machineNode.TryFindAttribute("ismechanical", out var isMechanicalAttribute))
                {
                    if (string.Equals(isMechanicalAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                    {
#if USE_STRINGS
                        datFileMachine.IsMechanical = TrueBooleanValue;
#else
                        datFileMachine.IsMechanical = true;
#endif
                        usedFieldsCollection[(int) DATFileMachineField.IsMechanical] = true;
                    }
                }

                if (machineNode.TryFindAttribute("cloneof", out var cloneOfAttribute))
                {
#if USE_STRINGS
                    datFileMachine.IsClone = TrueBooleanValue;
#else
                    datFileMachine.IsClone = true;
#endif
                    usedFieldsCollection[(int) DATFileMachineField.IsClone] = true;
                }

                if (machineNode.TryFindAttribute("isdevice", out var isDeviceAttribute))
                {
                    if (string.Equals(isDeviceAttribute.Value.ToString(), "yes", StringComparison.OrdinalIgnoreCase))
                    {
#if USE_STRINGS
                        datFileMachine.IsDevice = TrueBooleanValue;
#else
                        datFileMachine.IsDevice = true;
#endif
                        usedFieldsCollection[(int) DATFileMachineField.IsDevice] = true;
                    }
                }

                if (machineNode.TryFindAttribute("runnable", out var runnableAttribute))
                {
                    if (string.Equals(runnableAttribute.Value.ToString(), "no", StringComparison.OrdinalIgnoreCase))
                    {
#if USE_STRINGS
                        datFileMachine.IsDevice = TrueBooleanValue;
#else
                        datFileMachine.IsDevice = true;
#endif
                        usedFieldsCollection[(int) DATFileMachineField.IsDevice] = true;
                    }
                }

                var machineNodeChildNodes = machineNode.Children;
                Parallel.ForEach(machineNodeChildNodes, machineNodeChildNode =>
                {
                    var machineNodeChildNodeName = machineNodeChildNode.Name.ToString().ToLowerInvariant();
                    switch (machineNodeChildNodeName)
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
#if USE_STRINGS
                            datFileMachine.RequireCHDs = TrueBooleanValue;
#else
                            datFileMachine.RequireCHDs = true;
#endif
                            usedFieldsCollection[(int) DATFileMachineField.RequireCHDs] = true;
                        }
                            break;
                        case "sample":
                        {
#if USE_STRINGS
                            datFileMachine.RequireSamples = TrueBooleanValue;
#else
                            datFileMachine.RequireSamples = true;
#endif
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

                datFileMachineCollection_threaded.Enqueue(datFileMachine);
            });

            xml.Dispose();

            foreach (var datFileMachine in datFileMachineCollection_threaded)
            {
                datFile.AddMachine(datFileMachine);
            }

            datFile.SortMachines();

            usedFields = new HashSet<DATFileMachineField>(EqualityComparer<DATFileMachineField>.Default);
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
                var value = NormalizeTextAndCapitalizeFirstLetter(typeAttribute.Value.ToString());
                datFileMachine.ScreenType = value;
                usedFieldsCollection[(int) DATFileMachineField.ScreenType] = true;
            }

            if (displayNode.TryFindAttribute("refresh", out var refreshAttribute))
            {
                var value = refreshAttribute.Value.ToString();
                datFileMachine.ScreenRefreshRate = value;
                usedFieldsCollection[(int) DATFileMachineField.ScreenRefreshRate] = true;
            }

            if (displayNode.TryFindAttribute("width", out var widthAttribute))
            {
                var value = widthAttribute.Value.ToString();
                // datFileMachine.ScreenWidth = value;
                datFileMachine.ScreenWidth = int.Parse(value);
                usedFieldsCollection[(int) DATFileMachineField.ScreenWidth] = true;
            }

            if (displayNode.TryFindAttribute("height", out var heightAttribute))
            {
                var value = heightAttribute.Value.ToString();
                // datFileMachine.ScreenHeight = value;
                datFileMachine.ScreenHeight = int.Parse(value);
                usedFieldsCollection[(int) DATFileMachineField.ScreenHeight] = true;
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

            // Parse the controls
            var controlTypesList = new SortedSet<string>(); // Using a SortedSet so that we don't show duplicate controls and also to have them sorted
            var controlJoystickWaysList = new SortedSet<string>(); // Using a SortedSet so that we don't show duplicate controls and also to have them sorted
            var inputNodeChildren = machineNodeChildNode.Children;
            foreach (var inputNodeChild in inputNodeChildren)
            {
                if (inputNodeChild.Name == "control")
                {
                    if (inputNodeChild.TryFindAttribute("type", out var controlTypeAttribute))
                    {
                        controlTypesList.Add(controlTypeAttribute.Value.ToString());

                        usedFieldsCollection[(int) DATFileMachineField.Controls] = true;
                    }

                    if (inputNodeChild.TryFindAttribute("ways", out var controlJoystickWayAttribute))
                    {
                        controlJoystickWaysList.Add(controlJoystickWayAttribute.Value.ToString());

                        usedFieldsCollection[(int) DATFileMachineField.JoystickWay] = true;
                    }

                }
            }

            datFileMachine.Controls = string.Join(",", controlTypesList);
            datFileMachine.JoystickWays = string.Join(",", controlJoystickWaysList);
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