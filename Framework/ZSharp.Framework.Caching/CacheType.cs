using System.ComponentModel;

namespace ZSharp.Framework.Caching
{
    public enum CacheType
    {
        [Description("None")]
        None = 0,
        [Description("Web")]
        Web = 1,
        [Description("Memory")]
        Memory = 2,
        [Description("Redis")]
        Redis = 3,
        [Description("ConcurrentDictionary")]
        ConcurrentDictionary = 4,

        //Memcached,

        //AzureTableStorage,         

        //Disk
    }
}
