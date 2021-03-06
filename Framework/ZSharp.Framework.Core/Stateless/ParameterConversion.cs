﻿using System;

namespace ZSharp.Framework.Stateless
{
    static class ParameterConversion
    {
        public static object Unpack(object[] args, Type argType, int index)
        {
            Enforce.ArgumentNotNull(args, "args");

            if (args.Length <= index)
                throw new ArgumentException(
                    string.Format(StateMachineResources.ArgOfTypeRequiredInPosition, argType, index));

            var arg = args[index];

            if (arg != null && !argType.IsAssignableFrom(arg.GetType()))
                throw new ArgumentException(
                    string.Format(StateMachineResources.WrongArgType, index, arg.GetType(), argType));

            return arg;
        }

        public static TArg Unpack<TArg>(object[] args, int index)
        {
            return (TArg)Unpack(args, typeof(TArg), index);
        }

        public static void Validate(object[] args, Type[] expected)
        {
            if (args.Length > expected.Length)
                throw new ArgumentException(
                    string.Format(StateMachineResources.TooManyParameters, expected.Length, args.Length));

            for (int i = 0; i < expected.Length; ++i)
                Unpack(args, expected[i], i);
        }
    }
}
