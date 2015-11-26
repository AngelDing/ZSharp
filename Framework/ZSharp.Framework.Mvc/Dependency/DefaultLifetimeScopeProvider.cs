using System;
using Autofac;
using Autofac.Integration.Mvc;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Mvc
{
	public class DefaultLifetimeScopeProvider : ILifetimeScopeProvider
	{
		private readonly ILifetimeScopeAccessor _accessor;

		public DefaultLifetimeScopeProvider(ILifetimeScopeAccessor accessor)
		{
			GuardHelper.ArgumentNotNull(() => accessor);

			this._accessor = accessor;
			AutofacRequestLifetimeHttpModule.SetLifetimeScopeProvider(this);
		}

		public ILifetimeScope ApplicationContainer
		{
			get { return _accessor.ApplicationContainer; }
		}

		public void EndLifetimeScope()
		{
			_accessor.EndLifetimeScope();
		}

		public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
		{
			return _accessor.GetLifetimeScope(configurationAction);
		}
	}
}
