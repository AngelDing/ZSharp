using System;

namespace ZSharp.Framework.Caching
{
    public class CacheTag : IEquatable<CacheTag>
    {
        private readonly string tagStr;
        
        public CacheTag(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            tagStr = tag; 
        }

        public bool Equals(CacheTag other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(other.tagStr, tagStr, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(CacheTag))
            {
                return false;
            }

            return Equals((CacheTag)obj);
        }

        /// </returns>
        public static bool operator ==(CacheTag left, CacheTag right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CacheTag left, CacheTag right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return (tagStr != null ? tagStr.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return tagStr;
        }

        public static implicit operator string(CacheTag cacheTag)
        {
            return cacheTag.ToString();
        }
    }
}
