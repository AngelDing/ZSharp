using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Framework.SqlDb.Test.EfCommonTest
{
    /// <summary>
    /// 学生考分（成绩）
    /// </summary>
    public class Score
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        public int ChineseFraction { get; set; }

        public int MathematicsFraction { get; set; }

        public int EnglishFraction { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 老师批语
        /// </summary>
        //[Column("TeacherComment", TypeName = "varchar(max)")]
        public string TeacherComment { get; set; }

        /// <summary>
        /// 考试类型
        /// </summary>
        public ScoreTyep ScoreTyep { get; set; }
    }

    public enum ScoreTyep
    {
        模拟考试,
        正式考试
    }
}