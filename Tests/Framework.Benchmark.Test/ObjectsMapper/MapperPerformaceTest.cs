
using Xunit;
namespace Framework.Benchmark.Test.ObjectsMapper
{
    public class MapperPerformaceTest
    {
        /// <summary>
        /// 性能測試建議單獨測試，測試完請註釋掉Fact屬性，以免全部跑用例時耗時過長
        /// </summary>
        [Fact]
        public void mapper_performance_test()
        {
            for (var i = 0; i < 2; i++)
            {
                new FlatterPerformaceSpec().Test();
                new SimplePerformanceSpec().Test();
            }
            Assert.True(true);
        }
    }
}
