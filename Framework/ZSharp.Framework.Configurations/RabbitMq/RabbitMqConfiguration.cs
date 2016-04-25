using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Configurations
{
    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {
        #region IRabbitMqConfiguration

        public string VirtualHost { get; private set; }

        public string UserName { get; set; }

        public string Password { get; private set; }

        public ushort RequestedHeartbeat { get; private set; }

        public ushort PrefetchCount { get; private set; }

        public ushort Timeout { get; private set; }

        public RabbitMqClientPropertyCollection ClientPropertyCollection { get; }

        public IDictionary<string, object> ClientProperties { get; private set; }

        public RabbitMqHostCollection RabbitMqHosts { get; private set; }   

        public bool PublisherConfirms { get; private set; }

        public bool PersistentMessages { get; private set; }

        public bool CancelOnHaFailover { get; private set; }

        public bool Mandatory { get; set; }

        public bool UseBackgroundThreads { get; private set; }

        #endregion        

        public RabbitMqConfiguration(string rabbitMqConfigName)
        {
            GuardHelper.ArgumentNotNull(() => rabbitMqConfigName);
            var configuration = RabbitMqConfigurationHandler.GetConfig(rabbitMqConfigName);

            VirtualHost = configuration.VirtualHost;
            UserName = configuration.UserName;
            Password = configuration.UserName;
            RequestedHeartbeat = configuration.RequestedHeartbeat;
            Timeout = configuration.Timeout;
            PublisherConfirms = configuration.PublisherConfirms;
            PersistentMessages = configuration.PersistentMessages;
            CancelOnHaFailover = configuration.CancelOnHaFailover;
            UseBackgroundThreads = configuration.UseBackgroundThreads;
            Mandatory = configuration.Mandatory;
            PrefetchCount = configuration.PrefetchCount;
            ClientProperties = configuration.ClientProperties;
            RabbitMqHosts = configuration.RabbitMqHosts;

            SetClientDefaultProperties();
        }

        private void SetClientDefaultProperties()
        {
            var clientProperties = new Dictionary<string, object>();
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var applicationNameAndPath = Environment.GetCommandLineArgs()[0];

            var applicationName = "unknown";
            var applicationPath = "unknown";
            if (!string.IsNullOrWhiteSpace(applicationNameAndPath))
            {
                // Note: When running the application in an Integration Services Package (SSIS) the
                // Environment.GetCommandLineArgs()[0] can return null, and therefor it is not possible to get
                // the filename or directory name.
                try
                {
                    // Will only throw an exception if the applicationName contains invalid characters, is empty, or too long
                    // Silently catch the exception, as we will just leave the application name and path to "unknown"
                    applicationName = Path.GetFileName(applicationNameAndPath);
                    applicationPath = Path.GetDirectoryName(applicationNameAndPath);
                }
                catch (ArgumentException) { }
                catch (PathTooLongException) { }
            }

            var hostname = Environment.MachineName;
            var product = applicationName;
            var platform = hostname;

            clientProperties.Add("product", product);
            clientProperties.Add("platform", platform);
            clientProperties.Add("application", applicationName);
            clientProperties.Add("application_location", applicationPath);
            clientProperties.Add("machine_name", hostname);
            clientProperties.Add("connected", DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss")); 
            clientProperties.Add("requested_heartbeat", RequestedHeartbeat.ToString());
            clientProperties.Add("timeout", Timeout.ToString());
            clientProperties.Add("publisher_confirms", PublisherConfirms.ToString());
            clientProperties.Add("persistent_messages", PersistentMessages.ToString());

            ClientProperties.AddRange(clientProperties);
        }
    }
}