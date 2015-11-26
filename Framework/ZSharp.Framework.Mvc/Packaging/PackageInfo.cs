using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZSharp.Framework.Mvc.Packaging
{
	public class PackageInfo
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }
		public string Type { get; set; }
		public string Path { get; set; }
		public ExtensionDescriptor ExtensionDescriptor { get; set; }
	}
}
