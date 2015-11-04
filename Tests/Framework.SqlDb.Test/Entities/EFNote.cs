using System;
using System.Collections.Generic;
using ZSharp.Framework.Entities;

namespace Framework.SqlDb.Test
{
    [Serializable]
    public class EFNote : Entity<int>
    {
        public EFNote()
        {
            ChildNote = new HashSet<ChildNote>();
        }

        public string NoteText { get; set; }

        public int CustomerId { get; set; }

        public virtual ICollection<ChildNote> ChildNote { get; set; }

        public virtual EFCustomer EFCustomer { get; set; }
    }

    [Serializable]
    public class ChildNote : Entity<int>
    {
        public string Title { get; set; }

        public int NoteId { get; set; }

        public virtual EFNote EFNote { get; set; }
    }
}
