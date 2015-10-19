using System;
using System.Collections.Generic;
using System.Threading;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Redis;
using ZSharp.Framework.Extensions;

namespace Framework.ConsoleTest
{
    public class DistributedLockTest
    {
        private readonly IRedisWrapper redisWrapper;

        public DistributedLockTest()
        {
            this.redisWrapper = RedisFactory.GetRedisWrapper();
        }

        public void MultipleClientsGetSameLock()
        {
            //The number of concurrent clients to run
            const int noOfClients = 5;
            var asyncResults = new List<IAsyncResult>(noOfClients);
            for (var i = 1; i <= noOfClients; i++)
            {
                var clientNo = i;
                var actionFn = (Action)delegate
                {
                    using (DistributedLockFactory.GetLock("testlock"))
                    {
                        Console.WriteLine("client {0} acquired lock", clientNo);
                        var counter = redisWrapper.Get("atomic-counter").ToString().ToInt();

                        //Add an artificial delay to demonstrate locking behaviour
                        Thread.Sleep(100);

                        redisWrapper.Set("atomic-counter", (counter + 1).ToString());
                        Console.WriteLine("client {0} released lock", clientNo);
                    }
                };

                //Asynchronously invoke the above delegate in a background thread
                asyncResults.Add(actionFn.BeginInvoke(null, null));
            }

            //Wait at most 1 second for all the threads to complete
            asyncResults.WaitAll(TimeSpan.FromSeconds(1));

            //Print out the 'atomic-counter' result
            var result = redisWrapper.Get("atomic-counter").ToString().ToInt();
            Console.WriteLine("atomic-counter after 1sec: {0}", result);
        }
    }
}
