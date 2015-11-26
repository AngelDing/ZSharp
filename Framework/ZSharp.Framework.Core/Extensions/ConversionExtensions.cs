using System;
using System.Diagnostics;

namespace ZSharp.Framework.Extensions
{
    public static class ConversionExtensions
    {
        [DebuggerStepThrough]
        public static Version ToVersion(this string value, Version defaultVersion = null)
        {
            try
            {
                return new Version(value);
            }
            catch
            {
                return defaultVersion ?? new Version("1.0");
            }
        }
    }
}
