using System.Data.Entity;
using ZSharp.Framework.EfExtensions.Audit;

namespace ZSharp.Framework.EfExtensions
{
    public static class AuditExtensions
    {
        public static AuditLogger BeginAudit(this DbContext dbContext)
        {
            return new AuditLogger(dbContext);
        }
    }
}