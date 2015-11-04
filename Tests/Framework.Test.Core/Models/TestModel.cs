using System;
using ZSharp.Framework.Dtos;

namespace Framework.Test.Core.Models
{
    public class TestModel : IDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public long Qty { get; set; }

        public byte Type { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
