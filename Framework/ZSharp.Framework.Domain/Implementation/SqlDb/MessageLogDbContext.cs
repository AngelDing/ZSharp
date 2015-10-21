using System.Data.Entity;

namespace ZSharp.Framework.Domain
{
    public class MessageLogDbContext : DbContext
    {
        public const string SchemaName = "MessageLog";

        public MessageLogDbContext()
            : base("ConferenceDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MessageLogEntity>().ToTable("Messages", SchemaName);
        }
    }
}
