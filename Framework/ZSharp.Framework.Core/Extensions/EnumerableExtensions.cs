using System.Collections;

namespace ZSharp.Framework.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasItems(this IEnumerable source)
        {
            return source != null && source.GetEnumerator().MoveNext();
        }

        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            return !HasItems(source);
        }
    }
}