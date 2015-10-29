using System;
using System.Diagnostics;
using ZSharp.Framework.Logging.Configuration;
using ZSharp.Framework.Logging.Simple;

namespace ZSharp.Framework.Logging
{
    public class LogManager
    {
        public static string LOGGING_SECTION { get { return "logging"; } }
        private static IConfigurationReader configurationReader;
        private static ILoggerAdapter adapter;
        private static readonly object lockObj = new object();

        static LogManager() 
        {
            Reset();
        }

        public static void Reset()
        {
            Reset(new DefaultConfigurationReader());
        }

        public static void Reset(IConfigurationReader reader)
        {
            lock (lockObj)
            {
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }
                configurationReader = reader;
                adapter = null;
            }
        }

        public static IConfigurationReader ConfigurationReader
        {
            get
            {
                return configurationReader;
            }
        }

        public static ILogger GetLogger<T>()
        {
            return Adapter.GetLogger(typeof(T));
        }

        public static ILogger GetLogger(Type type)
        {
            return Adapter.GetLogger(type);
        }

        public static ILogger GetLogger(string key)
        {
            return Adapter.GetLogger(key);
        }

        public static ILoggerAdapter Adapter
        {
            get
            {
                if (adapter == null)
                {
                    lock (lockObj)
                    {
                        if (adapter == null)
                        {
                            adapter = BuildLoggerAdapter();
                        }
                    }
                }
                return adapter;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Adapter");
                }

                lock (lockObj)
                {
                    adapter = value;
                }
            }
        }

        private static ILoggerAdapter BuildLoggerAdapter()
        {
            object sectionResult = ConfigurationReader.GetSection(LOGGING_SECTION);

            if (sectionResult == null)
            {
                return GetDefaultFactory();
            }

            if (sectionResult is ILoggerAdapter)
            {
                var traceMsg = string.Format(
                    "Using ILoggerAdapter returned from custom ConfigurationReader '{0}'",
                    ConfigurationReader.GetType().FullName);
                Trace.WriteLine(traceMsg);
                return (ILoggerAdapter)sectionResult;
            }

            return BuildLoggerAdapterFromLogSettings(sectionResult);
        }

        private static ILoggerAdapter GetDefaultFactory()
        {
            string message = string.Empty;
            if (ConfigurationReader.GetType() == typeof(DefaultConfigurationReader))
            {
                message = string.Format(
                    "no configuration section <{0}> found - suppressing logging output",
                    LOGGING_SECTION);
            }
            else
            {
                message = string.Format(
                    "Custom ConfigurationReader '{0}' returned <null> - suppressing logging output",
                    ConfigurationReader.GetType().FullName);
            }
            Trace.WriteLine(message);

            ILoggerAdapter defaultFactory = new NoOpLoggerAdapter();
            return defaultFactory;
        }

        private static ILoggerAdapter BuildLoggerAdapterFromLogSettings(object sectionResult)
        {
            ILoggerAdapter adapter = null;
            if (sectionResult is LogSetting)
            {
                var setting = (LogSetting)sectionResult;
                try
                {
                    if (setting.Properties != null && setting.Properties.Count > 0)
                    {
                        object[] args = { setting.Properties };
                        adapter = (ILoggerAdapter)Activator.CreateInstance(setting.FactoryAdapterType, args);
                    }
                    else
                    {
                        adapter = (ILoggerAdapter)Activator.CreateInstance(setting.FactoryAdapterType);
                    }
                }
                catch (Exception ex)
                {
                    var errorMsg = string.Format(
                        @"Unable to create instance of type {0}.Possible explanation is lack of zero arg and single arg Common.Logging.Configuration.NameValueCollection constructors.",
                        setting.FactoryAdapterType.FullName);
                    throw new ConfigurationException(errorMsg, ex);
                }
            }
            else
            {
                var msg = string.Format(
                    "ConfigurationReader {0} returned unknown settings instance of type {1}",
                    ConfigurationReader.GetType().FullName,
                    sectionResult.GetType().FullName
                );
                throw new ConfigurationException(msg);
            }
            return adapter;
        }
    }
}
