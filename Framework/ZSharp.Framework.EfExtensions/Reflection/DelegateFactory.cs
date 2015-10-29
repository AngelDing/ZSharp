﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace ZSharp.Framework.EfExtensions.Reflection
{
    internal delegate object LateBoundMethod(object target, object[] arguments);
    internal delegate object LateBoundGet(object target);
    internal delegate void LateBoundSet(object target, object value);
    internal delegate object LateBoundConstructor();

    internal static class DelegateFactory
    {
        private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
        {
            return !owner.IsInterface
              ? new DynamicMethod(name, returnType, parameterTypes, owner, true)
              : new DynamicMethod(name, returnType, parameterTypes, owner.Assembly.ManifestModule, true);
        }

        public static LateBoundMethod CreateMethod(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            DynamicMethod dynamicMethod = CreateDynamicMethod(
              "Dynamic" + methodInfo.Name,
              typeof(object),
              new[] { typeof(object), typeof(object[]) },
              methodInfo.DeclaringType);

            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();

            var paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }

            var locals = new LocalBuilder[paramTypes.Length];
            for (int i = 0; i < paramTypes.Length; i++)
                locals[i] = il.DeclareLocal(paramTypes[i], true);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.FastInt(i);
                il.Emit(OpCodes.Ldelem_Ref);
                il.UnboxIfNeeded(paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }

            if (!methodInfo.IsStatic)
                il.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }

            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);

            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                il.BoxIfNeeded(methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (!ps[i].ParameterType.IsByRef)
                    continue;

                il.Emit(OpCodes.Ldarg_1);
                il.FastInt(i);
                il.Emit(OpCodes.Ldloc, locals[i]);
                if (locals[i].LocalType.IsValueType)
                    il.Emit(OpCodes.Box, locals[i].LocalType);
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Ret);
            return (LateBoundMethod)dynamicMethod.CreateDelegate(typeof(LateBoundMethod)); ;
        }

        public static LateBoundConstructor CreateConstructor(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            DynamicMethod dynamicMethod = CreateDynamicMethod("Create" + type.FullName, typeof(object), Type.EmptyTypes, type);
            dynamicMethod.InitLocals = true;
            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (type.IsValueType)
            {
                generator.DeclareLocal(type);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, type);
            }
            else
            {
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (constructorInfo == null)
                    throw new InvalidOperationException(string.Format("Could not get constructor for {0}.", type));

                generator.Emit(OpCodes.Newobj, constructorInfo);
            }

            generator.Return();

            return (LateBoundConstructor)dynamicMethod.CreateDelegate(typeof(LateBoundConstructor));
        }

        public static LateBoundGet CreateGet(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (!propertyInfo.CanRead)
                return null;

            MethodInfo methodInfo = propertyInfo.GetGetMethod(true);
            if (methodInfo == null)
                return null;

            DynamicMethod dynamicMethod = CreateDynamicMethod(
              "Get" + propertyInfo.Name,
              typeof(object),
              new[] { typeof(object) },
              propertyInfo.DeclaringType);

            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (!methodInfo.IsStatic)
                generator.PushInstance(propertyInfo.DeclaringType);

            generator.CallMethod(methodInfo);
            generator.BoxIfNeeded(propertyInfo.PropertyType);
            generator.Return();

            return (LateBoundGet)dynamicMethod.CreateDelegate(typeof(LateBoundGet));
        }

        public static LateBoundGet CreateGet(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            DynamicMethod dynamicMethod = CreateDynamicMethod(
              "Get" + fieldInfo.Name,
              typeof(object),
              new[] { typeof(object) },
              fieldInfo.DeclaringType);

            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (fieldInfo.IsStatic)
                generator.Emit(OpCodes.Ldsfld, fieldInfo);
            else
                generator.PushInstance(fieldInfo.DeclaringType);

            generator.Emit(OpCodes.Ldfld, fieldInfo);
            generator.BoxIfNeeded(fieldInfo.FieldType);
            generator.Return();

            return (LateBoundGet)dynamicMethod.CreateDelegate(typeof(LateBoundGet));
        }

        public static LateBoundSet CreateSet(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (!propertyInfo.CanWrite)
                return null;

            MethodInfo methodInfo = propertyInfo.GetSetMethod(true);
            if (methodInfo == null)
                return null;

            DynamicMethod dynamicMethod = CreateDynamicMethod(
              "Set" + propertyInfo.Name,
              null,
              new[] { typeof(object), typeof(object) },
              propertyInfo.DeclaringType);

            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (!methodInfo.IsStatic)
                generator.PushInstance(propertyInfo.DeclaringType);

            generator.Emit(OpCodes.Ldarg_1);
            generator.UnboxIfNeeded(propertyInfo.PropertyType);
            generator.CallMethod(methodInfo);
            generator.Return();

            return (LateBoundSet)dynamicMethod.CreateDelegate(typeof(LateBoundSet));
        }

        public static LateBoundSet CreateSet(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            DynamicMethod dynamicMethod = CreateDynamicMethod(
              "Set" + fieldInfo.Name,
              null,
              new[] { typeof(object), typeof(object) },
              fieldInfo.DeclaringType);

            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (fieldInfo.IsStatic)
                generator.Emit(OpCodes.Ldsfld, fieldInfo);
            else
                generator.PushInstance(fieldInfo.DeclaringType);

            generator.Emit(OpCodes.Ldarg_1);
            generator.UnboxIfNeeded(fieldInfo.FieldType);
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Return();

            return (LateBoundSet)dynamicMethod.CreateDelegate(typeof(LateBoundSet));
        }
    }
}
