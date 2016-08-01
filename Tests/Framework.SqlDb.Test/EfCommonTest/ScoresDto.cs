using System;

namespace Framework.SqlDb.Test.EfCommonTest
{
    public class ScoresDto
    {
        public int ChineseFraction { get; set; }
        public DateTime CreateTime { get; set; }

        public StudentDto Student { get; set; }

        public class StudentDto
        {
            public string Name { get; set; }
        }
    }
}
