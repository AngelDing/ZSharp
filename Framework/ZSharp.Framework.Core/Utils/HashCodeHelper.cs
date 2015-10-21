﻿using System.Collections.Generic;
using System.Linq;

namespace ZSharp.Framework.Utils
{
    public static class HashCodeHelper
    {
        public static int CombineHashCodes(IEnumerable<object> objects)
        {
            unchecked
            {
                return objects.Aggregate(17, (current, obj) => current * 23 + (obj != null ? obj.GetHashCode() : 0));
            }
        }
    }
}
