using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using ZSharp.Framework.EfExtensions.Reflection;

namespace ZSharp.Framework.EfExtensions.Audit
{
    public class AuditConfiguration
    {
        private const BindingFlags _defaultBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        private readonly ConcurrentDictionary<Type, bool> _auditableCache = new ConcurrentDictionary<Type, bool>();
        private readonly ConcurrentDictionary<string, bool> _notAuditedCache = new ConcurrentDictionary<string, bool>();
        private readonly ConcurrentDictionary<Type, IMemberAccessor> _displayCache = new ConcurrentDictionary<Type, IMemberAccessor>();

        internal bool IsAuditable(Type entityType)
        {
            return _auditableCache.GetOrAdd(entityType, 
                key => HasAttribute(key, typeof(AuditAttribute)));
        }

        internal bool IsNotAudited(Type entityType, string name)
        {
            string fullName = entityType.FullName + "." + name;

            return _notAuditedCache.GetOrAdd(fullName, 
                key => HasAttribute(entityType, name, typeof(NotAuditedAttribute)));
        }

        internal IMemberAccessor GetDisplayMember(Type entityType)
        {
            return _displayCache.GetOrAdd(entityType, key =>
            {
                TypeAccessor typeAccessor = TypeAccessor.GetAccessor(entityType);
                IMemberAccessor displayMember = null;

                var displayAttribute = GetAttribute<DisplayColumnAttribute>(entityType);

                // first try DisplayColumnAttribute property
                if (displayAttribute != null)
                    displayMember = typeAccessor.FindProperty(displayAttribute.DisplayColumn);

                if (displayMember != null)
                    return displayMember;

                var properties = typeAccessor.GetProperties().ToList();

                // try first string property        
                displayMember = properties.FirstOrDefault(m => m.MemberType == typeof(string));
                if (displayMember != null)
                    return displayMember;

                // try second property
                return properties.Count > 1 ? properties[1] : null;
            });
        }

        private static bool HasAttribute(Type entityType, string fullName, Type attributeType)
        {
            var info = FindMember(entityType, fullName);
            return HasAttribute(info, attributeType);
        }

        private static bool HasAttribute(MemberInfo memberInfo, Type attributeType)
        {
            if (memberInfo.IsDefined(attributeType, true))
                return true;

            // try the metadata object
            MemberInfo declaringType = memberInfo;
            if (memberInfo.MemberType != MemberTypes.TypeInfo)
                declaringType = memberInfo.DeclaringType;

            var metadataTypeAttribute = declaringType
              .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
              .FirstOrDefault() as MetadataTypeAttribute;

            if (metadataTypeAttribute == null)
                return false;

            Type metadataType = metadataTypeAttribute.MetadataClassType;
            if (memberInfo.MemberType == MemberTypes.TypeInfo)
                return metadataType.IsDefined(attributeType, true);

            MemberInfo metaInfo = metadataType
              .GetMember(memberInfo.Name, _defaultBinding)
              .FirstOrDefault();

            return metaInfo != null && metaInfo.IsDefined(attributeType, true);
        }

        private static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo)
          where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);

            var attribute = memberInfo
              .GetCustomAttributes(attributeType, true)
              .FirstOrDefault() as TAttribute;

            if (attribute != null)
                return attribute;

            // try the metadata object
            var declaringType = memberInfo;
            if (memberInfo.MemberType != MemberTypes.TypeInfo)
                declaringType = memberInfo.DeclaringType;

            var metadataTypeAttribute = declaringType
              .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
              .FirstOrDefault() as MetadataTypeAttribute;

            if (metadataTypeAttribute == null)
                return null;

            var metadataType = metadataTypeAttribute.MetadataClassType;
            if (memberInfo.MemberType == MemberTypes.TypeInfo)
                return metadataType
                  .GetCustomAttributes(attributeType, true)
                  .FirstOrDefault() as TAttribute;

            var metaInfo = metadataType
              .GetMember(memberInfo.Name, _defaultBinding)
              .FirstOrDefault();

            if (metaInfo == null)
                return null;

            return metaInfo
              .GetCustomAttributes(attributeType, true)
              .FirstOrDefault() as TAttribute;
        }

        private static MemberInfo FindMember(Type entityType, string fullName)
        {
            var name = fullName.Split('.').LastOrDefault();
            var member = LateBinder.Find(entityType, name);
            var info = member.MemberInfo;
            return info;
        }
    }
}
