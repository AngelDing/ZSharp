using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.SqlDb
{
    /// <summary>
    /// 定义统一IEntity<TKey>的接口時使用，即所有繼承此類的Entity均使用Id作為主鍵字段
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    [Serializable]
    public abstract class EfEntity<Tkey> : BaseEntity<Tkey>
    {
        /// <summary>
        /// 獲取傳入表達式的所要更新的屬性
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override string GetUpdateKey(LambdaExpression expression)
        {
            return EfEntityHelper.GetUpdateKey(expression, this.GetType().BaseType);
        }        
    }

    internal static class EfEntityHelper
    {
        public static string GetUpdateKey(LambdaExpression expression, Type type)
        {
            var keys = new List<string>();
            var body = expression.Body;
            while (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                var baseType = memberExpression.Type.BaseType;
                if (baseType == type)
                {
                    break;
                }
                keys.Add(memberExpression.Member.Name);

                var insideExpress = memberExpression.Expression;
                if (insideExpress != null && insideExpress.NodeType == ExpressionType.MemberAccess)
                {
                    body = insideExpress;
                }
                else
                {
                    break;
                }
            }

            keys.Reverse();
            return string.Join(".", keys.ToArray());
        }        
    }
}
