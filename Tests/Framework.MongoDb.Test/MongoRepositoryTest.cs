using Xunit;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using ZSharp.Framework.Infrastructure;

namespace Framework.MongoDb.Test
{
    public class MongoRepositoryTest : BaseMongoTest
    {
        [Fact]
        public void mongo_add_and_update_test()
        {
            var customerRepo = new MongoTestDB<Customer2>();

            var customer = CreateCustomer2();
            customerRepo.Insert(customer);

            customer.Id.Should().NotBeNull();
            var alreadyAddedCustomer = customerRepo.GetBy(c => c.FirstName == "Bob").FirstOrDefault();
            alreadyAddedCustomer.Should().NotBeNull();
            alreadyAddedCustomer.FirstName.Should().Be(customer.FirstName);
            alreadyAddedCustomer.HomeAddress.Address1.Should().Be(customer.HomeAddress.Address1);
            alreadyAddedCustomer.CreateDate.Should().BeBefore(DateTime.Now);

            alreadyAddedCustomer.Phone = "10110111";
            alreadyAddedCustomer.Email = "dil.bob@fastmail.org";

            customerRepo.Update(alreadyAddedCustomer);

            var updatedCustomer = customerRepo.GetByKey(alreadyAddedCustomer.Id);
            updatedCustomer.Should().NotBeNull();
            updatedCustomer.Phone.Should().Be(alreadyAddedCustomer.Phone);
            updatedCustomer.Email.Should().Be(alreadyAddedCustomer.Email);
            var isExists = customerRepo.Exists(c => c.HomeAddress.Country == "Alaska");
            isExists.Should().Be(true);
        }

        [Fact]
        public void mongo_complex_entity_test()
        {
            var customerRepo = new MongoTestDB<Customer>();
            var productRepo = new MongoTestDB<Product>();

            var customer = new Customer();
            customer.FirstName = "Erik";
            customer.LastName = "Swaun";
            customer.Phone = "123 99 8767";
            customer.Email = "erick@mail.com";
            customer.HomeAddress = new Address
            {
                Address1 = "Main bulevard",
                Address2 = "1 west way",
                PostCode = "89560",
                City = "Tempare",
                Country = "Arizona"
            };

            var order = new Order();
            order.PurchaseDate = DateTime.Now.AddDays(-2);
            var orderItems = new List<OrderItem>();

            var shampoo = new Product() { Name = "Palmolive Shampoo", Price = 5 };
            productRepo.Insert(shampoo);
            var paste = new Product() { Name = "Mcleans Paste", Price = 4 };
            productRepo.Insert(paste);


            var item1 = new OrderItem { Product = shampoo, Quantity = 1, Price = 100.07M };
            var item2 = new OrderItem { Product = paste, Quantity = 2, Price = 105.9M };

            orderItems.Add(item1);
            orderItems.Add(item2);

            order.Items = orderItems;

            customer.Orders = new List<Order>
            {
                order
            };

            customerRepo.Insert(customer);
            customer.Id.Should().NotBeNull();
            customer.Orders[0].Items[0].Product.Id.Should().NotBeNull();

            var theOrders = customerRepo.GetBy(c => c.Id == customer.Id).Select(c => c.Orders).ToList();            
            theOrders.Should().NotBeNull();

            var theOrder = theOrders[0].FirstOrDefault();
            theOrder.Should().NotBeNull();

            var items = theOrder.Items;
            items.Should().NotBeNull();

            items.FirstOrDefault().Price.Should().Be(100.07M);  
        }

