
using System;
using System.Threading.Tasks;
namespace Framework.ConsoleTest
{
    public class StringTest
    {
        public void Test()
        {
             const string str1 = "hello";
            var str2 = Console.ReadLine();
            var str3 = Console.ReadLine();
            var str4 = Console.ReadLine();

            Console.WriteLine("1.比较编译时常量：{0}", ReferenceEquals(str1, "hello"));
            Console.WriteLine("2.比较编译时常量和运行时实例：{0}", ReferenceEquals(str1, str2));
            Console.WriteLine("3.比较两个运行时实例：{0}", ReferenceEquals(str2, str3));
            Console.WriteLine("4.比较空字符串的编译时常量：{0}", ReferenceEquals(string.Empty, ""));
            Console.WriteLine("5.比较空字符串的编译时常量和运行时实例：{0}", ReferenceEquals(str4, ""));
            Console.WriteLine("6.比较两个相同的编译时常量调用方法：{0}", ReferenceEquals(GetStringAsLock(str1), GetStringAsLock(str1)));
            Console.WriteLine("7.用Intern方法获取运行时实例：{0}", ReferenceEquals(string.Intern(str2), string.Intern(str3)));
            Console.WriteLine("8.比较编译时常量调用方法1：{0}", ReferenceEquals(GetStringAsLock(str1), GetStringAsLock(str1)));
            Console.WriteLine("9.比较编译时常量调用方法2：{0}", ReferenceEquals(GetStringAsLock(string.Empty), GetStringAsLock(string.Empty)));

            Task.Factory.StartNew(() =>
            {
                var lockStr = string.Format("global::sign::{0}", str2);
                lock (lockStr)
                {
                    Console.WriteLine("线程1开始");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("线程1结束");
                }
            });

            Task.Factory.StartNew(() =>
            {
                var lockStr = string.Format("global::sign::{0}", str2);
                lock (lockStr)
                {
                    Console.WriteLine("线程2开始");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("线程2结束");
                }
            });

            Task.Delay(2000).Wait();

            Task.Factory.StartNew(() =>
            {
                var lockStr = string.Intern(string.Format("global::sign::{0}", str2));
                lock (lockStr)
                {
                    Console.WriteLine("新线程1开始");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("新线程1结束");
                }
            });

            Task.Factory.StartNew(() =>
            {
                var lockStr = string.Intern(string.Format("global::sign::{0}", str2));
                lock (lockStr)
                {
                    Console.WriteLine("新线程2开始");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("新线程2结束");
                }
            });

            Console.WriteLine("End");
            Console.ReadKey();
        }

        private string GetStringAsLock(string s)
        {
            const string prefx = "global::sign::{0}";
            return prefx + s;
        }
    }
}
