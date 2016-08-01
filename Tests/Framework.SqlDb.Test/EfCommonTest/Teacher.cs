using System.Collections.Generic;

namespace Framework.SqlDb.Test.EfCommonTest
{
    /// <summary>
    /// 老师
    /// </summary>
    public class Teacher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}