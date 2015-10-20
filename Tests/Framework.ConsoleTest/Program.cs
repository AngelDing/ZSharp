
using System;
using ZSharp.Framework.Infrastructure;

namespace Framework.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            InitUnityConfig();

            DistributedLockTest();

            Console.ReadLine();
        }

        private static void InitUnityConfig()
        {
            ServiceLocator.SetLocatorProvider(UnityConfig.GetConfiguredContainer());
        }

        private static void DistributedLockTest()
        {
            var lockTest = new LockTest();
            lockTest.MultipleClientsGetSameLock();
        }
    }
}
