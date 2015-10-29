using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using ZSharp.Framework.Logging.Simple;

namespace ZSharp.Framework.Logging.Configuration 
{
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        private static readonly string LOGGERADAPTER_ELEMENT = "loggerAdapter";
        private static readonly string LOGGERADAPTER_ELEMENT_TYPE_ATTRIB = "type";
        private static readonly string ARGUMENT_ELEMENT = "arg";
        private static readonly string ARGUMENT_ELEMENT_KEY_ATTRIB = "key";
        private static readonly string ARGUMENT_ELEMENT_VALUE_ATTRIB = "value";

        public object Create(object parent, object configContext, XmlNode section)
        {
            return Create(parent as LogSetting, configContext, section);
        }

        private LogSetting Create(LogSetting parent, object configContext, XmlNode section)
        {
            if (parent != null)
            {
                throw new ConfigurationException("parent configuration sections are not allowed");
            }

            int logElementsCount = section.SelectNodes(LOGGERADAPTER_ELEMENT).Count;

            if (logElementsCount > 1)
            {
                throw new ConfigurationException("Only one <loggerAdapter> element allowed");
            }
            else if (logElementsCount == 1)
            {
                return ReadConfiguration(section);
            }
            else
            {
                return null;
            }
        }

        private LogSetting ReadConfiguration(XmlNode section)
        {
            var logElement = section.SelectSingleNode(LOGGERADAPTER_ELEMENT);
            Type factoryType = GetLoggerAdapter(logElement);
            NameValueCollection properties = GetNameValueCollection(logElement);
            return new LogSetting(factoryType, properties);
        }

        private  NameValueCollection GetNameValueCollection(XmlNode logElement)
        {
            var propertyNodes = logElement.SelectNodes(ARGUMENT_ELEMENT);
            var properties = new NameValueCollection();

            foreach (XmlNode pNode in propertyNodes)
            {
                string key = string.Empty;
                string itsValue = string.Empty;

                var keyAttrib = pNode.Attributes[ARGUMENT_ELEMENT_KEY_ATTRIB];
                var valueAttrib = pNode.Attributes[ARGUMENT_ELEMENT_VALUE_ATTRIB];

                if (keyAttrib == null)
                {
                    var errMsg = "Required Attribute '"
                        + ARGUMENT_ELEMENT_KEY_ATTRIB
                        + "' not found in element '"
                        + ARGUMENT_ELEMENT
                        + "'";
                    throw new ConfigurationException(errMsg);
                }
                else
                {
                    key = keyAttrib.Value;
                }

                if (valueAttrib != null)
                {
                    itsValue = valueAttrib.Value;
                }

                properties.Add(key, itsValue);
            }
            return properties;
        }

        private  Type GetLoggerAdapter(XmlNode logElement)
        {
            string typeString = string.Empty;
            if (logElement.Attributes[LOGGERADAPTER_ELEMENT_TYPE_ATTRIB] != null)
            {
                typeString = logElement.Attributes[LOGGERADAPTER_ELEMENT_TYPE_ATTRIB].Value;
            }

            if (typeString == string.Empty)
            {
                var errorMsg = "Required Attribute '"
                    + LOGGERADAPTER_ELEMENT_TYPE_ATTRIB
                    + "' not found in element '"
                    + LOGGERADAPTER_ELEMENT
                    + "'";
                throw new ConfigurationException(errorMsg);
            }

            Type factoryType = null;
            try
            {
                if (String.Compare(typeString, "CONSOLE", true) == 0)
                {
                    factoryType = typeof(ConsoleOutLoggerAdapter);
                }
                else if (String.Compare(typeString, "TRACE", true) == 0)
                {
                    factoryType = typeof(TraceLoggerAdapter);
                }
                else if (String.Compare(typeString, "NOOP", true) == 0)
                {
                    factoryType = typeof(NoOpLoggerAdapter);
                }
                else
                {
                    factoryType = Type.GetType(typeString, true, false);
                } 
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Unable to create type '" + typeString + "'", e);
            }
            return factoryType;
        }
    }
}
