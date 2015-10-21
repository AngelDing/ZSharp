using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ZSharp.Framework.SqlDb
{
    public class CustomConnectionFactory : IDbConnectionFactory
    {
        private bool isIntegratedSecurity = false;
        private string dataSource, initialCatalog, userID, password;

        /// <summary>
        /// 使用windows用户验证方式
        /// </summary>
        /// <param name="dataSource">实例名</param>
        /// <param name="initialCatalog">指定数据库名</param>
        public CustomConnectionFactory(string dataSource, string initialCatalog)
        {
            this.isIntegratedSecurity = true;
            this.dataSource = dataSource;
            this.initialCatalog = initialCatalog;

        }
        /// <summary>
        /// 使用SQL用户名与密码验证方式
        /// </summary>
        /// <param name="dataSource">实例名</param>
        /// <param name="initialCatalog">指定数据库名</param>
        /// <param name="userID">用户名</param>
        /// <param name="password">密码</param>
        public CustomConnectionFactory(string dataSource, string initialCatalog, string userID, string password)
        {
            this.dataSource = dataSource;
            this.initialCatalog = initialCatalog;
            this.userID = userID;
            this.password = password;
        }
        public DbConnection CreateConnection(
          string nameOrConnectionString)
        {
            SqlConnectionStringBuilder builder;
            if (isIntegratedSecurity)
            {
                builder = new SqlConnectionStringBuilder
                {
                    DataSource = dataSource,
                    InitialCatalog = initialCatalog,
                    IntegratedSecurity = true,
                    MultipleActiveResultSets = true
                };
            }
            else
            {

                builder = new SqlConnectionStringBuilder
                {
                    DataSource = dataSource,
                    InitialCatalog = initialCatalog,
                    UserID = userID,
                    Password = password,
                    IntegratedSecurity = false,
                    MultipleActiveResultSets = true
                };
            }
            return new SqlConnection(builder.ToString());
        }
    }
}