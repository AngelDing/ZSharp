using System;

namespace ZSharp.Framework.MongoDb
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Empty collectionname not allowed", "value");
            }

            this.Name = value;
        }

        public virtual string Name { get; private set; }
    }
}
