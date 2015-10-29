﻿using System;
using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{   
    /// <summary>
    /// The query criteria for filtering events from the message log when reading.
    /// </summary>
    public class QueryCriteria
    {
        public QueryCriteria()
        {
            this.SourceTypes = new List<string>();
            this.SourceIds = new List<string>();
            this.AssemblyNames = new List<string>();
            this.Namespaces = new List<string>();
            this.FullNames = new List<string>();
            this.TypeNames = new List<string>();
        }

        public ICollection<string> SourceTypes { get; private set; }

        public ICollection<string> SourceIds { get; private set; }

        public ICollection<string> AssemblyNames { get; private set; }

        public ICollection<string> Namespaces { get; private set; }

        public ICollection<string> FullNames { get; private set; }

        public ICollection<string> TypeNames { get; private set; }

        public DateTimeOffset? EndDate { get; set; }
    }
}
