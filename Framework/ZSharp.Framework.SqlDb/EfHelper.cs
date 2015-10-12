using System.Linq;
using ZSharp.Framework.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ZSharp.Framework.SqlDb
{
    public static class EfHelper
    {
        /// <summary>
        /// 通用的转换实体状态方法
        /// </summary>
        /// <typeparam name="TEntity">要操作的实体</typeparam>
        /// <param name="root">根实体</param>
        public static void ApplyChanges<TEntity>(this DbContext context, TEntity root)
            where TEntity : BaseEntity
        {
            context.Set(root.GetType()).Attach(root);

            var entries = context.ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                entry.State = ConvertState(entity.ObjectState);

                if (entity.NeedUpdateList.Count() == 0)
                {
                    continue;
                }
                var type = entity.GetType();
                context.Set(type).Attach(entity);
                var puEntry = ((IObjectContextAdapter)context).ObjectContext
                    .ObjectStateManager.GetObjectStateEntry(entity);

                foreach (var prop in entity.NeedUpdateList.Keys)
                {
                    var pList = prop.Split('.');
                    //指定屬性更新，如果屬性是值對象，則會更新整個值對象對應的字段
                    //TODO: 是否有方法可以更新指定值對象的指定字段值？
                    puEntry.SetModifiedProperty(pList[0]);
                }
            }
        }
        
        private static EntityState ConvertState(ObjectStateType state)
        {
            switch (state)
            {
                case ObjectStateType.Added:
                    return EntityState.Added;
                case ObjectStateType.Modified:
                    return EntityState.Modified;
                case ObjectStateType.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
