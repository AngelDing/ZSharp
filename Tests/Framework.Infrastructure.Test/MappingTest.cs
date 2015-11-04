using Xunit;
using FluentAssertions;
using ZSharp.Framework.Infrastructure;
using Framework.Test.Core.Entities;
using System;
using Framework.Test.Core.Models;

namespace Framework.Service.Test
{
    public class MappingTest
    {
        [Fact]
        public void object_mapper_to_model_test()
        {
            var entity = new TestEntity
            {
                Id = 1, 
                Name = "Jacky",
                CreationTime = DateTime.Now,
                CreatedBy = "jia",
                IsDeleted = false,
                Length = 1000,
                Price = 100.50M,
                Qty = 20,
                Type = 1,
                LastModifiedBy = "jia",
                LastModificationTime = DateTime.Today.AddDays(2)
            };

            var model = entity.ToDto<TestModel>();

            model.Id.Should().Be(entity.Id);
            model.Name.Should().Be(entity.Name);
            model.Price.Should().Be(entity.Price);
            model.Qty.Should().Be(entity.Qty);
            model.CreatedDate.Should().Be(entity.CreationTime);
            model.Type.Should().Be(entity.Type);

            entity.Id = 2;
            entity.Name = "Zhou";
            entity.Length = 300;

            var model2 = entity.ToDto<TestEntity, TestModel>();

            model2.Id.Should().Be(2);
            model2.Name.Should().Be("Zhou");
            model2.Id.Should().Be(entity.Id);
            model2.Name.Should().Be(entity.Name);
            model2.Price.Should().Be(entity.Price);
            model2.Qty.Should().Be(entity.Qty);
            model2.CreatedDate.Should().Be(entity.CreationTime);
            model2.Type.Should().Be(entity.Type);
        }

        [Fact]
        public void object_mapper_to_entity_test()
        {
            var model = new TestModel
            {
                Id = 1,
                Name = "Jacky",
                CreatedDate = DateTime.Now,
                Price = 100.50M,
                Qty = 20,
                Type = 1
            };

            var entity = model.ToEntity<TestEntity>();

            entity.Id.Should().Be(model.Id);
            entity.Name.Should().Be(model.Name);
            entity.Price.Should().Be(model.Price);
            entity.Qty.Should().Be(model.Qty);
            entity.CreationTime.Should().Be(model.CreatedDate);
            entity.Type.Should().Be(model.Type);
            entity.LastModificationTime.HasValue.Should().BeFalse();

            model.Name = "xxxx";

            var entity2 = model.ToEntity<TestModel, TestEntity>();

            entity2.Name.Should().Be("xxxx");
        }

        [Fact]
        public void object_mapper_map_test()
        {
            var entity = new TestEntity 
            {               
                CreatedBy = "jia",
                IsDeleted = false,             
                LastModifiedBy = "jia",
                LastModificationTime = DateTime.Today.AddDays(2)
            };

            var model = new TestModel
            {
                Id = 1,
                Name = "Jacky",
                CreatedDate = DateTime.Now,
                Price = 100.50M,
                Qty = 20,
                Type = 1
            };

            model.Map<TestModel, TestEntity>(entity);

            entity.Id.Should().Be(1);
            entity.Name.Should().Be("Jacky");
            entity.LastModifiedBy.Should().Be("jia");

            entity = model.Map<TestEntity>();

            entity.Id.Should().Be(1);
            entity.Name.Should().Be("Jacky");
            entity.LastModifiedBy.Should().BeNull();
            entity.LastModificationTime.HasValue.Should().BeFalse();
        }
    }
}
