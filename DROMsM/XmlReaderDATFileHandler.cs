using System.Xml;
using System.Xml.Schema;

namespace DROMsM
{
    public class XmlReaderDATFileHandler : IDATFileHandler
    {
        public DATFile ParseDATFile(string filePath)
        {
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