using System;
using System.Linq;
using System.Transactions;
using Xunit;
using Framework.Test.Core;
using ZSharp.Framework.EfExtensions;

namespace Framework.EfExtension.Test
{
    [TestCaseOrderer("Framework.Test.Core.PriorityOrderer", "Framework.Test.Core")]
    public class ExtensionTest
    {
        [Fact, TestPriority(0)]
        public void ef_batch_begin_transaction_test()
        {
            using (var db = new TrackerContext())
            using (var tx = db.Database.BeginTransaction())
            {
                string emailDomain = "@test.com";

                var query = db.Users.Where(u => u.EmailAddress.Contains(emailDomain));
                int count = query.Update(u => new User { IsApproved = false });

                count = db.Users
                    .Where(u => u.IsApproved == false)
                    .Delete();

                tx.Commit();
            }
        }

        [Fact, TestPriority(1)]
        public void ef_batch_no_transaction_test()
        {
            using (var db = new TrackerContext())
            {
                string emailDomain = "@test1.com";

                int count = db.Users
                    .Where(u => u.EmailAddress.EndsWith(emailDomain))
                    .Update(u => new User { LastActivityDate = DateTime.Now });

                count = db.Users
                    .Where(u => u.EmailAddress.EndsWith(emailDomain))
                    .Delete();
            }
        }

        [Fact, TestPriority(2)]
        public void ef_batch_transaction_scope_test()
        {
            using (var tx = new TransactionScope())
            using (var db = new TrackerContext())
            {
                string emailDomain = "@test2.com";

                int count = db.Users
                    .Where(u => u.EmailAddress.EndsWith(emailDomain))
                    .Update(u => new User { IsApproved = false, LastActivityDate = DateTime.Now });

                count = db.Users
                    .Where(u => u.EmailAddress.EndsWith(emailDomain) && u.IsApproved == false)
                    .Delete();

                tx.Complete();
            }
        }
    }
}
