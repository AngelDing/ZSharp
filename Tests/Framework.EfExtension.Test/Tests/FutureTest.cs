using ZSharp.Framework.EfExtensions;
using ZSharp.Framework.EfExtensions.Future;
using System.Linq;
using Xunit;

namespace Framework.EfExtension.Test
{
    public class FutureTest
    {
        [Fact]
        public void ef_future_page_test()
        {
            var db = new TrackerContext();

            // base query
            var q = db.Tasks
                .Where(p => p.PriorityId == 2)
                .OrderByDescending(t => t.CreatedDate);
            int totalCount = 0;
            var result = PagingTest(q, out totalCount);
            var aa = result.ToList();

            // get total count
            var q1 = q.FutureCount();
            // get first page
            var q2 = q.Skip(0).Take(10).Future();
            // triggers sql execute as a batch
            var tasks = q2.ToList();
            int total = q1.Value;

            Assert.NotNull(tasks);
        }

        public void ef_future_new_page_test()
        {
            var db = new TrackerContext();
            var q = db.Tasks
               .Where(p => p.PriorityId == 2)
               .OrderByDescending(t => t.CreatedDate);
            var result = q.Paging();
            Assert.NotNull(result);
            Assert.Equal(20, result.Datas.Count());
            Assert.NotNull(result.Datas.First().Id);
        }

        public void ef_future_new_page_by_list_test()
        {
            var db = new TrackerContext();
            var q = db.Tasks
               .Where(p => p.PriorityId == 2)
               .OrderByDescending(t => t.CreatedDate);
            var list = q.ToList();

            var result = list.AsQueryable().Paging();
            Assert.NotNull(result);
            Assert.Equal(20, result.Datas.Count());
            Assert.NotNull(result.Datas.First().Id);           
        }

        private IQueryable<T> PagingTest<T>(IQueryable<T> source, out int totalCount, 
            int pageIndex = 1, int pageSize = 20)
            where T :class
        {
            totalCount = source.Count();
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 20;
            return source.Skip(((pageIndex - 1) * pageSize) < totalCount
                ? ((pageIndex - 1) * pageSize)
                : totalCount - (totalCount % pageSize)).Take(pageSize);
        }

        [Fact]
        public void ef_future_simple_test()
        {
            var db = new TrackerContext();

            // build up queries

            string emailDomain = "@battlestar.com";
            var q1 = db.Users
                .Where(p => p.EmailAddress.EndsWith(emailDomain))
                .Future();

            string search = "Earth";
            var q2 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .Future();

            // should be 2 queries 
            //Assert.Equal(2, db.FutureQueries.Count);

            // this triggers the loading of all the future queries
            var users = q1.ToList();
            Assert.NotNull(users);

            // should be cleared at this point
            //Assert.Equal(0, db.FutureQueries.Count);

            // this should already be loaded
            Assert.True(((IFutureQuery)q2).IsLoaded);

            var tasks = q2.ToList();
            Assert.NotNull(tasks);

        }

        [Fact]
        public void ef_future_count_test()
        {
            var db = new TrackerContext();

            // build up queries

            string emailDomain = "@battlestar.com";
            var q1 = db.Users
                .Where(p => p.EmailAddress.EndsWith(emailDomain))
                .Future();

            string search = "Summary";
            var q2 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .FutureCount();

            // should be 2 queries 
            //Assert.Equal(2, db.FutureQueries.Count);

            // this triggers the loading of all the future queries
            var users = q1.ToList();
            Assert.NotNull(users);

            // should be cleared at this point
            //Assert.Equal(0, db.FutureQueries.Count);

            // this should already be loaded
            Assert.True(((IFutureQuery)q2).IsLoaded);

            int count = q2;
            Assert.NotEqual(count, 0);
        }

        [Fact]
        public void ef_future_count_reverse_test()
        {
            var db = new TrackerContext();

            // build up queries

            string emailDomain = "@battlestar.com";
            var q1 = db.Users
                .Where(p => p.EmailAddress.EndsWith(emailDomain))
                .Future();

            string search = "Summary";
            var q2 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .FutureCount();

            // should be 2 queries 
            //Assert.Equal(2, db.FutureQueries.Count);

            // access q2 first to trigger loading, testing loading from FutureCount
            // this triggers the loading of all the future queries
            var count = q2.Value;
            Assert.NotEqual(count, 0);

            // should be cleared at this point
            //Assert.Equal(0, db.FutureQueries.Count);

            // this should already be loaded
            Assert.True(((IFutureQuery)q1).IsLoaded);

            var users = q1.ToList();
            Assert.NotNull(users);
        }

        [Fact]
        public void ef_future_value_test()
        {
            var db = new TrackerContext();

            // build up queries
            string emailDomain = "@battlestar.com";
            var q1 = db.Users
                .Where(p => p.EmailAddress.EndsWith(emailDomain))
                .FutureFirstOrDefault();

            string search = "Summary";
            var q2 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .FutureCount();

            // duplicate query except count
            var q3 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .Future();

            // should be 3 queries 
            //Assert.Equal(3, db.FutureQueries.Count);

            // this triggers the loading of all the future queries
            User user = q1;
            Assert.NotNull(user);

            // should be cleared at this point
            //Assert.Equal(0, db.FutureQueries.Count);

            // this should already be loaded
            Assert.True(((IFutureQuery)q2).IsLoaded);

            var count = q2.Value;
            Assert.NotEqual(count, 0);

            var tasks = q3.ToList();
            Assert.NotNull(tasks);
        }

        [Fact]
        public void ef_future_value_reverse_test()
        {
            var db = new TrackerContext();
            // build up queries

            string emailDomain = "@battlestar.com";
            var q1 = db.Users
                .Where(p => p.EmailAddress.EndsWith(emailDomain))
                .FutureFirstOrDefault();

            string search = "Summary";
            var q2 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .FutureCount();

            // duplicate query except count
            var q3 = db.Tasks
                .Where(t => t.Summary.Contains(search))
                .Future();

            // should be 3 queries 
            //Assert.Equal(3, db.FutureQueries.Count);

            // access q2 first to trigger loading, testing loading from FutureCount
            // this triggers the loading of all the future queries
            var count = q2.Value;
            Assert.NotEqual(count, 0);

            // should be cleared at this point
            //Assert.Equal(0, db.FutureQueries.Count);

            // this should already be loaded
            Assert.True(((IFutureQuery)q1).IsLoaded);

            var users = q1.Value;
            Assert.NotNull(users);

            var tasks = q3.ToList();
            Assert.NotNull(tasks);
        }

        [Fact]
        public void ef_future_value_with_aggregate_test()
        {
            var db = new TrackerContext();

            var q1 = db.Users.Where(x => x.EmailAddress.EndsWith("@battlestar.com")).FutureValue(x => x.Count());
            var q2 = db.Users.Where(x => x.EmailAddress.EndsWith("@battlestar.com")).FutureValue(x => x.Min(t => t.LastName));
            var q3 = db.Tasks.FutureValue(x => x.Sum(t => t.Priority.Order));

            Assert.False(((IFutureQuery)q1).IsLoaded);
            Assert.False(((IFutureQuery)q2).IsLoaded);
            Assert.False(((IFutureQuery)q3).IsLoaded);

            var r1 = q1.Value;
            var r2 = q2.Value;
            var r3 = q3.Value;

            Assert.True(((IFutureQuery)q1).IsLoaded);
            Assert.True(((IFutureQuery)q2).IsLoaded);
            Assert.True(((IFutureQuery)q3).IsLoaded);
        }
    }
}