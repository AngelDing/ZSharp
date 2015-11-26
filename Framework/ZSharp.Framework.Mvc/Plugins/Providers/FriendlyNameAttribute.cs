using System;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Mvc.Plugins
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class FriendlyNameAttribute : Attribute
	{
		public FriendlyNameAttribute(string name)
		{
			GuardHelper.ArgumentNotNull(() => name);

			Name = name;
		}

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
