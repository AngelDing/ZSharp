﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using ZSharp.Framework.Entities;
using System.Threading.Tasks;
using System.Threading;
using ZSharp.Framework.Repositories;
using ZSharp.Framework.Extensions;
using System.Data.Entity.Infrastructure;

namespace ZSharp.Framework.SqlDb
{
    public class EfRepositoryContext : RepositoryContext, IEfRepositoryContext
    {
        private readonly DbContext dbContext;

        public EfRepositoryContext(DbContext dbContext)
        {
            this.dbContext = dbContext;            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        public DbContext DbContext
        {
            get { return this.dbContext; }
        }

        #region IRepositoryContext Members

        //public override void RegisterNew<T>(T obj)
        //{
        //    this.dbContext.Entry(obj).State = EntityState.Added;
        //    Committed = false;
        //}

        public override void RegisterModified<T>(T obj)
        {            
            this.dbContext.Detach(obj);
            base.RegisterModified(obj);
            //obj.ObjectState = ObjectStateType.Modified;
        }

        public override void RegisterDeleted<T>(T obj)
        {
            this.dbContext.Detach(obj);
            base.RegisterDeleted(obj);
            //this.dbContext.Entry(obj).State = EntityState.Deleted;
        }       

        #endregion

        #region IUnitOfWork

        public override void Commit()
        {
            if (IsNeedSave())
            {
                //此處手動進行相關邏輯的校驗，故需要全局關閉SaveChanges時自動的校驗：
                //Configuration.ValidateOnSaveEnabled = false;
                var errors = dbContext.GetValidationErrors();

                if (errors.Any())
                {
                    var errorMsgs = GetErrors(errors);
                    throw new EfRepositoryException(errorMsgs);
                }

                try
                {
                    foreach (var newObj in NewCollection)
                    {
                        var newEntity = newObj.Key;
                        dbContext.Entry(newEntity).State = EntityState.Added;
                    }
                    foreach (var updateObj in ModifiedCollection)
                    {
                        var updateEntity = updateObj.Key as Entity;
                        if (updateEntity != null && updateEntity.ObjectState == ObjectStateType.Modified)
                        {
                            dbContext.Entry(updateEntity).State = EntityState.Modified;
                        }
                    }
                    foreach (var deleteObj in DeletedCollection)
                    {
                        var deleteEntity = deleteObj.Key;
                        dbContext.Entry(deleteEntity).State = EntityState.Deleted;
                    }

                    dbContext.SaveChanges();
                    ClearRegistrations();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new DbUpdateConcurrencyException(e.Message, e);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool IsNeedSave()
        {
            var isNeedSave = false;
            if (!NewCollection.IsNullOrEmpty()
                || !ModifiedCollection.IsNullOrEmpty()
                || !DeletedCollection.IsNullOrEmpty())
            {
                isNeedSave = true;
            }
            return isNeedSave;
        }

        private string GetErrors(IEnumerable<DbEntityValidationResult> results)
        {
            var errorMsgs = new StringBuilder();
            int counter = 0;

            foreach (DbEntityValidationResult result in results)
            {
                counter++;
                errorMsgs.AppendFormat("Failed Object #{0}: Type is {1}", counter, result.Entry.Entity.GetType().Name);
                errorMsgs.AppendLine();
                errorMsgs.AppendFormat(" Number of Problems: {0}", result.ValidationErrors.Count);
                errorMsgs.AppendLine();
                foreach (DbValidationError error in result.ValidationErrors)
                {
                    errorMsgs.AppendFormat(" - {0}", error.ErrorMessage);
                    errorMsgs.AppendLine();
                }
            }

            return errorMsgs.ToString();
        }

        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }

        public override async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (!Committed)
            {
                await this.dbContext.SaveChangesAsync(cancellationToken);
                Committed = true;
            }
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            Committed = false;
        }

        #endregion
    }
}
