using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;

namespace ZSharp.Framework.EfExtensions.Audit
{
    public class AuditEntity
    {
        private WeakReference _current;
        private Type _entityType;

        public AuditEntity(object current)
        {
            if (current == null)
            {
                return;
            }

            _current = new WeakReference(current);
            _entityType = ObjectContext.GetObjectType(current.GetType());

            Type = _entityType.FullName;
            Properties = new List<AuditProperty>();
        }

        public AduitAction Action { get; set; }

        public string Type { get; set; }

        public object Current
        {
            get { return _current.IsAlive ? _current.Target : null; }
        }

        public List<AuditProperty> Properties { get; set; }
    }
}