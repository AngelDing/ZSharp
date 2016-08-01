using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.SqlDb.Test.EfCommonTest
{
    /// <summary>
    /// 学生
    /// </summary>
    public class Student
    {
        public int Id { get; set; }

        //[Column("Name", TypeName = "nvarchar(max)")]
        public string Name { get; set; }

        public string Age { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }

        /// <summary>
        /// 学上考分成绩
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual ICollection<Score> Scores { get; set; }
    }
}