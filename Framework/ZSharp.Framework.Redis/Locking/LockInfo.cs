using System;

namespace ZSharp.Framework.Redis
{
    public class LockInfo
    {
        public LockInfo(string resource, string val, TimeSpan ttl)
        {
            this.Resource = resource;
            this.Value = val;
            this.TTL = ttl;
        }

        public string Resource { get; private set; }

        public string Value { get; private set; }

        /// <summary>
        /// 生存时间值：Time To Live 
        /// </summary>
        public TimeSpan TTL { get; private set; }
    }
}
