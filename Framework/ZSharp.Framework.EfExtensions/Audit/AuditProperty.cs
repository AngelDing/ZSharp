namespace ZSharp.Framework.EfExtensions.Audit
{
    public class AuditProperty
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsRelationship { get; set; }

        public object Current { get; set; }

        public object Original { get; set; }
    }
}
