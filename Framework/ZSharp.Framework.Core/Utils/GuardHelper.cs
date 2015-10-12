using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Utils
{
    public static class GuardHelper
    {
        private const string ImplementsMessage = "Type '{0}' must implement type '{1}'.";

        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(Func<string> arg)
        {
            if (arg().IsEmpty())
            {
                throw ErrorHelper.ArgumentNullOrEmpty(arg);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(Func<T> arg)
        {
            if (arg() == null)
            {
                throw new ArgumentNullException(GetParamName(arg));
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(Func<IEnumerable> arg)
        {
            if (!arg().HasItems())
            {
                string argName = GetParamName(arg);
                throw ErrorHelper.Argument(argName, "List cannot be null and must have at least one item.");
            }
        }

        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type, string message)
        {
            if (type.BaseType != typeof(TBase))
            {
                throw new InvalidOperationException(message);
            }
        }

        [DebuggerStepThrough]
        public static void Implements<TInterface>(Type type, string message = ImplementsMessage)
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
                throw new InvalidOperationException(message.FormatInvariant(type.FullName, typeof(TInterface).FullName));
        }

        [DebuggerStepThrough]
        private static string GetParamName<T>(Func<T> expression)
        {
            return expression.Method.Name;
        }
    }
}
