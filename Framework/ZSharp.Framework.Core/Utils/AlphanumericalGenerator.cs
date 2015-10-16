using System;

namespace ZSharp.Framework.Utils
{
    /// <summary>
    /// Generates random alphanumerical strings.
    /// </summary>
    public static class AlphanumericalGenerator
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Millisecond);
        private static readonly char[] allowableChars = "ABCDEFGHJKMNPQRSTUVWXYZ123456789".ToCharArray();

        public static string Generate(int length)
        {
            var result = new char[length];
            lock (rnd)
            {
                for (int i = 0; i < length; i++)
                {
                    result[i] = allowableChars[rnd.Next(0, allowableChars.Length)];
                }
            }

            return new string(result);
        }
    }
}
