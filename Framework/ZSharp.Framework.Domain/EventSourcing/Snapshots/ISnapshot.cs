
using System;

namespace ZSharp.Framework.Domain
{
    public interface ISnapshot
    {
        Guid AggregateId { get; set; }

        int Version { get; set; }
    }

    /// <summary>
    /// 快照基类
    /// </summary>
    public abstract class Snapshot : ISnapshot
    {
        public Guid AggregateId { get; set; }

        public int Version { get; set; }
    }


    //[Serializable]
    //public class SourcedCustomerSnapshot : Snapshot
    //{
    //    public XXXEntity Entity { get; set; } 

    //    public string Username { get; set; }

    //    public string Password { get; set; }

    //    public string FirstName { get; set; }

    //    public string LastName { get; set; }

    //    public string Email { get; set; }

    //    public DateTimeOffset Birth { get; set; }
    //}
}
