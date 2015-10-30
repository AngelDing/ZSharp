using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using System.Diagnostics;
using ZSharp.Framework.EfExtensions;
using System.Threading;

namespace Framework.EfExtension.Test
{    
    public class BatchTest
    {
        #region Batch Insert

        [Fact]
        public void ef_batch_insert_test()
        {
            for (var i = 0; i <= 5; i++)
            {
                var taskList = GetTaskList();
                var watch = Stopwatch.StartNew();
                using (var context = new TrackerContext())
                {
                    context.BulkInsert(taskList);
                    context.SaveChanges();
                }
                watch.Stop();
                var msgFormat = "Done, cost {0} milliseconds.";
                Console.WriteLine(string.Format(msgFormat, watch.ElapsedMilliseconds));
            }
        }

        private static List<Task> GetTaskList()
        {
            int count = 10000;
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                DateTime now = DateTime.Now;
                var task = new Task()
                {
                    AssignedId = i,
                    CreatedId = i,
                    StatusId = i,
                    PriorityId = 2,
                    Summary = "Summary: " + now.Ticks,
                    RowVersion = BitConverter.GetBytes(now.ToBinary())
                };
                taskList.Add(task);
            }

            return taskList;
        }

        #endregion

        #region Batch Delete

        [Fact]
        public void ef_batch_delete_test()
        {
            var db = new TrackerContext();
            int count = db.Tasks
                .Where(t => t.Id > 10)
                .Delete();
        }
      
         [Fact]
        public void ef_batch_delete_nodata_test()
        {
            var db = new TrackerContext();
            var today = DateTime.Now;
            db.Tasks.Where(t => t.CreatedDate > today).Delete();
        }
         public void ef_batch_delete_havedata_test()
         {
             var db = new TrackerContext();
             var taskIDs = new List<int>() { 12212,1212,12121212};
             var today = DateTime.Now;
             int count = db.Tasks
                 .Where(t => t.CreatedDate > today && taskIDs.Contains(t.Id))
                 .Delete();
         }

        [Fact]
        public async void ef_batch_delete_async_test()
        {
            var db = new TrackerContext();
            int count = await db.Tasks
                .Where(t => t.Id > 10)
                .DeleteAsync();
        }

        #endregion

        #region Batch Update

        [Fact]
        public void ef_batch_update_test()
        {
            var db = new TrackerContext();
            int count = db.Tasks
                .Where(t => t.PriorityId == 2)
                .Update(t => new Task { Summary = "Summary XXXXX: " + DateTime.Now.Ticks });
        }


        [Fact]
        public async void ef_batch_update_async_test()
        {
            var db = new TrackerContext();
            int count = await db.Tasks
                .Where(t => t.PriorityId == 2)
                .UpdateAsync(t => new Task { Summary = "Summary YYYYY: " + DateTime.Now.Ticks });
        }

        [Fact]
        public void ef_batch_update_append_test()
        {
            var db = new TrackerContext();
            int count = db.Tasks
                .Where(t => t.PriorityId == 2)
                .Update(t => new Task { Summary = t.Summary + " ZZZZZ"});
        }

        [Fact]
        public void ef_batch_update_join_test()
        {
            var db = new TrackerContext();
            string space = " ";

            int count = db.Tasks
                .Where(t => t.Id == 4)
                .Update(t => new Task { Summary = t.Summary + space + t.Details });
        }

        [Fact]
        public void ef_batch_update_copy_test()
        {
            var db = new TrackerContext();

            int count = db.Tasks
                .Where(t => t.Id == 6)
                .Update(t => new Task { Details = t.Summary });
        }        

        #endregion      
    }
}
