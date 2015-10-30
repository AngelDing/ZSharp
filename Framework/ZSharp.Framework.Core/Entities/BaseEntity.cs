using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace ZSharp.Framework.Entities
{
    [Serializable]
    public abstract class BaseEntity : IEntity, IPartialUpdateEntity, IObjectWithState, IValidatableObject
    {      
        private Dictionary<string, object> updateList = new Dictionary<string, object>();

        public Dictionary<string, object> NeedUpdateList
        {
            get
            {
                return updateList;
            }
            set
            {
                //用於自動比較后的指定字段更新
                updateList = value;
            }
        }

        /// <summary>
        /// 實體所處的操作狀態
        /// </summary>
        public virtual ObjectStateType ObjectState { get; set; }

        /// <summary>
        /// 用於實體的指定字段的更新
        /// </summary>
        /// <typeparam name="T">實體類型</typeparam>
        /// <param name="express">表達式： p => p.XXX</param>
        /// <param name="val">要更新的值</param>
        public virtual void SetUpdate<T>(Expression<Func<T>> express, object val)
        {
            if (express == null || express.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("'" + express + "': 不是有效的表達式！");
            }
            MemberExpression body = (MemberExpression)express.Body;
            var propStr = GetUpdateKey(express);

            updateList.Add(propStr, val);
            //用於EF的局部更新，此方法不影響MongoDB的局部更新
            SetPropertyValue(this, propStr, val);
        }

        /// <summary>
        /// 根據屬性字符串獲取PropertyDescriptor實例，如果傳入的是值對象的屬性，需要先找到值對象，再找到屬性對象
        /// </summary>
        /// <param name="type">類型</param>
        /// <param name="propStr">屬性字符串，默認是實體的屬性，如果傳入的是值對象的屬性，需按以下格式傳入：值對象類名.屬性名</param>
        /// <param name="val">要賦予的值</param>
        private void SetPropertyValue(object data, string propStr, object val)
        {
            var pdStrList = propStr.Split('.');
            var type = data.GetType();
            var pdList = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().ToList();
            var pd = pdList.Where(i => i.DisplayName == pdStrList[0]).FirstOrDefault();
            if (pdStrList.Length == 1)
            {
                pd.SetValue(data, val);
            }
            else
            {
                var childData = pd.GetValue(data);
                SetPropertyValue(childData, pdStrList[1], val);
            }
        }

        /// <summary>
        /// 實現IValidatableObject接口，強制每個Entity實現相關邏輯校驗
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        private string GetUpdateKey(LambdaExpression expression)
        {
            var keys = new List<string>();
            var body = expression.Body;
            var baseType = this.GetType().BaseType;
            while (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                var type = memberExpression.Type.BaseType;
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

    [Serializable]
    public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
    {
        public TKey Id { get; set; }

        public override bool Equals(object entity)
        {
            return entity != null
               && entity is BaseEntity<TKey>
               && this == (BaseEntity<TKey>)entity;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            return (!(entity1 == entity2));
        }
    }
}
