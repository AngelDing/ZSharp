using System;

namespace Framework.Test.Core.Serialization
{
    [Serializable]
    public class GenericsObject<T>
    {
        public string Key { get; set; }

        public T Value { get; set; }
    }
}
