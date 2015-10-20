using System;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Redis;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Utils;
using ZSharp.Framework;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Test
{
    public class LockTest : DisposableObject
    {
        private readonly IRedisWrapper redisWrapper;
        private readonly ITestOutputHelper testOutput;

        public LockTest(ITestOutputHelper testOutput)
        {
            ServiceLocator.SetLocatorProvider(UnityConfig.GetConfiguredContainer());
            this.redisWrapper = RedisFactory.GetRedisWrapper();
            this.testOutput = testOutput;
        }

        [Fact]
        public void multiple_clients_to_safely_execute()
        {
            //The number of concurrent clients to run
            const int noOfClients = 10;
            var asyncResults = new List<IAsyncResult>(noOfClients);
            for (var i = 1; i <= noOfClients; i++)
            {
                var clientNo = i;
                var actionFn = (Action)delegate
                {
                    using (LockFactory.GetLock("testlock"))
                    {
                        testOutput.WriteLine("client {0} acquired lock", clientNo);
                        var value = redisWrapper.Get("atomic-counter");
                        int counter = 0;
                        if (value != null)
                        {
                            counter = value.ToString().ToInt(0);
                        }                       

                        //Add an artificial delay to demonstrate locking behaviour
                        Thread.Sleep(RandomHelper.GetRandom(100, 200));

                        redisWrapper.Set("atomic-counter", (counter + 1).ToString());
                        testOutput.WriteLine("client {0} released lock", clientNo);
                    }
                };

                //Asynchronously invoke the above delegate in a background thread
                asyncResults.Add(actionFn.BeginInvoke(null, null));
            }

            //Wait at most 1 minute for all the threads to complete
            asyncResults.WaitAll(TimeSpan.FromMinutes(1));

            //Print out the 'atomic-counter' result
            var result = redisWrapper.Get("atomic-counter").ToString().ToInt();
            testOutput.WriteLine("atomic-counter after 1min: {0}", result);
        }

        [Fact]
        public void acquire_lock_using_Tasks()
        {
            int noOfClients = 10;
            var tasks = new Task[noOfClients];
            for (var i = 0; i < noOfClients; i++)
            {
                tasks[i] = Task.Factory.StartNew((object clientNo) =>
                {
                    try
                    {
                        using (LockFactory.GetLock("testlock1", TimeSpan.FromMinutes(3)))
                        {
                            testOutput.WriteLine("client {0} acquired lock", (int)clientNo);
                            var value = redisWrapper.Get("atomic-counter");
                            int counter = 0;
                            if (value != null)
                            {
                                counter = value.ToString().ToInt(0);
                            }

                            //Add an artificial delay to demonstrate locking behaviour
                            Thread.Sleep(100);

                            redisWrapper.Set("atomic-counter", (counter + 1).ToString());
                            testOutput.WriteLine("client {0} released lock", (int)clientNo);
                        }
                    }
                    catch (Exception e)
                    {
                        testOutput.WriteLine(e.Message);
                    }

                }, i + 1);
            }
            Task.WaitAll(tasks);
        }

        [Fact]
        public void acquire_lock_with_timeout()
        {
            //Initialize and set counter to '1'
            redisWrapper.Increment("atomic-counter");

            //Acquire lock and never release it
            LockFactory.GetLock("testlock");

            var waitFor = TimeSpan.FromSeconds(2);
            var now = DateTime.Now;

            try
            {
                //Attempt to acquire a lock with a 2 second timeout
                using (LockFactory.GetLock("testlock", waitFor))
                {
                    //If lock was acquired this would be incremented to '2'
                    redisWrapper.Increment("atomic-counter");
                }
            }
            catch (TimeoutException tex)
            {
                var timeTaken = DateTime.Now - now;
                testOutput.WriteLine(String.Format("After '{0}', Received TimeoutException: '{1}'", timeTaken, tex.Message));

                var counter = redisWrapper.Get("atomic-counter").ToString().ToInt();
                testOutput.WriteLine(String.Format("atomic-counter remains at '{0}'", counter));
            }
        }

        [Fact]
        public void simulate_lock_timeout()
        {
            var waitFor = TimeSpan.FromMilliseconds(20);
            var loc = LockFactory.GetLock("testlock", waitFor);
            Thread.Sleep(100); //should have lock expire
            using (var newloc = LockFactory.GetLock("testlock", waitFor))
            {
                testOutput.WriteLine("Should Get Lock!");
            }
        }

        [Fact]
        public void simulate_long_running_task_test()
        {
            var key = "testlock1";
            var tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew((object clientNo) => { LongRunningTask(clientNo, key); }, 1);
            Thread.Sleep(1000);
            tasks[1] = Task.Factory.StartNew((object clientNo) => { ShortRunningTask(clientNo, key); }, 2);

            Task.WaitAll(tasks);

            var counter = redisWrapper.Get("atomic-counter").ToString().ToInt();
            testOutput.WriteLine(String.Format("atomic-counter remains at '{0}'", counter));
        }

        private void LongRunningTask(object clientNo, string key)
        {
            try
            {
                using (LockFactory.GetLock(key, TimeSpan.FromSeconds(1)))
                {
                    testOutput.WriteLine("client {0} acquired lock", (int)clientNo);
                    var value = redisWrapper.Get("atomic-counter");
                    int counter = 0;
                    if (value != null)
                    {
                        counter = value.ToString().ToInt(0);
                    }

                    //Add an artificial delay to demonstrate locking behaviour
                    Thread.Sleep(30000);

                    redisWrapper.Set("atomic-counter", (counter + 1).ToString());
                    testOutput.WriteLine("client {0} released lock", (int)clientNo);
                }
            }
            catch (Exception e)
            {
                testOutput.WriteLine(e.Message);
            }
        }

        private void ShortRunningTask(object clientNo, string key)
        {
            try
            {
                using (LockFactory.GetLock(key, TimeSpan.FromSeconds(1)))
                {
                    testOutput.WriteLine("client {0} acquired lock", (int)clientNo);
                    var value = redisWrapper.Get("atomic-counter");
                    int counter = 0;
                    if (value != null)
                    {
                        counter = value.ToString().ToInt(0);
                    }

                    redisWrapper.Set("atomic-counter", (counter + 1).ToString());
                    testOutput.WriteLine("client {0} released lock", (int)clientNo);
                }
            }
            catch (Exception e)
            {
                testOutput.WriteLine(e.Message);
            }
        }       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.redisWrapper.ClearAll();
            }
        }
    }
}
