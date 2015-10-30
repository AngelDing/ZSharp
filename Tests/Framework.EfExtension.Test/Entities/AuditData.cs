using ZSharp.Framework.EfExtensions.Audit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.EfExtension.Test
{
    [Audit]
    public partial class AuditData
    {
        public AuditData()
        {
            CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int? UserId { get; set; }
        public int? TaskId { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Byte[] RowVersion { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}