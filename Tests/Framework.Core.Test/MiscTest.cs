using Xunit;
using ZSharp.Framework.Entities;

namespace Framework.Core.Test
{
    public class MiscTest
    {
        [Fact]
        public void nameof_test()
        {
            var result = nameof(ISoftDeletable.IsDeleted);
            Assert.Equal("IsDeleted", result);
        }
    }
}
