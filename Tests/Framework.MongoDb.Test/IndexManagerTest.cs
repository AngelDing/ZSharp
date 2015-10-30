using ZSharp.Framework.MongoDb.Managers;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace Framework.MongoDb.Test
{
    public class IndexManagerTest : BaseMongoTest
    {
        [Fact]
        public void mongo_index_create_by_key_name_test()
        {
            var indexManager = new IndexManager<Customer5>("MongoTestDB");
            indexManager.CreateIndex("LastName");
            var isExistes = indexManager.IndexExist("LastName");
            isExistes.Should().Be(true);
        }

        [Fact]
        public void mongo_index_create_by_index_name_test()
        {
            var indexManager = new IndexManager<Customer4>("MongoTestDB");
            indexManager.CreateIndexes(new List<string> { "CreateDate" }, "MyCreateDate");
            var isExistes = indexManager.IndexExist("CreateDate");
            isExistes.Should().Be(false);

            isExistes = indexManager.IndexExist("MyCreateDate");
            isExistes.Should().Be(true);
        }

        [Fact]
        public void mongo_index_drop_test()
        {
            var indexManager = new IndexManager<Customer6>("MongoTestDB");
            indexManager.CreateIndex("FirstName");
            var isExistes = indexManager.IndexExist("FirstName");
            isExistes.Should().Be(true);

            indexManager.DropIndex("FirstName");

            isExistes = indexManager.IndexExist("FirstName");
            isExistes.Should().Be(false);
        }

        [Fact]
        public void mongo_index_drop_by_index_name_test()
        {
            var indexManager = new IndexManager<Customer2>("MongoTestDB");
            indexManager.CreateIndexes(new List<string> { "LastName" }, "MyLastName");
            var isExistes = indexManager.IndexExist("MyLastName");
            isExistes.Should().Be(true);

            indexManager.DropIndex("MyLastName");

            isExistes = indexManager.IndexExist("MyLastName");
            isExistes.Should().Be(false);
        }

        [Fact]
        public void mongo_index_create_composite_test()
        {
            var indexManager = new IndexManager<Customer7>("MongoTestDB");
            indexManager.CreateIndexes(new List<string> { "FirstName", "LastName" }, "MyIndexName");
            var isExistes = indexManager.IndexExist("MyIndexName");
            isExistes.Should().Be(true);
        }
    }
}
