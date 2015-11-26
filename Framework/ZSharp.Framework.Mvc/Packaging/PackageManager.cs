using System;
using System.IO;
using ZSharp.Framework.Mvc.Plugins;
using Log = ZSharp.Framework.Logging;

namespace ZSharp.Framework.Mvc.Packaging
{

	public class PackageManager : IPackageManager
	{
		private readonly IPluginFinder _pluginFinder;
		private readonly IPackageBuilder _packageBuilder;
		private readonly IPackageInstaller _packageInstaller;
		private readonly Log.ILogger _logger;

		public PackageManager(
			IPluginFinder pluginFinder,
			IPackageBuilder packageBuilder,
			IPackageInstaller packageInstaller,
			Log.ILogger logger)
		{
			_pluginFinder = pluginFinder;
			_packageBuilder = packageBuilder;
			_packageInstaller = packageInstaller;
			_logger = logger;
		}

		private PackageInfo DoInstall(Func<PackageInfo> installer)
		{
			try
			{
				return installer();
			}
			catch (FrameworkException)
			{
				throw;
			}
			catch (Exception exception)
			{
				var message = "There was an error installing the requested package. " +
					"This can happen if the server does not have write access to the '~/Plugins' or '~/Themes' folder of the web site. " +
					"If the site is running in shared hosted environement, adding write access to these folders sometimes needs to be done manually through the Hoster control panel. " +
					"Once Themes and Plugins have been installed, it is recommended to remove write access to these folders.";
				throw new FrameworkException(message, exception);
			}
		}

		public PackageInfo Install(Stream packageStream, string location, string applicationPath)
		{
			return DoInstall(() => _packageInstaller.Install(packageStream, location, applicationPath));
		}

		public void Uninstall(string packageId, string applicationPath)
		{
			_packageInstaller.Uninstall(packageId, applicationPath);
		}

		public PackagingResult BuildPluginPackage(string pluginName)
		{
			var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(pluginName);
			if (pluginDescriptor == null)
			{
				return null;
			}
			return new PackagingResult
			{
				ExtensionType = "Plugin",
				PackageName = pluginDescriptor.FolderName,
				PackageVersion = pluginDescriptor.Version.ToString(),
				PackageStream = _packageBuilder.BuildPackage(pluginDescriptor)
			};
		}
	}
}
