using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace ZSharp.Framework.MongoDb.Conventions
{
    [BsonSerializer(typeof(MongoDbMoneyFieldSerializer))]
    public class MongoDbMoneyFieldSerializer : SerializerBase<decimal>
    {
        public override decimal Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            decimal result = 0;
            var bsonReader = context.Reader;

            if (bsonReader.CurrentBsonType == BsonType.String)
            {
                //兼容舊數據
                decimal.TryParse(bsonReader.ReadString(), out result);
            }
            else if (bsonReader.CurrentBsonType == BsonType.Int64)
            {
                var dbData = bsonReader.ReadInt64();
                result = (decimal)dbData / (decimal)10000;
            }
            else
            {
                throw new ArgumentException(
                    "金額數據類型錯誤：期待的類型為String或者Int64，現在類型：" + bsonReader.CurrentBsonType.ToString());
            }

            return result;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, decimal value)
        {
            context.Writer.WriteInt64(Convert.ToInt64(value * 10000));
        }
    }
}
