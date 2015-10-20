using System;
using System.Collections.Generic;
using System.Threading;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Redis;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Utils;

namespace Framework.ConsoleTest
{
    public class LockTest
    {
        private readonly IRedisWrapper redisWrapper;

        public LockTest()
        {
            this.redisWrapper = RedisFactory.GetRedisWrapper();
        }

        public void MultipleClientsGetSameLock()
        {
            //The number of concurrent clients to run
            const int noOfClients = 64;
            var asyncResults = new List<IAsyncResult>(noOfClients);
            for (var i = 1; i <= noOfClients; i++)
            {
                var clientNo = i;
                var actionFn = (Action)delegate
                {
                    using (LockFactory.GetLock("testlock"))
                    {
                        Console.WriteLine("client {0} acquired lock", clientNo);
                        var value = redisWrapper.Get("atomic-counter");
                        int counter = 0;
                        if (value != null)
                        {
                            counter = value.ToString().ToInt(0);
                        }                       

                        //Add an artificial delay to demonstrate locking behaviour
                        Thread.Sleep(RandomHelper.GetRandom(100, 200));

                        redisWrapper.Set("atomic-counter", (counter + 1).ToString());
                        Console.WriteLine("client {0} released lock", clientNo);
                    }
                };

                //Asynchronously invoke the above delegate in a background thread
                asyncResults.Add(actionFn.BeginInvoke(null, null));
            }

            //Wait at most 1 minute for all the threads to complete
            asyncResults.WaitAll(TimeSpan.FromMinutes(1));

            //Print out the 'atomic-counter' result
            var result = redisWrapper.Get("atomic-counter").ToString().ToInt();
            Console.WriteLine("atomic-counter after 1min: {0}", result);
        }
    }
}
