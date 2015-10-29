
using FluentAssertions;
using System;
using System.Collections;
using Xunit;

namespace Framework.SqlDb.Test
{
    public class TableInheritanceTest
    {
        public TableInheritanceTest()
        {
            using (var context = new SchoolContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }

                context.Database.Create();
            }
        }

        //[Fact]
        //public void ef_temp_test()
        //{
        //    var a = 1;
        //    a.Should().Be(1);
        //}
    }
}
