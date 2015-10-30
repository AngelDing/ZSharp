using System.Linq;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System;

namespace Framework.MongoDb.Test
{
    public class MongoIdTest : BaseMongoTest
    {
        [Fact]
        public void mongo_string_id_test()
        {
            var pInfo = new Product
            {
               Description = "XXX",
               Name = "aa",
               Price = 100               
            };

            var repo = new MongoTestDB<Product>();
            repo.Insert(pInfo);

            var added = repo.GetBy(p => p.Name == "aa").FirstOrDefault();
            added.Should().NotBeNull();
            added.Id.Should().NotBeEmpty();
            added.Price.Should().Be(100);
        }

        [Fact]
        public void mongo_long_id_generator_test()
        {
            var repo = new MongoTestDB<OrderLog, long>();
            var all = repo.GetAll();
            var count = all.Count();

            var log = new OrderLog
            {
                Summary = "test",
                Title = "aa",
                OrderDate = DateTime.Now
            };
            repo.Insert(log);
            var id = log.Id;

            id.Should().Be(count + 1);
        }

        [Fact]
        public void mongo_long_id_generator_concurrency_test()
        {
            var repo = new MongoTestDB<CustomEntityTest2, int>();
            var all = repo.GetAll();
            var count = all.Count();

            var num = 10;
            var tasks = new Task[num];
            for (var i = 0; i < num; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var custom = new CustomEntityTest2()
                    {
                        Name = "jia" + DateTime.Now.ToLongDateString()
                    };
                    repo.Insert(custom);
                });
            }
            Task.WaitAll(tasks);

            var list = repo.GetAll().ToList();
            var maxId = list.Max(p => p.Id);
            maxId.Should().Be(count + num);
        }
    }
}
