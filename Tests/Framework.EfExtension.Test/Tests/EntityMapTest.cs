using ZSharp.Framework.EfExtensions.Mapping;
using System;
using Xunit;

namespace Framework.EfExtension.Test
{
    public class EntityMapTest
    {
        [Fact]
        public void ef_mapping_test()
        {
            var db = new TrackerContext();
            var resolver = new MetadataMappingProvider();

            var map = resolver.GetEntityMap(typeof(AuditData), db);

            Assert.Equal("[dbo].[AuditData]", map.TableFullName);

            resolver = new MetadataMappingProvider();

            map = resolver.GetEntityMap(typeof(AuditData), db);

            Assert.Equal("[dbo].[AuditData]", map.TableFullName);
        }
    }
}
