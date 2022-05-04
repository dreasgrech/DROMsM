using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace DROMsM
{
    /// <summary>
    /// I started writing this XML parses using the .NET parsing tools
    /// because UNFORTUNATELY U8XmlParser does not support external doctypes!
    /// https://github.com/ikorin24/U8XmlParser/issues/26
    ///
    /// Update: This class is no longer needed because I am now skipping reading external doctypes in U8XmlParser.
    /// </summary>
    public class XmlReaderDATFileHandler : IDATFileHandler
    {
        public DATFile ParseDATFile(string filePath, out HashSet<DATFileMachineField> usedFields)
        {
            usedFields = new HashSet<DATFileMachineField>();

            var datFile = new DATFile();

            XmlReaderSettings settings = new XmlReaderSettings();

            // SET THE RESOLVER
            settings.XmlResolver = new XmlUrlResolver();

            settings.ValidationType = ValidationType.DTD;
            settings.DtdProcessing = DtdProcessing.Parse;
            // settings.DtdProcessing = DtdProcessing.Ignore;
            // settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.DocumentType)
                    {

                    }

                        if (reader.IsStartElement())
                    {
                        //if (reader.Name == "row" && reader.GetAttribute("Reputation") == "1")
                        //{
                        //    singleRepRowIds.Add(reader.GetAttribute("Id"));
                        //}
                    }
                }
            }
            return datFile;
        }
    }
}