using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.ConsoleTest
{
    public static class AsyncTest
    {
        #region 1
        //static void Main()
        //{
        //    new Thread(Go).Start();     // .NET 1.0开始就有的
        //    Task.Factory.StartNew(Go);  // .NET 4.0 引入了 TPL
        //    Task.Run(new Action(Go));   // .NET 4.5 新增了一个Run的方法

        //    Console.ReadLine();
        //}

        //public static void Go()
        //{
        //    var threadId = Thread.CurrentThread.ManagedThreadId;
        //    Console.WriteLine("我是另一个线程,Thread Id {0}", threadId);
        //}

        #endregion

        #region 2
        //static void Main()
        //{
        //    Console.WriteLine("我是主线程：Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
        //    ThreadPool.QueueUserWorkItem(Go);

        //    Console.ReadLine();
        //}

        //public static void Go(object data)
        //{
        //    Console.WriteLine("我是另一个线程:Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
        //}
        #endregion 

        #region 3
        //private static bool _isDone = false;
        //private static object _lock = new object();
        //static void Main()
        //{
        //    new Thread(Done).Start();
        //    new Thread(Done).Start();
        //    new Thread(Done).Start();
        //    new Thread(Done).Start();
        //    Console.ReadLine();
        //}

        //static void Done()
        //{
        //    if (!_isDone)
        //    {
        //        lock (_lock)
        //        {
        //            Console.WriteLine("我是另一个线程:Thread Id {0}", Thread.CurrentThread.ManagedThreadId);
        //            if (!_isDone)
        //            {
        //                Console.WriteLine("Done"); // 猜猜这里面会被执行几次？
        //                _isDone = true;
        //            }
        //        }
        //    }
        //    Console.WriteLine("XXXXX");
        //}
        #endregion

        #region 4
        //static SemaphoreSlim _sem = new SemaphoreSlim(3);    // 我们限制能同时访问的线程数量是3
        //static void Main()
        //{
        //    for (int i = 1; i <= 5; i++) new Thread(Enter).Start(i);
        //    Console.ReadLine();
        //}

        //static void Enter(object id)
        //{
        //    Console.WriteLine(id + " 开始排队...");
        //    _sem.Wait();
        //    Console.WriteLine(id + " 开始执行！");
        //    Thread.Sleep(1000 * (int)id);
        //    Console.WriteLine(id + " 执行完毕，离开！");
        //    _sem.Release();
        //}
        #endregion 

        #region 5
        //public static void Main()
        //{
        //    try
        //    {
        //        //var task = Task.Run(() => { Go(); });
        //        //task.Wait();  // 在调用了这句话之后，主线程才能捕获task里面的异常

        //        // 对于有返回值的Task, 我们接收了它的返回值就不需要再调用Wait方法了
        //        // GetName 里面的异常我们也可以捕获到
        //        var task2 = Task.Run(() => { return GetName(); });
        //        var name = task2.Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception!");
        //    }

        //    Console.ReadLine();
        //}
        //static void Go() { throw null; }
        //static string GetName() { throw null; }

        #endregion

        #region 6
        public static void Test6()
        {
            Console.WriteLine("Start Main Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            Test();
            Console.WriteLine("End Main Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
        }

        static async Task Test()
        {
            Console.WriteLine("Before calling GetName, Thread Id: {0}\r\n", Thread.CurrentThread.ManagedThreadId);
            var name = GetName();   //我们这里没有用 await,所以下面的代码可以继续执行
            // 但是如果上面是 await GetName()，下面的代码就不会立即执行，输出结果就不一样了。

            Thread.Sleep(2000);
            Console.WriteLine("End calling GetName.\r\n");
            Console.WriteLine("Get result from GetName: {0}", await name);
        }

        static async Task<string> GetName()
        {
            // 这里还是主线程
            Console.WriteLine("Before calling Task.Run, current thread Id is: {0}", Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() =>
            {
                //Thread.Sleep(1000);
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("AAA");
                }
                Console.WriteLine("'GetName' Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return "Jesse";
            });
        }
        #endregion
    }
}
