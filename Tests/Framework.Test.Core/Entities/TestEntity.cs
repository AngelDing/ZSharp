
using ZSharp.Framework.Entities;
using System;
namespace Framework.Test.Core.Entities
{
    public class TestEntity : IBusinessEntity<int, string>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public long Qty { get; set; }

        public byte Type { get; set; }

        public long Length { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
