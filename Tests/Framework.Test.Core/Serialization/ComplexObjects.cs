using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Framework.Test.Core.Serialization
{
    [Serializable]
    [ProtoContract]
    public class ComplexObject : SimpleObjects
    {
        [ProtoMember(1)]
        public TimeSpan TimeSpan { get; set; }

        [ProtoMember(2)]
        public List<ListObject> ListObjects { get; set; }

        [ProtoMember(3)]
        public OrderItem OrderItem { get; set; }      

        [ProtoMember(4)]
        public DateTime? NullableDateTime { get; set; }

        [ProtoMember(5)]
        public decimal? NetPrice { get; set; }

        [ProtoMember(6)]
        public long? OrderId { get; set; }

        //[ProtoMember(7)]
        //public DateTimeOffset UpdatedDate { get; set; }

    }

    [Serializable]
    [ProtoContract]
    public class ListObject
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
    }

    [Serializable]
    [ProtoContract]
    public class OrderItem
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public decimal Price { get; set; }
        [ProtoMember(4)]
        public int Qty { get; set; }
        [ProtoMember(5)]
        public decimal SubTotal { get; set; }
    }
}
