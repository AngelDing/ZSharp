using System.IO;

namespace ZSharp.Framework.Mvc.Packaging
{
	public class PackagingResult
	{
		public string ExtensionType { get; set; }

		public string PackageName { get; set; }
		public string PackageVersion { get; set; }
		public Stream PackageStream { get; set; }
	}
}
