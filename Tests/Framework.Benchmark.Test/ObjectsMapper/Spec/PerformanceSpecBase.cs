using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;
using System.Reflection;
using NLite;
using Xunit;
using Framework.Test.Core;

namespace Framework.Benchmark.Test.ObjectsMapper
{
    public abstract class PerformanceSpecBase : SpecBase
    {
        protected Dictionary<string,IObjectToObjectMapper> Mappers = new Dictionary<string,IObjectToObjectMapper>();

        protected abstract void RegisterComponents();

        public override void Given()
        {
            RegisterComponents();

            foreach (var item in Mappers)
            {
                item.Value.Initialize();
                item.Value.Map();
            }
        }

        public override void Test()
        {
            foreach (var item in Mappers)
            {
                CodeTimer.Time(item.Key, 100000, () => item.Value.Map());
            }
        }
    }  
}