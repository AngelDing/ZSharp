using System;
using System.Runtime.Caching;

namespace ZSharp.Framework.Caching
{
    public class CachePolicy
    {
        private static readonly Lazy<CachePolicy> current = new Lazy<CachePolicy>(() => new CachePolicy());
        public static CachePolicy Default
        {
            get { return current.Value; }
        }
        
        public CachePolicy()
        {
            ExpirationType = CacheExpirationType.None;
            absoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            slidingExpiration = ObjectCache.NoSlidingExpiration;
            duration = TimeSpan.Zero; 
        }

        public CacheExpirationType ExpirationType { get; set; }

        private DateTimeOffset absoluteExpiration;
        public DateTimeOffset AbsoluteExpiration
        {
            get { return absoluteExpiration; }
            set
            {
                absoluteExpiration = value;
                ExpirationType = CacheExpirationType.Absolute;
            }
        }

        private TimeSpan slidingExpiration;
        public TimeSpan SlidingExpiration
        {
            get { return slidingExpiration; }
            set
            {
                slidingExpiration = value;
                ExpirationType = CacheExpirationType.Sliding;
            }
        }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                ExpirationType = CacheExpirationType.Duration;
            }
        }

        public static CachePolicy WithDurationExpiration(TimeSpan expirationSpan)
        {
            var policy = new CachePolicy
            {
                Duration = expirationSpan
            };

            return policy;
        }

        public static CachePolicy WithAbsoluteExpiration(DateTimeOffset absoluteExpiration)
        {
            var policy = new CachePolicy
            {
                AbsoluteExpiration = absoluteExpiration
            };

            return policy;
        }

        public static CachePolicy WithSlidingExpiration(TimeSpan slidingExpiration)
        {
            var policy = new CachePolicy
            {
                SlidingExpiration = slidingExpiration
            };

            return policy;
        }
    }
}
