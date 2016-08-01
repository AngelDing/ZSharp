namespace Framework.SqlDb.Test.EfCommonTest
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<EfCommonTestDb>
    {
        public Configuration()
        {
            //�����Զ�Ǩ��
            AutomaticMigrationsEnabled = true;
            //�Զ�Ǩ��Ĭ������²��ӵ��������ǵ����ݿ��еı�������ǲ�ϣ����������Ϊ�����ǿ��Ը���Ǩ����ȷ�������ݶ�ʧ���������AutomaticMigrationDataLossAllowed��������Ϊtrue
            AutomaticMigrationDataLossAllowed = true;  
        }

        protected override void Seed(EfCommonTestDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
