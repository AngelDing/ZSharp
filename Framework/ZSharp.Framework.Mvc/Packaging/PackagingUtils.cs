using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using NuGet;
using ZSharp.Framework.Mvc.Plugins;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Mvc.Packaging
{
    public static class PackagingUtils
    {
        public static string GetExtensionPrefix(string extensionType)
        {
            return string.Format("SmartStore.{0}.", extensionType);
        }

        public static string BuildPackageId(string extensionName, string extensionType)
        {
            return GetExtensionPrefix(extensionType) + extensionName;
        }

        internal static string ExtensionFolder(this IPackage package)
        {
            return ExtensionFolder();
        }

        internal static string ExtensionId(this IPackage package)
        {
            return ExtensionId(package.Id);
        }

        private static string ExtensionFolder()
        {
            return "Plugins";
        }

        private static string ExtensionId(string packageId)
        {
            return packageId.Substring(GetExtensionPrefix("Plugin").Length);
        }

        internal static ExtensionDescriptor ConvertToExtensionDescriptor(this PluginDescriptor pluginDescriptor)
        {
            // TODO: (pkg) Add Icons to extension manifests
            var descriptor = new ExtensionDescriptor
            {
                ExtensionType = "Plugin",
                Location = "~/Plugins",
                Path = pluginDescriptor.PhysicalPath,
                Id = pluginDescriptor.FolderName,
                Author = pluginDescriptor.Author,
                MinAppVersion = pluginDescriptor.MinAppVersion,
                Version = pluginDescriptor.Version,
                Name = pluginDescriptor.FriendlyName,
                Description = pluginDescriptor.Description,
                WebSite = pluginDescriptor.Url, // TODO: (pkg) Add author url to plugin manifests,
                Tags = string.Empty // TODO: (pkg) Add tags to plugin manifests,
            };

            return descriptor;
        }

        internal static ExtensionDescriptor GetExtensionDescriptor(this IPackage package, string extensionType)
        {
            IPackageFile packageFile = package.GetFiles().FirstOrDefault(file =>
            {
                var fileName = Path.GetFileName(file.Path);
                return fileName != null && fileName.Equals("Description.txt", StringComparison.OrdinalIgnoreCase);
            });

            ExtensionDescriptor descriptor = null;

            if (packageFile != null)
            {
                var filePath = packageFile.EffectivePath;
                if (filePath.HasValue())
                {
                    filePath = Path.Combine(HostingEnvironment.MapPath("~/"), filePath);

                    var pluginDescriptor = PluginFileParser.ParsePluginDescriptionFile(filePath);
                    if (pluginDescriptor != null)
                    {
                        descriptor = pluginDescriptor.ConvertToExtensionDescriptor();
                    }

                }
            }

            return descriptor;
        }
    }
}
