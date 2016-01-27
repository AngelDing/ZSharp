using System;

namespace ZSharp.Framework.Domain
{
    public class MessageLogEntity : BaseDomainEntity
    {
        public string Kind { get; set; }

        public string SourceId { get; set; }

        public string AssemblyName { get; set; }

        public string Namespace { get; set; }

        public string FullName { get; set; }

        public string TypeName { get; set; }

        public string SourceType { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public string Payload { get; set; }
    }
}
