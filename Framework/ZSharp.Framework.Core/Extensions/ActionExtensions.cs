

using System;
using System.Collections.Generic;
using System.Threading;

namespace ZSharp.Framework.Extensions
{
    public static class ActionExtensions
    {
        public static bool WaitAll(this List<IAsyncResult> asyncResults, TimeSpan timeout)
        {
            var waitHandles = asyncResults.ConvertAll(x => x.AsyncWaitHandle);
            return WaitAll(waitHandles.ToArray(), (int)timeout.TotalMilliseconds);
        }

        public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout)
        {
            return WaitAll(waitHandles, (int)timeout.TotalMilliseconds);
        }

        public static bool WaitAll(WaitHandle[] waitHandles, int timeOutMs)
        {
            // throws an exception if there are no wait handles
            if (waitHandles == null) throw new ArgumentNullException("waitHandles");
            if (waitHandles.Length == 0) return true;

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                // WaitAll for multiple handles on an STA thread is not supported.
                // CurrentThread is ApartmentState.STA when run under unit tests
                var successfullyComplete = true;
                foreach (var waitHandle in waitHandles)
                {
                    successfullyComplete = successfullyComplete
                        && waitHandle.WaitOne(timeOutMs, false);
                }
                return successfullyComplete;
            }

            return WaitHandle.WaitAll(waitHandles, timeOutMs, false);
        }
    }
}
