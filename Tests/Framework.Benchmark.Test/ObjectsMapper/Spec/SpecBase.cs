
namespace Framework.Benchmark.Test.ObjectsMapper
{
    public class SpecBase
    {
        public SpecBase()
        {
            NLite.Mapper.Reset();
            //AutoMapper.Mapper.Reset();
            EmitMapper.ObjectMapperManager._defaultInstance = null;
            SetUp();
        }

        public void SetUp()
        {
            Given();
            //When();
        }

        public virtual void Given()
        {
        }

        public virtual void Test()
        {
        }
    }
}
