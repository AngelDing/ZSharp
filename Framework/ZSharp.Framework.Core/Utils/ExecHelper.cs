using System;
using System.Threading;

namespace ZSharp.Framework.Utils
{
    public static class ExecHelper
    {
        public static void RetryUntilTrue(Func<bool> action, TimeSpan? timeOut)
        {
            var i = 0;
            var firstAttempt = DateTimeOffset.UtcNow;

            while (timeOut == null || DateTimeOffset.UtcNow - firstAttempt < timeOut.Value)
            {
                i++;
                if (action())
                {
                    return;
                }
                SleepBackOffMultiplier(i);
            }

            throw new TimeoutException(string.Format("Exceeded timeout of {0}", timeOut.Value));
        }

        public static void RetryOnException(Action action, TimeSpan? timeOut)
        {
            var i = 0;
            Exception lastEx = null;
            var firstAttempt = DateTimeOffset.UtcNow;

            while (timeOut == null || DateTimeOffset.UtcNow - firstAttempt < timeOut.Value)
            {
                i++;
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                    SleepBackOffMultiplier(i);
                }
            }

            throw new TimeoutException(string.Format("Exceeded timeout of {0}", timeOut.Value), lastEx);
        }

        public static void RetryOnException(Action action, int maxRetries)
        {
            for (var i = 0; i < maxRetries; i++)
            {
                try
                {
                    action();
                    break;
                }
                catch
                {
                    if (i == maxRetries - 1)
                    {
                        throw;
                    }

                    SleepBackOffMultiplier(i);
                }
            }
        }

        private static void SleepBackOffMultiplier(int i)
        {
            //exponential/random retry back-off.
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var nextTry = rand.Next(
                (int)Math.Pow(i, 2), (int)Math.Pow(i + 1, 2) + 1);

            Thread.Sleep(nextTry);
        }
    }
}
