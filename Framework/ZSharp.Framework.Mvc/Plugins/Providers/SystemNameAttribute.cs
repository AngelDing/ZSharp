using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Mvc.Plugins
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class SystemNameAttribute : Attribute
	{
		public SystemNameAttribute(string name)
		{
			GuardHelper.ArgumentNotEmpty(() => name);
			Name = name;
		}

		public string Name { get; set; }
	}
}
