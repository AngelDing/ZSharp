using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ZSharp.Framework.SqlDb;

namespace Framework.SqlDb.Test
{
    public class EFTestContext : BaseCustomDbContext
    {      
        public EFTestContext()
            : base(ConstHelper.EFTestDBName)
        {
            this.LogChangesDuringSave = true;
        }

        public DbSet<EFCustomer> EFCustomer
        {
            get { return Set<EFCustomer>(); }
        }

        public DbSet<EFNote> EFNote
        {
            get { return Set<EFNote>(); }
        }

        public DbSet<ChildNote> ChildNote
        {
            get { return Set<ChildNote>(); }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约

            modelBuilder.Entity<EFCustomer>().HasKey(p => p.Id);
            modelBuilder.Entity<EFCustomer>().Property(
                p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<EFCustomer>().Property(t => t.Address.City).HasColumnName("City");
            modelBuilder.Entity<EFCustomer>().Property(t => t.Address.Country).HasColumnName("Country");
            modelBuilder.Entity<EFNote>().HasRequired(c => c.EFCustomer)
              .WithMany(t => t.EFNote).HasForeignKey(p => p.CustomerId);
            modelBuilder.Entity<EFNote>().HasKey(p => p.Id);

            modelBuilder.Entity<ChildNote>().HasRequired(c => c.EFNote)
            .WithMany(t => t.ChildNote).HasForeignKey(p => p.NoteId);
            modelBuilder.Entity<ChildNote>().HasKey(p => p.ChildNoteId);
     
            base.OnModelCreating(modelBuilder);
        }
    }
}
