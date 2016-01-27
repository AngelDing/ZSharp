
using System.Data.Entity;

namespace ZSharp.Framework.Domain
{
    public class DomainDbContext : DbContext
    {
        public DomainDbContext()
            : base("DomainDb")
        {
        }

        public DbSet<CommandMessageEntity> Commands { get; set; }

        public DbSet<EventMessageEntity> Events { get; set; }

        public DbSet<DomainEventEntity> DomainEvents { get; set; }

        public DbSet<MessageLogEntity> MessageLogs { get; set; }

        public DbSet<SnapshotEntity> Snapshots { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DomainEventEntity>().ToTable("DomainEvent");
            modelBuilder.Entity<MessageLogEntity>().ToTable("MessageLog");
            modelBuilder.Entity<CommandMessageEntity>().ToTable("CommandBus");
            modelBuilder.Entity<EventMessageEntity>().ToTable("EventBus");
            modelBuilder.Entity<SnapshotEntity>().ToTable("Snapshot");
        }
    }
}
