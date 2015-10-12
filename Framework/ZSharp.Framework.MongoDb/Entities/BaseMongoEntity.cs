using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.MongoDb
{
    [Serializable]
    public abstract class BaseMongoEntity : BaseEntity
    {
        public override string GetUpdateKey(LambdaExpression expression)
        {
            var keys = new List<string>();
            var body = expression.Body;
            while (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                var baseType = memberExpression.Type.BaseType;
                if (baseType == typeof(IntKeyMongoEntity)
                    || baseType == typeof(StringKeyMongoEntity)
                    || baseType == typeof(LongKeyMongoEntity))
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
