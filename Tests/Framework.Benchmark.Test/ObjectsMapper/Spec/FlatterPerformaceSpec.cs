using EmitMapper;
using System;

namespace Framework.Benchmark.Test.ObjectsMapper
{
    public class FlatterPerformaceSpec : PerformanceSpecBase
    {     
        protected override void RegisterComponents()
        {
            Mappers.Add("Flattening.Class.AutoMapper", new AutoMapperWrapper());
            Mappers.Add("Flattening.Class.ObjectMapper", new ObjectMapperWrapper());
            Mappers.Add("Flattening.Class.NLiteMapper", new NLiteMaperWrapper());
            Mappers.Add("Flattening.Class.EmitMapper", new EmitMapperWrapper());
            Mappers.Add("Flattening.Class.Manual", new ManualMapper());
        }

        public class AutoMapperWrapper : FlatterMapperBase
        {
            protected override void OnInitialize()
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ModelObject, ModelDto>();
                });
                AutoMapper.Mapper.AssertConfigurationIsValid();
            }

            protected override ModelDto MapImp()
            {
                return AutoMapper.Mapper.Map<ModelObject, ModelDto>(Source);
            }
        }

        public class ObjectMapperWrapper : FlatterMapperBase
        {
            protected override ModelDto MapImp()
            {
                return ObjectMapper.Mapper.Map<ModelObject, ModelDto>(Source);
            }
        }

        public class NLiteMaperWrapper : FlatterMapperBase
        {
            private NLite.Mapping.IMapper<ModelObject, ModelDto> mapper;
            protected override void OnInitialize()
            {
                base.OnInitialize();

                mapper = NLite.Mapper.CreateMapper<ModelObject, ModelDto>();
            }

            protected override ModelDto MapImp()
            {
                return mapper.Map(Source);
            }
        }

        public class EmitMapperWrapper : FlatterMapperBase
        {
            ObjectsMapper<ModelObject, ModelDto> mapper;
            protected override void OnInitialize()
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<ModelObject, ModelDto>(new FlatteringConfig());
            }

            protected override ModelDto MapImp()
            {
                return mapper.Map(Source);
            }
        }

        public class ManualMapper : FlatterMapperBase
        {
            protected override ModelDto MapImp()
            {
                return new ModelDto
                {
                    BaseDate = Source.BaseDate,
                    Sub2ProperName = Source.Sub2.ProperName,
                    SubProperName = Source.Sub.ProperName,
                    SubSubSubIAmACoolProperty = Source.Sub.SubSub.IAmACoolProperty,
                    SubWithExtraNameProperName = Source.SubWithExtraName.ProperName
                };
            }
        }        
    }
}
