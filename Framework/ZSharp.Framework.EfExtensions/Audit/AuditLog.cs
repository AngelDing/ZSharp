using System;
using System.Linq;
using System.Collections.Generic;
using ZSharp.Framework.EfExtensions.Reflection;

namespace ZSharp.Framework.EfExtensions.Audit
{
    /// <summary>
    /// A class representing a log of the changes.
    /// </summary>
    public class AuditLog
    {       
        public AuditLog()
        {
            Entities = new List<AuditEntity>();
        }

        public string Username { get; set; }

        public DateTime Date { get; set; }

        public List<AuditEntity> Entities { get; set; }

        /// <summary>
        /// Refresh key and property values. Call after Save to capture database updated keys and values.
        /// </summary>
        public AuditLog Refresh()
        {
            // update current values because the entites can change after submit
            foreach (var auditEntity in Entities)
            {
                // don't need to update deletes
                if (auditEntity.Action == AduitAction.Deleted)
                {
                    continue;
                }

                // if current is stored, it will be updated on submit
                object current = auditEntity.Current;
                if (current == null)
                {
                    continue;
                }

                // update the property current values
                foreach (var property in auditEntity.Properties.Where(p => !p.IsRelationship))
                {
                    property.Current = LateBinder.GetProperty(current, property.Name);
                }
            }

            return this;
        }
    }
}