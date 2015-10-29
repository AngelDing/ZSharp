using System;

namespace ZSharp.Framework.EfExtensions.Audit
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false)]
    public class AuditAttribute : Attribute
    { 
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotAuditedAttribute : Attribute
    {
    }
}
