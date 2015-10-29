using System;

namespace ZSharp.Framework.Redis
{
    public static class TypeHelper
    {     
        /// <summary>
        /// 判斷是否常用基礎類型,與StackExchange.Redis中RedisValue的類型重載相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsCommonBasicType<T>()
        {
            var type = typeof(T);
            return type.IsEquivalentTo(typeof(bool))
               || type.IsEquivalentTo(typeof(int))
               || type.IsEquivalentTo(typeof(long))
               || type.IsEquivalentTo(typeof(double))
               || type.IsEquivalentTo(typeof(string))
               || type.IsEquivalentTo(typeof(byte[]));
        }

        public static bool IsCommonBasicType(Type type)
        {
            return type.IsEquivalentTo(typeof(bool))
              || type.IsEquivalentTo(typeof(int))
              || type.IsEquivalentTo(typeof(long))
              || type.IsEquivalentTo(typeof(double))
              || type.IsEquivalentTo(typeof(string))
              || type.IsEquivalentTo(typeof(byte[]));
        }
    }
}
