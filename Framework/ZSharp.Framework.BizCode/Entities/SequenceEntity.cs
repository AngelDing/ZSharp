using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.BizCode
{
    public class SequenceEntity : Entity<Guid>, IAggregateRoot<Guid>
    {
        public string CurrentCode { get; internal set; }
        public int CurrentNo { get; internal set; }
        public string CurrentReset { get; internal set; }
        public string SequenceDelimiter { get; internal set; }
        public string SequenceName { get; internal set; }
        public string SequenceReset { get; internal set; }
        public int Step { get; internal set; }
    }
}
