using System;
using System.Linq;
using System.Collections.Generic;

namespace ZSharp.Framework.Caching
{
    public class CacheKey
    {
        private readonly string key;
        private readonly HashSet<CacheTag> tags;

        public CacheKey(string key)
            : this(key, Enumerable.Empty<string>())
        { }

        public CacheKey(string key, IEnumerable<string> tags)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }

            this.key = key;
             
            var cacheTags = tags.Select(k => new CacheTag(k));
            this.tags = new HashSet<CacheTag>(cacheTags);
        }

        public string Key
        {
            get { return key; }
        }

        public HashSet<CacheTag> Tags
        {
            get { return tags; }
        }
    }
}
