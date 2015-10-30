using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;
using System.Reflection;
using NLite;


namespace Framework.Benchmark.Test.ObjectsMapper
{
    public class SimplePerformanceSpec : PerformanceSpecBase
    {
        protected override void RegisterComponents()
        {
            Mappers.Add("Simple.Class.AutoMapper", new AutoMapperWrapper());
            Mappers.Add("Simple.Class.ObjectMapper", new ObjectMapperWrapper());
            Mappers.Add("Simple.Class.NLiteMapper", new NLiteMaperWrapper());
            Mappers.Add("Simple.Class.EmitMapper", new EmitMapperWrapper());
            Mappers.Add("Simple.Class.Manual", new ManualMapper());
        }       

        public class ObjectMapperWrapper : SimpleMapperBase
        {
            protected override A2 MapImp()
            {
                return ObjectMapper.Mapper.Map<B2, A2>(Source);
            }
        }

        public class ManualMapper : SimpleMapperBase
        {
            protected override A2 MapImp()
            {
                var result = new A2();
                result.str1 = Source.str1;
                result.str2 = Source.str2;
                result.str3 = Source.str3;
                result.str4 = Source.str4;
                result.str5 = Source.str5;
                result.str6 = Source.str6;
                result.str7 = Source.str7;
                result.str8 = Source.str8;
                result.str9 = Source.str9;

                return result;
            }
        }

        public class AutoMapperWrapper : SimpleMapperBase
        {
            protected override void OnInitialize()
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<B2, A2>();
                    AutoMapper.Mapper.AssertConfigurationIsValid();
                });
            }

            protected override A2 MapImp()
            {
                try
                {
                    return AutoMapper.Mapper.Map<B2, A2>(Source);
                }
                finally
                {
                }
            }
        }

        public class EmitMapperWrapper : SimpleMapperBase
        {
            ObjectsMapper<B2, A2> mapper;
            protected override void OnInitialize()
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<B2, A2>();
            }
            protected override A2 MapImp()
            {
                return mapper.Map(Source);
            }
        }

        public class NLiteMaperWrapper : SimpleMapperBase
        {
            private NLite.Mapping.IMapper<B2, A2> mapper;
            protected override void OnInitialize()
            {
                base.OnInitialize();

                mapper = NLite.Mapper.CreateMapper<B2, A2>();
            }

            protected override A2 MapImp()
            {
                return mapper.Map(Source);
            }
        }      
    }
}
