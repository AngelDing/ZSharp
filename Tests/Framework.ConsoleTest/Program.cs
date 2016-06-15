
using System;
using ZSharp.Framework.Infrastructure;

namespace Framework.ConsoleTest
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    //DistributedLockTest();
        //    //LoggerTest.Test();
        //    //new StringTest().Test();

        //    //PhoneTest.MainTest();
        //    //BugTracker.MainTest();
        //    //SwitchTest.MainTest();
        //    AsyncTest.Test6();


        //    Console.ReadLine();
        //}

        private static void InitUnityConfig()
        {
            ServiceLocator.SetLocatorProvider(UnityConfig.GetConfiguredContainer());
        }

        private static void DistributedLockTest()
        {
            InitUnityConfig();
            var lockTest = new LockTest();
            lockTest.MultipleClientsGetSameLock();
        }
    }
}
