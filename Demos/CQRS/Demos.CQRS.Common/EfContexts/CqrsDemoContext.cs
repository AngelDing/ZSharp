using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Demos.CQRS.Common
{
    public class CqrsDemoContext : DbContext
    {
        public CqrsDemoContext()
            : base("CqrsDemoDb")
        { }

        public DbSet<CustomerEntity> Customers
        {
            get { return Set<CustomerEntity>(); }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<CustomerEntity>().Property(p => p.Id).IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            base.OnModelCreating(modelBuilder);
        }
    }
}
