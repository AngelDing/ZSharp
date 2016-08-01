using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace Framework.SqlDb.Test.EfCommonTest
{
    public class EfCommonTest : IDisposable
    {
        private readonly EfCommonTestDb dbContext;

        public EfCommonTest()
        {
            InitDb();

            dbContext = new EfCommonTestDb();
        }

        [Fact]
        public void foreach_lazy_load_test()
        {
            var scores = dbContext.Scores
               .Take(100)
               .ToList();
            //訪問導航屬性的值，在沒有顯示加載，同時又啟用了延遲加載的情況下，每訪問一次就會訪問一次數據庫
            //建議禁用延遲加載，如果需要關聯子表信息，需要手動明確加載
            foreach (var item in scores)
            {
                var teacherName = item.Student.Name;
            }
        }

        [Fact]
        public void specified_field_test()
        {
            var scores = dbContext.Scores
                .Take(100)
                .Include(t => t.Student)
                .Select(t => new { t.ChineseFraction, t.CreateTime, StudentName = t.Student.Name })
                .ToList();
            foreach (var item in scores)
            {
                var teacherName = item.StudentName;
            }
        }

        [Fact]
        public void specified_field_auto_mapper_test()
        {
            //此处CreateMap应该放入Global.asax里面执行
            Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Score, ScoresDto>();
                    cfg.CreateMap<Student, ScoresDto.StudentDto>();
                });

            var scores = dbContext.Scores
                .Take(100)
                .ProjectTo<ScoresDto>()
                .ToList();
            foreach (var item in scores)
            {
                var teacherName = item.Student.Name;
            }
        }

        [Fact]
        public void select_many_test()
        {
            var students = dbContext.Students
                .Take(100)
                .SelectMany(t => t.Scores.Select(e => new { e.ScoreTyep, e.ChineseFraction, t.Name }))
                .GroupBy(t => t.ScoreTyep)
                .Select(t => new
                {
                    Count = t.Count(),
                    ChineseAvg = t.Average(e => e.ChineseFraction),
                    ScoreTyep = t.Key,
                    StudentName = t.Max(e => e.Name)
                })
                .Where(t => t.Count >= 3)
                .ToList();
            students.Should().NotBeNull();
        }

        [Fact]
        public void order_by_test()
        {
            var studentNames = dbContext.Students
                .AsNoTracking()
                .Where(t => t.Name.Contains("张三"))
                .OrderBy("name asc,age desc")
                .ToList();
            studentNames.Should().NotBeNullOrEmpty();
        }

        #region Init DB

        private void InitDb()
        {
            var isNeedAddData = false;
            using (var context = new EfCommonTestDb())
            {
                if (context.Database.Exists() == false)
                {
                    context.Database.Create();
                    isNeedAddData = true;
                }
            }
            if (isNeedAddData)
            {
                AddData();
            }
        }

        public void AddData()
        {
            using (var db = new EfCommonTestDb())
            {
                List<Teacher> teachers = new List<Teacher>();
                for (int i = 0; i < 1000; i++)
                {
                    teachers.Add(new Teacher()
                    {
                        Age = i.ToString(),
                        Name = "Teacher" + i.ToString()
                    });
                }
                db.Teachers.AddRange(teachers);
                db.SaveChanges();


                var teas = db.Teachers.ToList();
                List<Student> students = new List<Student>();
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        var r = new Random(j);
                        var stu = new Student()
                        {
                            //Id = 1,
                            Age = (j + i).ToString(),
                            Name = "张三" + j.ToString(),
                            Teachers = new List<Teacher>() {
                                   teas[r.Next(teas.Count())],
                                   teas[r.Next(teas.Count())],
                                   teas[r.Next(teas.Count())]
                                   }
                        };
                        students.Add(stu);
                    }
                }

                db.Students.AddRange(students);
                db.SaveChanges();

                var stus = db.Students.Select(t => t.Id).ToList();
                List<Score> scores = new List<Score>();
                if (stus.Count() > 0)
                    for (int z = 0; z < 10000; z++)
                    {
                        var r = new Random(z);
                        var score = new Score()
                        {
                            ChineseFraction = r.Next(100),
                            CreateTime = DateTime.Now,
                            EnglishFraction = r.Next(100),
                            MathematicsFraction = r.Next(100),
                            ScoreTyep = (ScoreTyep)r.Next(0, 2),
                            TeacherComment = GetTeacherComment(r),
                            StudentId = stus[r.Next(stus.Count())]
                        };
                        scores.Add(score);
                    }
                db.Scores.AddRange(scores);
                db.SaveChanges();
            }
        }

        private string GetTeacherComment(Random r)
        {
            var strkey = new string[] { "张三", "李四", "王五", "郑六" };
            return strkey[r.Next(strkey.Length)];
        }

        #endregion

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
