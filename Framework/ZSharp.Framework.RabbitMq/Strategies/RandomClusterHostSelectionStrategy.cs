using System.Collections;
using System.Collections.Generic;
using ZSharp.Framework.Extensions;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class RandomClusterHostSelectionStrategy<T> : IClusterHostSelectionStrategy<T>, IEnumerable<T>
        where T : class
    {
        private readonly IList<T> items = new List<T>();

        private int CurrentIndex { get; set; }

        private int LastIndex
        {
            get { return items.Count - 1; }
        }

        public RandomClusterHostSelectionStrategy()
        {
            Succeeded = false;
        }

        public void Add(T item)
        {
            GuardHelper.ArgumentNotNull(() => item);
            items.Add(item);
            items.Shuffle();
        }

        public T Current()
        {
            if (items.Count == 0)
            {
                throw new ZRabbitMqException("No items in collection");
            }
            return items[CurrentIndex];
        }

        public bool Next()
        {
            if (CurrentIndex >= LastIndex)
                return false;
            if (Succeeded)
                return false;
            ++CurrentIndex;
            return true;
        }

        public void Success()
        {
            Succeeded = true;
        }

        public bool Succeeded { get; private set; }

        public void Reset()
        {
            items.Shuffle();
            Succeeded = false;
            CurrentIndex = 0;
        }


        public virtual IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}