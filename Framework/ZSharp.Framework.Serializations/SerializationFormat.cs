
using System.ComponentModel;

namespace ZSharp.Framework.Serializations
{
    public enum SerializationFormat
    {
        /// <summary>
        /// No serialization format set 
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Null serailization
        /// </summary>
        [Description("Null")]
        Null,

        /// <summary>
        /// JSON serialization
        /// </summary>
        [Description("Json")]
        Json,

        /// <summary>
        /// XML serialization 
        /// </summary>
        [Description("Xml")]
        Xml,

        /// <summary>
        /// Binary serialization
        /// </summary>
        [Description("Binary")]
        Binary,

        /// <summary>
        /// Jil serialization
        /// https://github.com/kevin-montrose/Jil
        /// </summary>
        [Description("Jil")]
        Jil,

        /// <summary>
        /// MsgPack serialization
        /// https://github.com/msgpack/msgpack-cli
        /// </summary>
        [Description("MsgPack")]
        MsgPack,

        /// <summary>
        /// ProtoBuf serialization
        /// </summary>
        [Description("ProtoBuf")]
        ProtoBuf,
    }
}
