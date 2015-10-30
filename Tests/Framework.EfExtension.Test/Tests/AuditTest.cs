using System;
using System.Linq;
using System.Data.Entity;
using Xunit;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using ZSharp.Framework.EfExtensions;

namespace Framework.EfExtension.Test
{
    public class AuditTest
    {
        [Fact]
        public void ef_aduit_create_log_format_test()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = db.Users.Find(1);
            user.Comment = "Testing: " + DateTime.Now.Ticks;

            var task = new Task()
            {
                AssignedId = 1,
                CreatedId = 1,
                StatusId = 1,
                PriorityId = 2,
                Summary = "Summary: " + DateTime.Now.Ticks
            };
            db.Tasks.Add(task);

            var task2 = db.Tasks.Where(p => p.Id == 2)
                .Include(p => p.TaskExtended)
                .Include(p => p.Audits)
                .FirstOrDefault();
            task2.PriorityId = 2;
            task2.StatusId = 2;
            task2.Summary = "Summary: " + DateTime.Now.Ticks;
            task2.TaskExtended.ModifiedDate = DateTime.Now.AddDays(1);
            var auditList = task2.Audits.ToList();
            auditList.RemoveAt(0);

            auditList.AddRange(new List<AuditData> 
            {
                new AuditData
                {
                    Content = "1111",
                    CreatedDate = DateTime.Now,
                    Date = DateTime.Now,
                    UserId = 1,
                    Username = "jia"
                }
            });

            task2.Audits = auditList;
            task2.TaskExtended = new TaskExtended
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            db.SaveChanges();
            var log = audit.LastLog;
            Assert.NotNull(log);
        }

        [Fact]
        public void ef_aduit_create_log_loaded_test()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = db.Users.Find(1);
            user.Comment = "Testing: " + DateTime.Now.Ticks;

            var newTask = new Task()
            {
                AssignedId = 1,
                CreatedId = 1,
                StatusId = 1,
                PriorityId = 2,
                Summary = "Summary: " + DateTime.Now.Ticks
            };
            db.Tasks.Add(newTask);

            var p = db.Priorities.Find(2);

            var updateTask = db.Tasks.Find(2);
            updateTask.Priority = p;
            updateTask.StatusId = 2;
            updateTask.Summary = "Summary: " + DateTime.Now.Ticks;

