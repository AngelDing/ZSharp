using System;
using ZSharp.Framework.MongoDb;

namespace Framework.MongoDb.Test
{
    public class OrderLog : LongKeyMongoEntity
    {
        public int OrderId { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime OrderDate { get; set; }
    }

    public class OrderLog2 : OrderLog
    {
    }
}
