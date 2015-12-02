using System;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace ZSharp.Framework.MongoDb.Conventions
{
    public class UseLocalDateTimeConvention : IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            IBsonSerializer serializer = null;
            switch (memberMap.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberMap.MemberInfo;
                    serializer = GetBsonSerializer(propertyInfo.PropertyType);                  
                    break;
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberMap.MemberInfo;
                    serializer = GetBsonSerializer(fieldInfo.FieldType);    
                    break;
                default:
                    break;
            }
            if (serializer != null)
            {
                memberMap.SetSerializer(serializer);
            }
        }

        private IBsonSerializer GetBsonSerializer(Type type)
        {
            IBsonSerializer serializer = null;
            if (type == typeof(DateTime))
            {
                serializer = new DateTimeSerializer(DateTimeKind.Local);
            }
            else if (type == typeof(DateTime?))
            {
                var dateTimeSerializer = new DateTimeSerializer(DateTimeKind.Local);
                serializer = new NullableSerializer<DateTime>(dateTimeSerializer);
            }
            return serializer;
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }
    }
}