            var log = audit.CreateLog();
            Assert.NotNull(log);
        }

        [Fact]
        public void ef_aduit_create_log_test()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = db.Users.Find(1);
            user.Comment = "Testing: " + DateTime.Now.Ticks;

            var task = new Task()
            {
                AssignedId = 1,
                CreatedId = 1,
                StatusId = 1,
                PriorityId = 2,
                Summary = "Summary: " + DateTime.Now.Ticks
            };
            db.Tasks.Add(task);

            var task2 = db.Tasks.Find(2);
            task2.PriorityId = 2;
            task2.StatusId = 2;
            task2.Summary = "Summary: " + DateTime.Now.Ticks;

            var log = audit.CreateLog();
            Assert.NotNull(log);
        }

        [Fact]
        public void ef_aduit_create_log_test2()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var task = db.Tasks.Find(2);
            Assert.NotNull(task);

            task.PriorityId = 2;
            task.StatusId = 2;
            task.Summary = "Summary: " + DateTime.Now.Ticks;

            var log = audit.CreateLog();
            Assert.NotNull(log);
        }

        [Fact]
        public void ef_aduit_create_log_test3()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = new User();
            user.EmailAddress = string.Format("email.{0}@test.com", DateTime.Now.Ticks);
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.PasswordHash = DateTime.Now.Ticks.ToString();
            user.PasswordSalt = "abcde";
            user.IsApproved = false;
            user.LastActivityDate = DateTime.Now;

            db.Users.Add(user);

            var log = audit.CreateLog();
            Assert.NotNull(log);
            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }
            db.SaveChanges();
            log.Refresh();
        }

        [Fact]
        public void ef_aduit_refresh_test()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = new User();
            user.EmailAddress = string.Format("email.{0}@test.com", DateTime.Now.Ticks);
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.PasswordHash = DateTime.Now.Ticks.ToString();
            user.PasswordSalt = "abcde";
            user.IsApproved = false;
            user.LastActivityDate = DateTime.Now;

            db.Users.Add(user);

            var log = audit.CreateLog();
            Assert.NotNull(log);
            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            db.SaveChanges();

            log.Refresh();
            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            var lastLog = audit.LastLog;

        }

        [Fact]
        public void ef_aduit_delete_test()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = new User();
            user.EmailAddress = string.Format("email.{0}@test.com", DateTime.Now.Ticks);
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.PasswordHash = DateTime.Now.Ticks.ToString();
            user.PasswordSalt = "abcde";
            user.IsApproved = false;
            user.LastActivityDate = DateTime.Now;

            db.Users.Add(user);

            var log = audit.CreateLog();
            Assert.NotNull(log);

            db.SaveChanges();

            log.Refresh();
            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }



            var lastLog = audit.LastLog;


            db.Users.Remove(user);

            var deleteLog = audit.CreateLog();
            Assert.NotNull(deleteLog);
        }

        [Fact]
        public void ef_aduit_update_test()
        {
            var db = new TrackerContext();
            var audit = db.BeginAudit();

            var user = new User();
            user.EmailAddress = string.Format("email.{0}@test.com", DateTime.Now.Ticks);
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            user.PasswordHash = DateTime.Now.Ticks.ToString();
            user.PasswordSalt = "abcde";
            user.IsApproved = false;
            user.LastActivityDate = DateTime.Now;

            db.Users.Add(user);

            var log = audit.CreateLog();
            Assert.NotNull(log);


            db.SaveChanges();

            log.Refresh();
            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }


            var lastLog = audit.LastLog;


            user.EmailAddress = string.Format("update.{0}@test.com", DateTime.Now.Ticks);

            var updateLog = audit.CreateLog();
            Assert.NotNull(updateLog);
            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            db.SaveChanges();


        }

        [Fact]
        public void ef_aduit_maintain_across_save_test()
        {
            var db = new TrackerContext();
            var tran = db.Database.BeginTransaction();
            var audit = db.BeginAudit();

            var user = db.Users.Find(1);
            user.Comment = "Testing: " + DateTime.Now.Ticks;

            var task = new Task()
            {
                AssignedId = 1,
                StatusId = 1,
                PriorityId = 2,
                Summary = "Summary: " + DateTime.Now.Ticks,
                CreatedId = 1,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            db.Tasks.Add(task);

            db.SaveChanges();

            Assert.NotNull(audit.LastLog);
            Assert.Equal(2, audit.LastLog.Entities.Count);


            var task2 = db.Tasks.Find(2);
            task2.PriorityId = 2;
            task2.StatusId = 2;
            task2.Summary = "Summary: " + DateTime.Now.Ticks;

            db.SaveChanges();

            Assert.NotNull(audit.LastLog);
            Assert.Equal(3, audit.LastLog.Entities.Count);

            var log = audit.LastLog;
            Assert.NotNull(log);



            foreach (var property in log.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            //undo work
            tran.Rollback();
        }

        [Fact]
        public void ef_aduit_log_with_nullable_relations_test()
        {
            var db = new TrackerContext();
            var tran = db.Database.BeginTransaction();
            var audit = db.BeginAudit();

            var task = new Task()
            {
                AssignedId = 1,
                StatusId = 1,
                PriorityId = 2,
                Summary = "Summary: " + DateTime.Now.Ticks,
                CreatedId = 1,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,

            };
            db.Tasks.Add(task);
            db.SaveChanges();

            foreach (var property in audit.LastLog.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            task.PriorityId = null;
            db.SaveChanges();

            foreach (var property in audit.LastLog.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            task.PriorityId = 1;
            db.SaveChanges();

            foreach (var property in audit.LastLog.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            task.PriorityId = 2;
            db.SaveChanges();

            foreach (var property in audit.LastLog.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            //undo work
            tran.Rollback();
        }

        [Fact]
        public void ef_aduit_log_with_nullable_relations_test2()
        {
            var db = new TrackerContext();
            var tran = db.Database.BeginTransaction();
            var audit = db.BeginAudit();

            var task = new Task()
            {
                AssignedId = 1,
                StatusId = 1,
                Priority = null,
                Summary = "Summary: " + DateTime.Now.Ticks,
                CreatedId = 1,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            db.Tasks.Add(task);

            var entries = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            //var relation = entries.First().RelationshipManager.GetRelatedReference<Priority>("Tracker.SqlServer.CodeFirst.Task_Priority", "Task_Priority_Target");
            //relation.Load();

            db.SaveChanges();

            foreach (var property in audit.LastLog.Entities.SelectMany(e => e.Properties))
            {
                Assert.NotEqual(property.Current, "{error}");
                Assert.NotEqual(property.Original, "{error}");
            }

            //undo work
            tran.Rollback();
        }
    }
}
