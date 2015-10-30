
namespace Framework.EfExtension.Test
{
    public partial class TrackerContext
          : System.Data.Entity.DbContext
    {
        public TrackerContext()
            : base("Name=TrackerContext")
        {
            InitializeContext();
        }

        public TrackerContext(System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base("Name=TrackerContext", model)
        {
            InitializeContext();
        }

        public TrackerContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InitializeContext();
        }

        public TrackerContext(string nameOrConnectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            InitializeContext();
        }

        public TrackerContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializeContext();
        }

        public TrackerContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializeContext();
        }

        public System.Data.Entity.DbSet<AuditData> Audits { get; set; }
        public System.Data.Entity.DbSet<Task> Tasks { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<Priority> Priorities { get; set; }
        public System.Data.Entity.DbSet<Role> Roles { get; set; }
        public System.Data.Entity.DbSet<Status> Status { get; set; }
        public System.Data.Entity.DbSet<TaskExtended> TaskExtendeds { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuditMap());
            modelBuilder.Configurations.Add(new TaskMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new PriorityMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new TaskExtendedMap());

            InitializeMapping(modelBuilder);
        }

        partial void InitializeContext();
        partial void InitializeMapping(System.Data.Entity.DbModelBuilder modelBuilder);
    }
}