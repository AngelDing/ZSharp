using System.Data.Entity.ModelConfiguration;

namespace ZSharp.Framework.SqlDb
{
    internal class SequenceConfiguration : EntityTypeConfiguration<SqlServerSequence>
    {
        public SequenceConfiguration()
        {            
            Property(c => c.RowVersion).IsRowVersion();
            HasKey(s => s.Key);
        }
    }
}
