using System.Data.Entity;

namespace Framework.SqlDb.Test.EfCommonTest
{
    public class EfCommonTestDb : DbContext
    {
        public EfCommonTestDb()
            : base("Default")
        {
            //每次重新启动总是初始化数据库到最新版本（连接字符串为“Default”）
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EfCommonTestDb, Configuration>("Default"));
        }

        /// <summary>
        /// 学生集合
        /// </summary>
        public DbSet<Student> Students { get; set; }
        /// <summary>
        /// 老师集合
        /// </summary>
        public DbSet<Teacher> Teachers { get; set; } 
        /// <summary>
        /// 成绩集合
        /// </summary>
        public DbSet<Score> Scores { get; set; }

    }
}