        [Fact]
        public void mongo_batch_test()
        {
            var customerRepo = new MongoTestDB<Customer>();

            var custlist = new List<Customer>(new Customer[] {
                new Customer() { FirstName = "Jacky Customer A" },
                new Customer() { FirstName = "Jacky Client B" },
                new Customer() { FirstName = "Jacky Customer C" },
                new Customer() { FirstName = "Jacky Client D" },
                new Customer() { FirstName = "Jacky Customer E" },
                new Customer() { FirstName = "Jacky Client F" },
                new Customer() { FirstName = "Jacky Customer G" },
            });

            //Insert batch
            customerRepo.Insert(custlist);
            var count = customerRepo.GetBy(p => p.FirstName.Contains("Jacky")).Count();
            count.Should().Be(7);

            foreach (Customer c in custlist)
            {
                c.Id.Should().NotBe(new string('0', 24));
            }

            //Update batch
            foreach (Customer c in custlist)
            {
                c.LastName = c.FirstName;
            }
            customerRepo.Update(custlist);

            var updateList = customerRepo.GetBy(p => p.LastName.Contains("Jacky"));

            foreach (Customer c in updateList)
            {
                c.LastName.Should().Be(c.FirstName);
            }

            //Delete by criteria
            customerRepo.Delete(f => f.FirstName.StartsWith("Jacky Client"));

            count = customerRepo.GetBy(p => p.FirstName.Contains("Jacky")).Count();
            count.Should().Be(4);

            //Delete specific object
            customerRepo.Delete(custlist[0]);

            //Test AsQueryable
            var selectedcustomers = customerRepo.GetBy(cust => cust.LastName.StartsWith("Jacky"));

            selectedcustomers.ToList().Count.Should().Be(3);
            
            customerRepo.Delete(f => f.FirstName.StartsWith("Jacky"));
        }

        [Fact]
        public void mongo_update_assign_field()
        {
            var customerRepo = new MongoTestDB<Customer3>();

            var customer = CreateCustomer3();
            customerRepo.Insert(customer);

            var updateFields = new Dictionary<string, object>();
            updateFields.Add("FirstName", "JiaJia");
            updateFields.Add("Phone", "123");

            customerRepo.Update(q => q.Id == customer.Id, updateFields);
            var updatedCustomer = customerRepo.GetByKey(customer.Id);
            updatedCustomer.FirstName.Should().Be("JiaJia");
            updatedCustomer.Phone.Should().Be("123");
        }

        [Fact]
        public void mongo_task_entity_insert_test()
        {
            var dateTime = DateTime.Now;
            var timeSpan = new TimeSpan(1, 1, 1);
            var task = GetMongoTaskEntity(dateTime, timeSpan);

            var repo = new MongoTestDB<MongoTaskEntity>();
            repo.Insert(task);
            var id = task.Id;
            var addedTask = repo.GetByKey(id);
            addedTask.TaskStatusEntity.LastRunTime.Millisecond.Should().Be(dateTime.Millisecond);
            addedTask.TaskStatusEntity.LastRunTime.Second.Should().Be(dateTime.Second);
            addedTask.TaskEntity.StartTime.Should().Be(timeSpan);
        }

        [Fact]
        public void mongo_task_entity_update_immediate_test()
        {
            var dateTime = DateTime.Now;
            var timeSpan = new TimeSpan(1, 1, 1);
            var task = GetMongoTaskEntity2(dateTime, timeSpan);

            var repo = new MongoTestDB<MongoTaskEntity2>();
            repo.Insert(task);
            var id = task.Id;

            var newTimeSpan = new TimeSpan(3, 3, 3);
            var newDateTime = dateTime.AddDays(-1);
            var newName = "newMongoTest";

            var dic = new Dictionary<string, object>();
            dic.Add("TaskEntity.StartTime", newTimeSpan);
            dic.Add("TaskStatusEntity.LastRunTime", newDateTime);
            dic.Add("Name", newName);

            repo.Update(p => p.Id == id, dic);
            var updatedTask = repo.GetByKey(id);
            updatedTask.TaskEntity.StartTime.Should().Be(newTimeSpan);
            updatedTask.TaskStatusEntity.LastRunTime.Day.Should().Be(dateTime.AddDays(-1).Day);
            updatedTask.Name.Should().Be(newName);
        }

