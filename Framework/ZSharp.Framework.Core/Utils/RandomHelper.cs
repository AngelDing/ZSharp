using System;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Utils
{
    public static class RandomHelper
    {
        private static readonly Random _rnd = new Random();

        public static int GetRandom(int minValue, int maxValue)
        {
            lock (_rnd)
            {
                return _rnd.Next(minValue, maxValue);
            }
        }

        public static int GetRandom(int maxValue)
        {
            lock (_rnd)
            {
                return _rnd.Next(maxValue);
            }
        }

        public static int GetRandom()
        {
            lock (_rnd)
            {
                return _rnd.Next();
            }
        }

        public static T GetRandomOf<T>(params T[] objs)
        {
            if (objs.IsNullOrEmpty())
            {
                throw new ArgumentException("objs can not be null or empty!", "objs");
            }

            return objs[GetRandom(0, objs.Length - 1)];
        }
    }
}
