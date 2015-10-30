using System.Configuration;
using System.Xml;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Logging.Configuration;

namespace Framework.Logging.Test
{
    public class StandaloneConfigurationReader : IConfigurationReader, ILoggerAdapter
    {
        private string xmlString;

        public StandaloneConfigurationReader()
        {
        }

        public StandaloneConfigurationReader(string xmlString)
        {
            XmlString = xmlString;
        }

        public string XmlString
        {
            get { return xmlString; }
            set { xmlString = value; }
        }

        public object GetSection(string sectionName)
        {
            ConfigurationSectionHandler handler = new ConfigurationSectionHandler();
            return handler.Create(null, null, BuildConfigurationSection(XmlString));
        }

        private static XmlNode BuildConfigurationSection(string xml)
        {
            var doc = new ConfigXmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        public ILogger GetLogger(System.Type type)
        {
            throw new System.NotImplementedException();
        }

        public ILogger GetLogger(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}