        [Fact]
        public void mongo_task_entity_update_by_express_test()
        {
            var dateTime = DateTime.Now;
            var timeSpan = new TimeSpan(1, 1, 1);
            var task = GetMongoTaskEntity3(dateTime, timeSpan);

            var repo = new MongoTestDB<MongoTaskEntity3>();
            repo.Insert(task);
            var id = task.Id;

            var newTimeSpan = new TimeSpan(3, 3, 3);
            var newDateTime = dateTime.AddDays(-1);
            var newName = "newMongoTest";

            task.SetUpdate(() => task.TaskEntity.StartTime, newTimeSpan);
            task.SetUpdate(() => task.TaskStatusEntity.LastRunTime, newDateTime);
            task.SetUpdate(() => task.Name, newName);

            repo.Update(p => p.Id == id, task);

            var updatedTask = repo.GetByKey(id);
            updatedTask.Should().NotBeNull();
            updatedTask.TaskEntity.StartTime.Should().Be(newTimeSpan);
            updatedTask.TaskStatusEntity.LastRunTime.Day.Should().Be(dateTime.AddDays(-1).Day);
            updatedTask.Name.Should().Be(newName);
        }

        [Fact]
        public void mongo_nullable_datetime_test()
        {
            var repo = new MongoTestDB<LogEntity>();
            repo.RemoveAll();
            var log = new LogEntity
            {
                CreateDate = DateTime.Now,
                Price = 300,
                Amount = 200,
                CreatedBy = "Jia",
                Details = new List<MyTestEntity>
                {
                    new MyTestEntity
                    {
                        A = "A",
                        B = DateTime.Today,
                        C = 100
                    },
                    new MyTestEntity
                    {
                        A = "AA",
                        B = DateTime.Today,
                        C = 200,
                        D = DateTime.Today
                    },
                }
            };

            repo.Insert(log);

            var result = repo.GetBy(p => p.CreatedBy == "Jia").LastOrDefault();
            result.Should().NotBeNull();
            var list = result.Details.ToList();
            var first = list.FirstOrDefault();
            first.B.Should().Be(DateTime.Today);
            first.D.Should().Be(null);

            result.Details.LastOrDefault().D.Should().Be(DateTime.Today);

            result = repo.GetBy(p => p.Price > 100 && p.Amount == 200).FirstOrDefault();
            result.Should().NotBeNull();
        }

        #region Private Method

        private MongoTaskEntity GetMongoTaskEntity(DateTime dateTime, TimeSpan timeSpan)
        {
            var task = new MongoTaskEntity
            {
                Name = "MongoTest",
                TaskStatusEntity = new TaskStatusEntity
                {
                    LastRunTime = dateTime
                },
                TaskEntity = new TaskEntity
                {
                    StartTime = timeSpan
                }
            };
            return task;
        }

        private MongoTaskEntity2 GetMongoTaskEntity2(DateTime dateTime, TimeSpan timeSpan)
        {
            var task1 = GetMongoTaskEntity(dateTime, timeSpan);
            return task1.Map<MongoTaskEntity2>();
        }

        private MongoTaskEntity3 GetMongoTaskEntity3(DateTime dateTime, TimeSpan timeSpan)
        {
            var task1 = GetMongoTaskEntity(dateTime, timeSpan);
            return task1.Map<MongoTaskEntity3>();
        }

        private Customer2 CreateCustomer2()
        {
            var customer = new Customer2();
            customer = CreateBaseInfo(customer) as Customer2;
            return customer;
        }

        private Customer3 CreateCustomer3()
        {
            var customer = new Customer3();
            customer = CreateBaseInfo(customer) as Customer3;
            return customer;
        }

        private BaseCustomer CreateBaseInfo(BaseCustomer customer)
        {
            customer.FirstName = "Bob";
            customer.LastName = "Dillon";
            customer.Phone = "0900999899";
            customer.Email = "Bob.dil@snailmail.com";
            customer.CreateDate = DateTime.Now;
            customer.HomeAddress = new Address
            {
                Address1 = "North kingdom 15 west",
                Address2 = "1 north way",
                PostCode = "40990",
                City = "George Town",
                Country = "Alaska"
            };
            return customer;
        }

        #endregion
    }
}
