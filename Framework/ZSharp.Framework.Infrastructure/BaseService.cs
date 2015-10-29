using ZSharp.Framework.Caching;
using ZSharp.Framework.Logging;
using ZSharp.Framework.MongoDb;
using ZSharp.Framework.Repositories;
using ZSharp.Framework.Configurations;

namespace ZSharp.Framework.Service
{
    public abstract class BaseService : DisposableObject
    {
        private readonly IRepositoryContext context;
        private readonly ILogger logger;
        private ICacheManager cacheManager;

        static BaseService()
        {
            var isSupport = CommonConfig.IsSupportMongoDb;
            if (isSupport)
            {
                MongoInitHelper.InitMongoDBRepository();
            }
        }

        public BaseService(IRepositoryContext context)
        {
            this.context = context;
            logger = LogManager.GetLogger(this.GetType());
            cacheManager = CacheHelper.RedisCache;
        }

        /// <summary>
        /// 获取当前应用层服务所使用的仓储上下文实例。
        /// </summary>
        protected IRepositoryContext RepoContext
        {
            get { return this.context; }
        }

        /// <summary>
        /// 日誌記錄器，從配置中獲取LogAdapter
        /// </summary>
        protected ILogger Logger
        {
            get { return this.logger; }
        }

        /// <summary>
        /// 緩存管理器，服務端默認使用RedisCache
        /// </summary>
        protected ICacheManager CacheManager
        {
            get { return this.cacheManager; }
            set { this.cacheManager = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RepoContext.Dispose();
            }
        }
    }
}
