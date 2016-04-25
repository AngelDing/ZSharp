using System;
using System.Collections;
using System.Diagnostics;
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
        public static void ArgumentNotEmpty(Func<IEnumerable> arg)
        {
            if (!arg().HasItems())
            {
                string argName = GetParamName(arg);
                throw ErrorHelper.Argument(argName, "List cannot be null and must have at least one item.");
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(Func<T> arg, string msg = null)
        {
            if (arg() == null)
            {
                if (msg.IsNullOrEmpty())
                {
                    throw new ArgumentNullException(GetParamName(arg));
                }
                else
                {
                    throw new ArgumentNullException(GetParamName(arg), msg);
                }
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
            {
                var msg = message.FormatInvariant(type.FullName, typeof(TInterface).FullName);
                throw new InvalidOperationException(msg);
            }
        }

        [DebuggerStepThrough]
        private static string GetParamName<T>(Func<T> expression)
        {
            return expression.Method.Name;
        }

        [DebuggerStepThrough]
        public static void CheckStringLength(Func<string> arg, int length)
        {
            ArgumentNotNull(arg);            
            if (arg().Length > length)
            {
                var msgTemplate = "Argument '{0}' must be less than or equal to {1} characters.";
                throw new ArgumentException(string.Format(msgTemplate, GetParamName(arg), length));
            }
        }
    }
}
