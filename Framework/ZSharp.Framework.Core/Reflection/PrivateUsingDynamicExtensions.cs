using System;
using System.Reflection;

namespace ZSharp.Framework.Reflection
{
    public static class PrivateUsingDynamicExtensions
    {
        public static dynamic AsDynamic(this object o)
        {
            // Don't wrap primitive types, which don't have many interesting internal APIs
            if (o == null || o.GetType().IsPrimitive || o is string || o is PrivateDynamicObjectBase)
                return o;

            return new PrivateDynamicObjectInstance(o);
        }

        public static dynamic AsDynamicType(this Type type)
        {
            return new PrivateDynamicObjectStatic(type);
        }

        public static dynamic GetDynamicType(this Assembly assembly, string typeName)
        {
            return assembly.GetType(typeName).AsDynamicType();
        }

        public static dynamic CreateDynamicInstance(this Assembly assembly, string typeName, params object[] args)
        {
            return assembly.GetDynamicType(typeName).New(args);
        }
    }
}
