using ZSharp.Framework.EfExtensions.Audit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.EfExtension.Test
{
    [Audit]
    public partial class Task
    {
        public Task()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Audits = new List<AuditData>();
        }

        public int Id { get; set; }
        public int StatusId { get; set; }
        public int? PriorityId { get; set; }
        public int CreatedId { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }
        public System.DateTime? StartDate { get; set; }
        public System.DateTime? DueDate { get; set; }
        public System.DateTime? CompleteDate { get; set; }
        public int? AssignedId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Byte[] RowVersion { get; set; }
        public string LastModifiedBy { get; set; }

        public virtual ICollection<AuditData> Audits { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Status Status { get; set; }
        public virtual User AssignedUser { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual TaskExtended TaskExtended { get; set; }
    }
}