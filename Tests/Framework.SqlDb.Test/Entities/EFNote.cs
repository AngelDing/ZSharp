using System;
using System.Collections.Generic;
using ZSharp.Framework.SqlDb;

namespace Framework.SqlDb.Test
{
    [Serializable]
    public class EFNote : EfEntity<int>
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
    public class ChildNote : EfEntity<int>
    {
        public long ChildNoteId { get; set; }

        public string Title { get; set; }

        public long NoteId { get; set; }

        public virtual EFNote EFNote { get; set; }
    }
}
