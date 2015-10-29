using ProtoBuf;
using System;

namespace Framework.Test.Core.Serialization
{
    [Serializable]
    [ProtoContract]
    public class Book
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public Author Author { get; set; }
    }

    [Serializable]
    [ProtoContract]
    public class Author
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public Book Book { get; set; }
    }
}
