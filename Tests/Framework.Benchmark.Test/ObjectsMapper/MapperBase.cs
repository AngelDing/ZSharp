using System;
namespace Framework.Benchmark.Test.ObjectsMapper
{
    public abstract class SimpleMapperBase : MapperBase<B2, A2>
    {
        protected override B2 CreateSource()
        {
            return new B2();
        }
    }

    public abstract class FlatterMapperBase : MapperBase<ModelObject, ModelDto>
    {
        protected override ModelObject CreateSource()
        {
            return new ModelObject
            {
                BaseDate = new DateTime(2007, 4, 5),
                Sub = new ModelSubObject
                {
                    ProperName = "Some name",
                    SubSub = new ModelSubSubObject
                    {
                        IAmACoolProperty = "Cool daddy-o"
                    }
                },
                Sub2 = new ModelSubObject
                {
                    ProperName = "Sub 2 name"
                },
                SubWithExtraName = new ModelSubObject
                {
                    ProperName = "Some other name"
                },
            };
        }
    }

    public abstract class MapperBase<TFrom, TTo> : IObjectToObjectMapper
    {
        protected TFrom Source { get; private set; }
        protected TTo Target { get; private set; }

        protected virtual void OnInitialize() { }
        protected abstract TFrom CreateSource();

        public void Initialize()
        {
            OnInitialize();
            Source = CreateSource();
        }

        protected abstract TTo MapImp();

        public void Map()
        {
            Target = MapImp();
        }
    }
}
