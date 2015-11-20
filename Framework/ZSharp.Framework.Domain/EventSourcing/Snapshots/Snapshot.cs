
using System;

namespace ZSharp.Framework.Domain
{
    public abstract class Snapshot : ISnapshot
    {
        public Guid AggregateId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// 数据库实例序列化后的字符串
        /// </summary>
        public string State { get; set; }
    }
}
