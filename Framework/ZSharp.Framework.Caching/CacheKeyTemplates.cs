
namespace ZSharp.Framework.Caching
{
    public static class CacheKeyTemplates
    {
        /// <summary>
        /// 用於暫度
        /// </summary>
        public static readonly string CacheSign = "global::sign::{0}";

        /// <summary>
        /// 用於緩存依賴
        /// </summary>
        public static readonly string CacheTag = "global::tag::{0}";
    }
}
