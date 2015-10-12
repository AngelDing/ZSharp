using System;

namespace ZSharp.Framework.Extensions
{
    public static class DateTimeExtensions
    {
        private static long EpochMilliseconds = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000L;

        public static long ToEpochMilliseconds(this DateTime? time)
        {
            if (time.HasValue)
            {
                return (time.Value.Ticks / 10000L) - EpochMilliseconds;
            }

            return 0L;
        }
    }
}
