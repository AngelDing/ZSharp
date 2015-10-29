﻿
namespace ZSharp.Framework.Caching
{
    public enum CacheExpirationType
    {
        /// <summary>
        /// The cache item will not expire.
        /// </summary>
        None,
        /// <summary>
        /// The cache item will expire using the Duration property to calculate
        /// the absolute expiration from DateTimeOffset.Now.
        /// </summary>
        Duration,
        /// <summary>
        /// The cache item will expire using the Duration property as the
        /// sliding expiration.
        /// </summary>
        Sliding,
        /// <summary>
        /// The cache item will expire on the AbsoluteExpiration DateTime.
        /// </summary>
        Absolute
    } 
}
