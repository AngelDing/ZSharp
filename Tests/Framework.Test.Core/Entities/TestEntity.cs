using ZSharp.Framework.Entities;

namespace Framework.Test.Core.Entities
{
    public class TestEntity : FullAuditedEntity<int, string>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public long Qty { get; set; }

        public byte Type { get; set; }

        public long Length { get; set; }
    }
}
