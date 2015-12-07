

using System;
using ZSharp.Framework.Caching;
using ZSharp.Framework.Configurations;
using ZSharp.Framework.Logging;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.Infrastructure
{
    public class CommonServices : ICommonServices
    {
        private readonly Lazy<ICacheManager> cache;
        private readonly Lazy<IRepositoryContext> repoContext;
        private readonly Lazy<ILogger> logger;
        private readonly Lazy<ISettingService> settings;

        //private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        //{
        //    var container = new UnityContainer();
        //    RegisterTypes(container);
        //    return container;
        //});

        //public CommonServices(
        //    Func<string, Lazy<ICacheManager>> cache,
        //    Lazy<IDbContext> dbContext,
        //    Lazy<IStoreContext> storeContext,
        //    Lazy<IWebHelper> webHelper,
        //    Lazy<IWorkContext> workContext,
        //    Lazy<IEventPublisher> eventPublisher,
        //    Lazy<ILocalizationService> localization,
        //    Lazy<ICustomerActivityService> customerActivity,
        //    Lazy<INotifier> notifier,
        //    Lazy<IPermissionService> permissions,
        //    Lazy<ISettingService> settings,
        //    Lazy<IStoreService> storeService)
        //{
        //    this._cache = cache("static");
        //    this._dbContext = dbContext;
        //    this._storeContext = storeContext;
        //    this._webHelper = webHelper;
        //    this._workContext = workContext;
        //    this._eventPublisher = eventPublisher;
        //    this._localization = localization;
        //    this._customerActivity = customerActivity;
        //    this._notifier = notifier;
        //    this._permissions = permissions;
        //    this._settings = settings;
        //    this._storeService = storeService;
        //}

        public ICacheManager Cache
        {
            get
            {
                return cache.Value;
            }
        }

        public ISettingService Settings
        {
            get
            {
                return settings.Value;
            }
        }

        public IRepositoryContext RepoContext
        {
            get
            {
                return repoContext.Value;
            }
        }

        public ILogger Logger
        {
            get
            {
                return logger.Value;
            }
        }
    }
}
