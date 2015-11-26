using System.IO;
using ZSharp.Framework.Mvc.Plugins;

namespace ZSharp.Framework.Mvc.Packaging
{
	public interface IPackageBuilder
	{
		Stream BuildPackage(PluginDescriptor pluginDescriptor);
	}
}
