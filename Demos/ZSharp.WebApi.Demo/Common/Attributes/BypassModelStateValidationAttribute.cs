using System;

namespace ZSharp.WebApi.Demo.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public sealed class BypassModelStateValidationAttribute : Attribute
    {

    }
}