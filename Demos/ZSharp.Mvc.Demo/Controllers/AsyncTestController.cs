using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Concurrent;
using System;

namespace ZSharp.Mvc.Demo.Controllers
{
    public class AsyncTestController : Controller
    {
        private ConcurrentDictionary<string, DateTime> debugStrings = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// ManagedThreadId3同ManagedThreadId1和ManagedThreadId2，不屬於同一線程，這個說明:
        /// 在調用異步方法等待返回結果的這段時間內，請求線程是釋放會了線程池的，當所有異步方法都處理完成，系統會自動從線程池中隨機
        /// 拿一個線程繼續後續處理，隨機的進程可能是開始的請求線程，也可能是最後一個異步處理的線程；
        /// </summary>
        [AsyncTimeout(3000)]
        [Route("")]
        [HttpGet]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            debugStrings.TryAdd("Thread.CurrentThread.ManagedThreadId1:" + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            var stopwatch = Stopwatch.StartNew();
            var task1 = GetName1();
            var task2 = GetName2();
            var task3 = GetName3();
            debugStrings.TryAdd("Thread.CurrentThread.ManagedThreadId2:" + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            await Task.WhenAll(task1, task2, task3);
            var result = task1.Result + task2.Result + task3.Result;
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            debugStrings.TryAdd("TotalTime :" + time, DateTime.Now);
            debugStrings.TryAdd("Result:" + result, DateTime.Now);
            debugStrings.TryAdd("Thread.CurrentThread.ManagedThreadId3:" + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            var result2 = debugStrings.OrderBy(p => p.Value).Select(p => p.Key).ToList();
            return View(result2);
        }

        //[Route("")]
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    debugStrings.Add("Thread.CurrentThread.ManagedThreadId1:" + Thread.CurrentThread.ManagedThreadId);
        //    var stopwatch = Stopwatch.StartNew();
        //    GetName4();
        //    GetName5();
        //    GetName6();
        //    stopwatch.Stop();
        //    debugStrings.Add("Thread.CurrentThread.ManagedThreadId2:" + Thread.CurrentThread.ManagedThreadId);
        //    var time = stopwatch.ElapsedMilliseconds;
        //    debugStrings.Add("TotalTime :" + time);

        //    return View(debugStrings.ToList());
        //}


        private string GetName4()
        {
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(300);
                debugStrings.TryAdd("AAA" + i, DateTime.Now);
            }
            debugStrings.TryAdd("'GetName1' Thread Id: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            return "Jesse";
        }

        private string GetName5()
        {
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(400);
                debugStrings.TryAdd("BBBB" + i, DateTime.Now);
            }
            debugStrings.TryAdd("'GetName2' Thread Id: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            return "Jesse";
        }

        private string GetName6()
        {
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                debugStrings.TryAdd("CCCC", DateTime.Now);
            }
            debugStrings.TryAdd("'GetName3' Thread Id: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            return "Jesse";
        }

        private async Task<string> GetName1(CancellationToken cancelToken = default(CancellationToken))
        {
            // 这里还是主线程
            debugStrings.TryAdd("GetName1 Before calling Task.Run, current thread Id is: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            return await Task.Run(() =>
            {
                //Thread.Sleep(1000);
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(300);
                    debugStrings.TryAdd("AAA" + i, DateTime.Now);
                }
                debugStrings.TryAdd("'GetName1' Thread Id: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                return "Jesse";
            }, cancelToken);
        }

        private async Task<string> GetName2()
        {
            // 这里还是主线程
            debugStrings.TryAdd("GetName2: Before calling Task.Run, current thread Id is: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            return await Task.Run(() =>
            {
                //Thread.Sleep(1000);
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(400);
                    debugStrings.TryAdd("BBBB" + i, DateTime.Now);
                }
                debugStrings.TryAdd("'GetName2' Thread Id: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                return "Jesse";
            });
        }

        private async Task<string> GetName3()
        {
            // 这里还是主线程
            debugStrings.TryAdd("GetName3: Before calling Task.Run, current thread Id is: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
            return await Task.Run(() =>
            {
                //Thread.Sleep(1000);
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(500);
                    debugStrings.TryAdd("CCCC" + i, DateTime.Now);
                }
                debugStrings.TryAdd("'GetName3' Thread Id: " + Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                return "Jesse";
            });
        }
    }
}