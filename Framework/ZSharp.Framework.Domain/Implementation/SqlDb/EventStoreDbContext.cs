using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Used by <see cref="SqlEventSourcedRepository{T}"/>, and is used only for running the sample application
    /// without the dependency to the Windows Azure Service Bus when using the DebugLocal solution configuration.
    /// </summary>
    public class EventStoreDbContext : DbContext
    {
        public const string SchemaName = "Events";

        public EventStoreDbContext()
            : base("ConferenceDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventEntity>().ToTable("Events", SchemaName);
            modelBuilder.Entity<EventEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<EventEntity>().Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
