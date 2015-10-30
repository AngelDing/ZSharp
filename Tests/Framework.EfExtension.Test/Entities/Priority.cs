using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.EfExtension.Test
{
    public partial class Priority
    {
        public Priority()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Tasks = new List<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Byte[] RowVersion { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}