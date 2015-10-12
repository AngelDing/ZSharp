using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ZSharp.Framework.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 尝试将值类型变量转换为指定枚举类型，并获取枚举变量值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="obj">值类型变量</param>
        /// <returns>如果包含特性值则返回，否则返回枚举项的名称；对有Flags位标识的枚举，且输入参数值是多个枚举项基本值按位或结果的，返回多个枚举项的特性或名称，以逗号分隔</returns>
        public static string GetDescription<TEnum>(this object obj)
            where TEnum : struct
        {
            return obj.ToEnum<TEnum>().GetDescription();
        }

        /// <summary>
        /// 获取枚举变量值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <returns>如果包含特性值则返回，否则返回枚举项的名称；对有Flags位标识的枚举，且输入参数值是多个枚举项基本值按位或结果的，返回多个枚举项的特性或名称，以逗号分隔</returns>
        public static string GetDescription(this object obj)
        {
            return GetDescription(obj, false);
        }

        /// <summary>
        /// 获取枚举变量值的 Description 、 DisplayName 或 Display(Name= 特性值
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含特性值则返回，否则返回枚举项的名称；对有Flags位标识的枚举，且输入参数值是多个枚举项基本值按位或结果的，返回多个枚举项的特性或名称，以逗号分隔</returns>
        public static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            Type type = obj.GetType();
            try
            {
                if ((!type.IsEnum && !type.IsValueType) || (type.IsEnum && isTop))
                {
                    var da = (DescriptionAttribute)Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute));
                    if (da != null && !string.IsNullOrEmpty(da.Description))
                        return da.Description;
                    var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(type, typeof(DisplayNameAttribute));
                    if (dna != null && !string.IsNullOrEmpty(dna.DisplayName))
                        return dna.DisplayName;
                    var na = (DisplayAttribute)Attribute.GetCustomAttribute(type, typeof(DisplayAttribute));
                    if (na != null && !string.IsNullOrEmpty(na.Name))
                        return na.Name;
                }
                else
                {
                    if (Attribute.GetCustomAttribute(type, typeof(FlagsAttribute)) != null && !Enum.IsDefined(type, obj))
                    {
                        List<string> lst = new List<string>();
                        var values = Enum.GetValues(type);
                        if (values.Length > 0)
                        {
                            var objValue = Convert.ToUInt64(obj);
                            foreach (var value in values)
                            {
                                if ((objValue & Convert.ToUInt64(value)) > 0)
                                {
                                    lst.Add(GetDescription(type, value, isTop));
                                }
                            }
                        }
                        return string.Join(",", lst);
                    }
                    else
                    {
                        return GetDescription(type, obj, isTop);
                    }
                }
            }
            catch { }
            return obj.ToString();
        }

        private static string GetDescription(Type type, object obj, bool isTop)
        {
            try
            {
                FieldInfo fi = type.GetField(Enum.GetName(type, obj));
                var da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if (da != null && !string.IsNullOrEmpty(da.Description))
                    return da.Description;
                var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayNameAttribute));
                if (dna != null && !string.IsNullOrEmpty(dna.DisplayName))
                    return dna.DisplayName;
                var na = (DisplayAttribute)Attribute.GetCustomAttribute(fi, typeof(DisplayAttribute));
                if (na != null && !string.IsNullOrEmpty(na.Name))
                    return na.Name;
            }
            catch { }
            return obj.ToString();
        }

        /// <summary>
        /// 将对象转换为指定类型的枚举变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object value)
            where T : struct
        {
            T t;
            bool flag = Enum.TryParse<T>(value.ToString(), true, out t);
            if (flag && Enum.IsDefined(typeof(T), t))
            {
                return t;
            }
            else
            {
                return default(T);
            }
        }

        public static T ToEnum<T>(this string value, T defaultValue) 
            where T : struct
        {
            T t;
            bool flag = Enum.TryParse<T>(value, true, out t);
            if (flag && Enum.IsDefined(typeof(T), t))
            {
                return t;
            }
            else
            {
                return defaultValue;
            }
        }       
    }
}
