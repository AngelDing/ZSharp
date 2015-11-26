using System;
using System.Threading;
using System.IO;
using NuGet;
using ZSharp.Framework.Threading;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Mvc.Packaging
{
	
	public sealed class AppUpdater
	{
		public const string UpdatePackagePath = "~/App_Data/Update";
		
		private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
		private Logging.ILogger logger;

		#region Package update

		public bool InstallablePackageExists()
		{
			string packagePath = null;
			var package = FindPackage(false, out packagePath);

			if (package == null)
				return false;

			if (!ValidatePackage(package))
				return false;

			if (!CheckEnvironment())
				return false;

			return true;
		}

		internal bool TryUpdateFromPackage()
		{			
			using (rwLock.GetUpgradeableReadLock())
			{
				try
				{
					string packagePath = null;
					var package = FindPackage(true, out packagePath);

					if (package == null)
						return false;

					if (!ValidatePackage(package))
						return false;

					if (!CheckEnvironment())
						return false;

					using (rwLock.GetWriteLock())
					{
						Backup();

						var info = ExecuteUpdate(package);

						if (info != null)
						{
							var newPath = packagePath + ".applied";
							if (File.Exists(newPath))
							{
								File.Delete(packagePath);
							}
							else
							{
								File.Move(packagePath, newPath);
							}						
						}

						return info != null;
					}
				}
				catch (Exception ex)
				{
					logger.Error("An error occured while updating the application: {0}".FormatCurrent(ex.Message), ex);
					return false;
				}
			}
		}

		private Logging.ILogger CreateLogger(IPackage package)
		{
            var logFile = Path.Combine(CommonHelper.MapPath(UpdatePackagePath, false), "Updater.{0}.log".FormatInvariant(package.Version.ToString()));
            return Logging.LogManager.GetLogger(logFile);
        }

		private IPackage FindPackage(bool createLogger, out string path)
		{
			path = null;
			var dir = CommonHelper.MapPath(UpdatePackagePath, false);

			if (!Directory.Exists(dir))
				return null;

			var files = Directory.GetFiles(dir, "SmartStore.*.nupkg", SearchOption.TopDirectoryOnly);

			// TODO: allow more than one package in folder and return newest
			if (files == null || files.Length == 0 || files.Length > 1)
				return null;

			IPackage package = null;

			try
			{
				path = files[0];
				package = new ZipPackage(files[0]);
				if (createLogger)
				{
					logger = CreateLogger(package);
					logger.Info("Found update package '{0}'".FormatInvariant(package.GetFullName()));
				}
				return package;
			}
			catch { }

			return null;
		}

		private bool ValidatePackage(IPackage package)
		{
			if (package.Id != "SmartStore")
				return false;
			
			var currentVersion = new SemanticVersion(ZSharpVersion.Version);
			return package.Version > currentVersion;
		}

		private bool CheckEnvironment()
		{
			// TODO: Check it :-)
			return true;
		}

		private void Backup()
		{
			var source = new DirectoryInfo(CommonHelper.MapPath("~/"));

			var tempPath = CommonHelper.MapPath("~/App_Data/_Backup/App/SmartStore");
			string localTempPath = null;
			for (int i = 0; i < 50; i++)
			{
				localTempPath = tempPath + (i == 0 ? "" : "." + i.ToString());
				if (!Directory.Exists(localTempPath))
				{
					Directory.CreateDirectory(localTempPath);
					break;
				}
				localTempPath = null;
			}
			
			if (localTempPath == null)
			{
				var exception = new FrameworkException("Too many backups in '{0}'.".FormatInvariant(tempPath));
				logger.Error(exception.Message, exception);
				throw exception;
			}

			var backupFolder = new DirectoryInfo(localTempPath);
			var folderUpdater = new FolderUpdater(logger);
			folderUpdater.Backup(source, backupFolder, "App_Data", "Media");

			logger.Info("Backup successfully created in folder '{0}'.".FormatInvariant(localTempPath));
		}

		private PackageInfo ExecuteUpdate(IPackage package)
		{
			var appPath = CommonHelper.MapPath("~/");
			
			var logger = new NugetLogger(this.logger);

			var project = new FileBasedProjectSystem(appPath) { Logger = logger };

			var nullRepository = new NullSourceRepository();

			var projectManager = new ProjectManager(
				nullRepository,
				new DefaultPackagePathResolver(appPath),
				project,
				nullRepository
				) { Logger = logger };

			// Perform the update
			projectManager.AddPackageReference(package, true, false);

			var info = new PackageInfo
			{
				Id = package.Id,
				Name = package.Title ?? package.Id,
				Version = package.Version.ToString(),
				Type = "App",
				Path = appPath
			};

			this.logger.Info("Update '{0}' successfully executed.".FormatInvariant(info.Name));

			return info;
		}
        #endregion
    }
}
