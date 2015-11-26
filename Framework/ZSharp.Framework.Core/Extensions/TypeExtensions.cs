using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns single attribute from the type
        /// </summary>
        /// <typeparam name="T">Attribute to use</typeparam>
        /// <param name="target">Attribute provider</param>
        ///<param name="inherit"><see cref="MemberInfo.GetCustomAttributes(Type,bool)"/></param>
        /// <returns><em>Null</em> if the attribute is not found</returns>
        /// <exception cref="InvalidOperationException">If there are 2 or more attributes</exception>
        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target.GetCustomAttributes(typeof(TAttribute), inherits);
                if (attributes.Length > 1)
                {
                    throw ErrorHelper.MoreThanOneElement();
                }
                return (TAttribute)attributes[0];
            }

            return null;
        }

        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type type)
        {
            GuardHelper.ArgumentNotNull(() => type);

            if (type.IsValueType)
                return true;

            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Any(ctor => ctor.GetParameters().Length == 0);
        }
    }
}
