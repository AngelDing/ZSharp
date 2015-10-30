using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System;
using MongoDB.Driver;
using System.Configuration;
using ZSharp.Framework.MongoDb.Managers;

namespace Framework.MongoDb.Test
{
    public class MongoManagerTest : BaseMongoTest
    {
        private List<TreeNode> nodeList;
        public MongoManagerTest()
        {
            InitManagerTest();
            nodeList = CacheHelper.GetTreeNodes();
        }

        private void InitManagerTest()
        {
            var orderLogRep = new MongoTestDB<OrderLog2, long>();
            for (var i = 0; i < 10; i++)
            {
                var orderLog = new OrderLog2
                {
                    OrderId = i,
                    Summary = "Summary : " + i,
                    Title = "OrderLog : " + i,
                    UpdatedDate = DateTime.Now
                };
                orderLogRep.Insert(orderLog);
            }
            var manager = new MongoIndexManagerTest<OrderLog2>();
            manager.CreateIndex("OrderId");
        }

        [Fact]
        public void mongo_manager_get_all_nodes_test()
        {
            nodeList.Count.Should().BeGreaterThan(0);
        }

        #region Context Test
        [Fact]
        public void mongo_manager_server_context_test()
        {
            var context = new ServerContext();
            var sList = context.GetServerNodes();
            sList.Count.Should().Be(1);

            context.AddServer("127.0.0.1", 27017);
            sList = context.GetServerNodes();
            sList.Count.Should().Be(2);

            context.DeleteServer("127.0.0.1", 27017);
            sList = context.GetServerNodes();
            sList.Count.Should().Be(1);
        }

        //[Fact]
        //public void mongo_manager_replication_context_test()
        //{
        //    var sNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Server);
        //    var context = new ReplicationContext(sNode.Id);
        //    var info = context.GetReplicationInfo();
        //    info.Count.Should().BeGreaterThan(0);
        //}

        //[Fact]
        //public void mongo_manager_profile_context_test()
        //{
        //    var dbNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Database && p.Name == "MongoTestDB");
        //    var context = new ProfileContext(dbNode.Id);

        //    var result = context.SetProfile((int)ProfilingLevel.All);
        //    result.Should().BeTrue();

        //    InitManagerTest();
        //    var info = context.GetProfileData(10);
        //    info.Count.Should().Be(10);

        //    var status = context.GetProfileStatus();
        //    status.Should().Be(ProfilingLevel.All);
        //}

        //[Fact]
        //public void mongo_manager_data_context_test()
        //{
        //    var fieldNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Field && p.Name.Contains("OrderId"));
        //    var context = new DataContext(fieldNode.PId);
        //    var dataList = context.GetData("{OrderId:5}", "{CreatedBy:-1}", 0, 10);
        //    dataList.Count.Should().BeGreaterThan(0);

        //    var fieldList = context.GetFields();
        //    fieldList.Count.Should().BeGreaterThan(0);

        //    var explainList = context.Explain("{OrderId:5}", "{CreatedBy:-1}");
        //    explainList.Count.Should().BeGreaterThan(0);
        //}

        //[Fact]
        //public void mongo_manager_index_context_test()
        //{
        //    var idxNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Index && p.Name.Contains("OrderId"));
        //    var context = new IndexContext(idxNode.PId);
        //    var indexList = context.GetIndexes();
        //    indexList.Count.Should().BeGreaterThan(0);

        //    var indexEdit = new IndexEditModel
        //    {
        //        IndexName = "CreatedBy_Index",
        //        Keys = new List<IndexKey>
        //        {
        //            new IndexKey
        //            {
        //                FieldName = "CreatedBy",
        //                OrderType = IndexOrderType.Ascending
        //            }
        //        }
        //    };
        //    var jsonStr = JsonConvert.SerializeObject(indexEdit);
        //    context.CreateIndex(jsonStr);

        //    idxNode = CacheHelper.GetTreeNodes().FirstOrDefault(p => 
        //        p.Type == TreeNodeType.Index && p.Name.Contains("CreatedBy_Index"));
        //    idxNode.Should().NotBeNull();

        //    context.DeleteIndex(idxNode.Id);

        //    idxNode = CacheHelper.GetTreeNodes().FirstOrDefault(p =>
        //        p.Type == TreeNodeType.Index && p.Name.Contains("CreatedBy_Index"));
        //    idxNode.Should().BeNull();
        //}
        #endregion

        #region Info Test
        //[Fact]
        //public void mongo_manager_server_info_test()
        //{
        //    var sNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Server);
        //    var info = new ServerInfo(sNode.Id);
        //    var result = info.GetInfo();
        //    result.Count.Should().BeGreaterThan(0);
        //}

        //[Fact]
        //public void mongo_manager_collection_info_test()
        //{
        //    var cNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Collection);
        //    var info = new CollectionInfo(cNode.Id);
        //    var result = info.GetInfo();
        //    result.Count.Should().BeGreaterThan(0);
        //}

        //[Fact]
        //public void mongo_manager_database_info_test()
        //{
        //    var dbNode = nodeList.FirstOrDefault(p => p.Type == TreeNodeType.Database);
        //    var info = new DatabaseInfo(dbNode.Id);
        //    var result = info.GetInfo();
        //    result.Count.Should().BeGreaterThan(0);
        //}
        #endregion

        //private string keyName = "CustId";

        //[Fact]
        //public void mongo_index_test()
        //{
        //    var manager = new MongoIndexManagerTest<Customer>();
        //    if (!manager.IndexExists(keyName))
        //    {
        //        manager.CreateIndex("CustId");
        //    }
        //    manager.IndexExists("CustId").Should().BeTrue();

        //    manager.DropIndex("CustId");
        //    manager.IndexExists("CustId").Should().BeFalse();
        //}

       
    }
}
