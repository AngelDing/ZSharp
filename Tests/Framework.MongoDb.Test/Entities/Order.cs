using System;
using System.Collections.Generic;
using ZSharp.Framework.MongoDb;
using MongoDB.Bson.Serialization.Attributes;
using ZSharp.Framework.MongoDb.Conventions;

namespace Framework.MongoDb.Test
{
    public class Order : StringKeyMongoEntity
    {
        public DateTime PurchaseDate { get; set; }

        public IList<OrderItem> Items;

        public Customer Customer { get; set; }
    }

    public class OrderItem
    {
        public Product Product
        {
            get;
            set;
        }

        public int Quantity { get; set; }

        //[BsonSerializer(typeof(MongoDbMoneyFieldSerializer))]
        public decimal Price { get; set; }
    }
}
