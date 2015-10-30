using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.EfExtension.Test
{
    public partial class Role
    {
        public Role()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Byte[] RowVersion { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}