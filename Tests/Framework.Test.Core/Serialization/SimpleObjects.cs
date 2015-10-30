using ProtoBuf;
using System;

namespace Framework.Test.Core.Serialization
{
    [Serializable]
    [ProtoContract]
    [ProtoInclude(5, typeof(ComplexObject))]
    public class SimpleObjects
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public decimal Price { get; set; }
        [ProtoMember(4)]
        public DateTime CreatedDate { get; set; }
    }
}
