
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

        public DbSet<EventSourcedEntity> EventSources { get; set; }

        public DbSet<MessageLogEntity> MessageLogs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventSourcedEntity>().ToTable("EventSource");
            modelBuilder.Entity<MessageLogEntity>().ToTable("MessageLog");
            modelBuilder.Entity<CommandMessageEntity>().ToTable("CommandBus");
            modelBuilder.Entity<EventMessageEntity>().ToTable("EventBus");
        }
    }
}
