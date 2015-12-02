using System;
using System.Collections.Generic;
using ZSharp.Framework.MongoDb;

namespace Framework.MongoDb.Test
{
    public class MyTestEntity : StringKeyMongoEntity
    {
        public string A { get; set; }

        public DateTime B { get; set; }

        public decimal C { get; set; }

        public DateTime? D { get; set; }
    }


    public class LogEntity : StringKeyMongoEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Price { get; set; }

        public decimal? Amount { get; set; }

        public ICollection<MyTestEntity> Details { get; set; }
    }
}
