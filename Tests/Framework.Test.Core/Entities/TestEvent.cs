using ProtoBuf;
using System;

namespace Framework.Test.Core.Entities
{
    public interface IEvent
    {
        Guid SourceId { get; set; }
    }

    [Serializable]
    [ProtoContract]
    public class TestEvent : IEvent
    {
        [ProtoMember(1)]
        public Guid SourceId { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public decimal Price { get; set; }
        [ProtoMember(4)]
        public DateTime? CreatedDate { get; set; }
    }
}
