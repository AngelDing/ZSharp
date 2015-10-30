using ZSharp.Framework.Serializations;
using System;
using System.Collections.Generic;

namespace Framework.Test.Core.Serialization
{
    public abstract class BaseSerializerTest
    {
        public ISerializer Serializer { get; private set; }

        public BaseSerializerTest()
        {
            Serializer = GetSerializer();
        }

        public abstract ISerializer GetSerializer();

        public object GetSerializedLoopObject()
        {
            var loopObject = CreateLoopObject();
            var result = Serializer.Serialize<byte[]>(loopObject);
            return result;
        }

        public object GetSerializedSimpleObject()
        {
            var simpleObject = CreateSimpleObject();
            var result = Serializer.Serialize<byte[]>(simpleObject);
            return result;
        }

        public object GetSerializedComplexObject()
        {
            var simpleObject = CreateComplexObject();
            var result = Serializer.Serialize<byte[]>(simpleObject);
            return result;
        }

        public object GetSerializedGenericsObject(object genericsObj = null)
        {
            if (genericsObj == null)
            {
                genericsObj = CreateGenericsObject();
            }
            var result = Serializer.Serialize<byte[]>(genericsObj);
            return result;
        }

        private object CreateGenericsObject()
        {
            var obj = new GenericsObject<DateTime>
            {
                Key = "Jacky",
                Value = DateTime.Now
            };

            return obj;
        }

        public object CreateComplexObject()
        {
            var obj = new ComplexObject
            {
                Id = 1,
                Name = "Complex Object",
                Price = 1000,
                TimeSpan = new TimeSpan(1, 1, 1),
                CreatedDate = DateTime.Now,
                //UpdatedDate = DateTimeOffset.Now.UtcDateTime,
                OrderItem = new OrderItem
                {
                    Id = 1,
                    Name = "Order Item",
                    Price = 20,
                    Qty = 3,
                    SubTotal = 60
                },
                ListObjects = new List<ListObject>
                {
                    new ListObject { Id = 1, Name =  "Item1"},
                    new ListObject { Id = 2, Name =  "Item2"},
                    new ListObject { Id = 3, Name =  "Item3"}
                }
            };

            return obj;
        }

        private object CreateSimpleObject()
        {
            var obj = new SimpleObjects
            {
                Id = 1,
                Name = "Test Name",
                Price = 100,
                CreatedDate = DateTime.Now
            };
            return obj;
        }

        private Book CreateLoopObject()
        {
            var book = new Book
            {
                Name = "平凡世界"
            };
            var autor = new Author
            {
                Name = "路遥"
            };
            book.Author = autor;
            autor.Book = book;
            return book;
        }
    }
}
