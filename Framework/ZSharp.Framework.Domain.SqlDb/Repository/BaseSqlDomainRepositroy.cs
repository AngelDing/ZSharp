
namespace ZSharp.Framework.Domain
{
    public class BaseSqlDomainRepositroy : DisposableObject
    {
        protected DomainDbContext DB { get; private set; }
        public BaseSqlDomainRepositroy()
        {
            this.DB = new DomainDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DB.Dispose();
            }
        }
    }
}
