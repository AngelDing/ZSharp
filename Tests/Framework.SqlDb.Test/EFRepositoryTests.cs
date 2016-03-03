using System.Linq;
using System.Data.Entity;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using ZSharp.Framework.Entities;
using ZSharp.Framework;

namespace Framework.SqlDb.Test
{
    public class EFRepositoryTests : DisposableObject
    {
        public EFRepositoryTests()
        {
            InitializeEFTestDB();
        }

        private void InitializeEFTestDB()
        {
            Database.SetInitializer<EFTestContext>(new DropCreateDatabaseIfModelChanges<EFTestContext>());
            using (var context = new EFTestContext())
            {
                context.Database.Initialize(true);
            }
            using (var context = new EFTestContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }

                context.Database.Create();
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (var context = new EFTestContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
            }
        }

        [Fact]
        public void ef_save_aggregate_root_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);

            var custList = new List<EFCustomer>();           
            using (var newRepo = new CustomerRepository())
            {
                custList = newRepo.GetAll().ToList();
            }
            custList.Count.Should().Be(1);
            custList.First().UserName.Should().Be("jacky.zhou");
            custList.First().Id.Should().BeGreaterThan(0);

            using (var newRepo = new CustomerRepository())
            {
                var newCustomer = newRepo.GetCustomerByName("jacky.zhou");
                var list = newRepo.GetNoteList();
            }
        }     

        [Fact]
        public void ef_batch_test()
        {
            var customers = GetCustomerList();

            var getByList = new List<EFCustomer>();
            var allList = new List<EFCustomer>();
            var updateList = new List<EFCustomer>();
            var deleteList = new List<EFCustomer>();
            var cId = 0;
            bool isExists;
            using (var repo = new CustomerRepository())
            {
                repo.Insert(customers);
                repo.RepoContext.Commit();

                getByList = repo.GetBy(p => p.UserName.StartsWith("d") && p.Password != "dd").ToList();
                allList = repo.GetAll().ToList();
                cId = allList.Find(p => p.UserName == "dd").Id;

                foreach (var c in allList)
                {
                    if (c.Id == 1)
                    {
                        c.UserName = c.UserName + "123";
                    }
                    else
                    {
                        c.SetUpdate(() => c.UserName, c.UserName + "123");
                    }
                }
                repo.Update(allList);
                repo.RepoContext.Commit();

                updateList = repo.GetAll().ToList();

                repo.Delete(customers[0]);
                repo.RepoContext.Commit();
                repo.Delete(p => p.Id == cId);
                repo.RepoContext.Commit();
                repo.Delete(p => p.Password == "bb");
                repo.RepoContext.Commit();
                deleteList = repo.GetAll().ToList();
                isExists = repo.Exists(p => p.Password == "cc");
            }

            getByList.Count().Should().Be(1);
            allList.Count.Should().Be(5);
            foreach (var c in allList)
            {
                c.Id.Should().BeGreaterThan(0);
            }
            foreach (var c in updateList)
            {
                c.UserName.Should().Contain("123");
            }
            deleteList.Count().Should().Be(2);
            isExists.Should().Be(true);
        }

        [Fact]
        public void ef_spec_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);

            var custList = new List<EFCustomer>();
            var cQuery = new CustomerQueryCriteria
            {
                UserName = "jacky.zhou"
            };
            var spec = CustomerSpecification.GetCustomerByFilter(cQuery);

            using (var repo = new CustomerRepository())
            {              
                custList = repo.GetBy(spec).ToList();               
            }

            custList.Count.Should().Be(1);
            custList.First().UserName.Should().Be("jacky.zhou");
            custList.First().Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ef_multi_commit_test()
        {
            var customer = GetCustomerInfo();
            using (var repo = new CustomerRepository())
            {
                repo.Insert(customer);
                repo.RepoContext.Commit();
                customer.SetUpdate(() => customer.UserName, "jiajia");
                customer.EFNote.Add(new EFNote { NoteText = "XXX", CustomerId = 1, ObjectState = ObjectStateType.Added });
                repo.Update(customer);
                repo.RepoContext.Commit();
            }

            using (var repo = new CustomerRepository())
            {
                var notes = repo.GetNoteList();
                notes.Count.Should().Be(3);
            }
        }

        #region Update

        [Fact]
        public void ef_update_by_manual_compare_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.Email = "jacky@ZSharp.com";
            cInfo.Address.City = "SZ";
            cInfo.Address.Zip = "000000000";
            cInfo.ObjectState = ObjectStateType.Modified;
            cInfo.EFNote.First().NoteText = "DDDD";
            cInfo.EFNote.First().ObjectState = ObjectStateType.Modified;
            cInfo.EFNote.Last().ObjectState = ObjectStateType.Deleted;

            var newNote = new EFNote
            {
                CustomerId = 1, 
                NoteText = "CCCC", 
                ObjectState = ObjectStateType.Added 
            };
            cInfo.EFNote.Add(newNote);

            using (var repo = new CustomerRepository())
            {
                repo.Update(cInfo);
                repo.RepoContext.Commit();
            }

            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.Email.Should().Be("jacky@ZSharp.com");
            var address = cInfo.Address;
            address.City.Should().Be("SZ");
            address.Zip.Should().Be("000000000");
            cInfo.EFNote.Count.Should().Be(2);
            cInfo.EFNote.Last().NoteText.Should().Be("CCCC");
            cInfo.EFNote.First().NoteText.Should().Be("DDDD");
        }

        [Fact]
        public void ef_update_by_partial_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.SetUpdate(() => cInfo.Address.City, "SZ");
            cInfo.SetUpdate(() => cInfo.Address.Zip, "000000000");
            cInfo.SetUpdate(() => cInfo.Email, "jacky@ZSharp.com");
            
            var firstNote = cInfo.EFNote.First();
            firstNote.SetUpdate(() => firstNote.NoteText, "DDDD");
            
            using (var repo = new CustomerRepository())
            {
                repo.Update(cInfo);
                repo.RepoContext.Commit();
            }

            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }
            cInfo.Email.Should().Be("jacky@ZSharp.com");
            var address = cInfo.Address;
            address.City.Should().Be("SZ");
            address.Zip.Should().Be("000000000");
            cInfo.EFNote.Count.Should().Be(2);
            cInfo.EFNote.Last().NoteText.Should().Be("BB");
            cInfo.EFNote.First().NoteText.Should().Be("DDDD");
        }

        #endregion

        #region Delete

        [Fact]
        public void ef_delete_by_key_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            using (var repo = new CustomerRepository())
            {
                repo.Delete(p => p.Id == 1);
                repo.RepoContext.Commit();
                var notes = repo.GetNoteList();
                var cInfo = repo.GetBy(p => p.Id == 1).FirstOrDefault();
                cInfo.Should().BeNull();
                notes.Count.Should().Be(0);
            }

            using (var repo = new CustomerRepository())
            {
                var notes = repo.GetNoteList();
                var cInfo = repo.GetBy(p => p.Id == 1).FirstOrDefault();
                cInfo.Should().BeNull();
                notes.Count.Should().Be(0);
            }
        }

        [Fact]
        public void ef_delete_by_entity_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            using (var repo = new CustomerRepository())
            {
                var cInfo = repo.GetBy(p => p.Id == 1).FirstOrDefault();
                if (cInfo != null)
                {
                    repo.Delete(cInfo);
                    repo.RepoContext.Commit();
                }
            }
            using (var repo = new CustomerRepository())
            {
                var cInfo = repo.GetBy(p => p.Id == 1).FirstOrDefault();
                var notes = repo.GetNoteList();
                cInfo.Should().BeNull();
                notes.Count.Should().Be(0);
            }
        }


        [Fact]
        public void ef_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);

            EFCustomer newCustomer;
            using (var repo = new CustomerRepository())
            {
                newCustomer = repo.GetCustomFullInfo(1);
                newCustomer.Should().NotBeNull();

                newCustomer.SetUpdate(() => newCustomer.UserName, "Jacky");
                var note = newCustomer.EFNote.FirstOrDefault();
                note.SetUpdate(() => note.NoteText, "XXX");
                repo.Update(newCustomer);
                repo.RepoContext.Commit();
            }

            using (var repo = new CustomerRepository())
            {
                newCustomer = repo.GetCustomFullInfo(1);
                newCustomer.Should().NotBeNull();
                newCustomer.UserName.Should().Be("Jacky");
                newCustomer.EFNote.FirstOrDefault().NoteText.Should().Be("XXX");
            }
        }

        #endregion

        #region Private Methods

        private EFCustomer GetCustomerInfo()
        {
            var customer = new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "jacky.zhou",
                Phone = "111111",
                Password = "123456",
                EFNote = new List<EFNote> { new EFNote { NoteText = "AA" }, new EFNote { NoteText = "BB" } }
            };
            return customer;
        }

        private void InsertNewCustomer(EFCustomer customer)
        {
            using (var custRepo = new CustomerRepository())
            {
                custRepo.Insert(customer);
                custRepo.RepoContext.Commit();
            }
        }

        private List<EFCustomer> GetCustomerList()
        {
            List<EFCustomer> customers = new List<EFCustomer>{new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "daxnet",
                Password = "123456"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "aa",
                Password = "aa"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "bb",
                Password = "bb"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "cc",
                Password = "cc"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "dd",
                Password = "dd"
            }};
            return customers;
        }

        #endregion
    }
}
