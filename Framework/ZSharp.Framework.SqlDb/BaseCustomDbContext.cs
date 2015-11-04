using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.SqlDb
{
    public class BaseCustomDbContext : DbContext
    {
        //设置标识，使用自定义的SaveChanges方法
        public bool LogChangesDuringSave { get; set; }

        public BaseCustomDbContext(string connectionString)
            : base(connectionString)
        {
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized +=
            (sender, args) =>
            {
                var entity = args.Entity as Entity;
                if (entity != null)
                {
                    entity.ObjectState = ObjectStateType.Unchanged;
                }
            };

            //调用SaveChanges方法的时候不验证实体
            Configuration.ValidateOnSaveEnabled = false;
            //禁用延遲加載
            this.Configuration.LazyLoadingEnabled = false;
            //禁用代理對象
            this.Configuration.ProxyCreationEnabled = false;
        }

        public BaseCustomDbContext(string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {          
        }

        /// <summary>
        /// 记录帮助类
        /// </summary>
        private void PrintPropertyValues(DbPropertyValues values, IEnumerable<string> propertiesToPrint, int indent = 1)
        {
            foreach (var propertyName in propertiesToPrint)
            {
                var value = values[propertyName];
                if (value is DbPropertyValues)
                {
                    Console.WriteLine("{0}- Complex Property: {1}", string.Empty.PadLeft(indent), propertyName);
                    var complexPropertyValues = (DbPropertyValues)value;
                    PrintPropertyValues(complexPropertyValues, complexPropertyValues.PropertyNames, indent + 1);
                }
                else
                {
                    Console.WriteLine("{0}- {1}: {2}", string.Empty.PadLeft(indent), propertyName, values[propertyName]);
                }
            }
        }
        private IEnumerable<string> GetKeyPropertyNames(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            return objectContext.ObjectStateManager.GetObjectStateEntry(entity)
                .EntityKey.EntityKeyValues.Select(k => k.Key);
        }

        /// <summary>
        /// 重写SaveChanges方法
        /// </summary>
        public override int SaveChanges()
        {
            if (LogChangesDuringSave)
            {
                var entries = from e in this.ChangeTracker.Entries()
                              where e.State != EntityState.Unchanged
                              select e;   //过滤所有修改了的实体，包括：增加 / 修改 / 删除
                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            Console.WriteLine("Adding a {0}", entry.Entity.GetType());
                            PrintPropertyValues(entry.CurrentValues, entry.CurrentValues.PropertyNames);
                            break;
                        case EntityState.Deleted:
                            Console.WriteLine("Deleting a {0}", entry.Entity.GetType());
                            PrintPropertyValues(entry.OriginalValues, GetKeyPropertyNames(entry.Entity));
                            break;
                        case EntityState.Modified:
                            Console.WriteLine("Modifying a {0}", entry.Entity.GetType());
                            var modifiedPropertyNames = from n in entry.CurrentValues.PropertyNames
                                                        where entry.Property(n).IsModified
                                                        select n;
                            PrintPropertyValues(entry.CurrentValues, GetKeyPropertyNames(entry.Entity).Concat(modifiedPropertyNames));
                            break;
                    }
                }
            }
            return base.SaveChanges();  //返回普通的上下文SaveChanges方法
        }
    }
}
