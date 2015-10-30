using System;

namespace Framework.Caching.Test
{
    [Serializable]
    public class TestClass<T>
    {
        public string Key { get; set; }

        public T Value { get; set; }
    }
}