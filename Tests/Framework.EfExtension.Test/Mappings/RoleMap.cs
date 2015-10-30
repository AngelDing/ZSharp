using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.EfExtension.Test
{
    public partial class RoleMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // table
            ToTable("Role", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.Description)
                .HasColumnName("Description")
                .HasMaxLength(150)
                .IsOptional();
            Property(t => t.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();
            Property(t => t.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired();
            Property(t => t.RowVersion)
                .HasColumnName("RowVersion")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasMaxLength(8)
                .IsRowVersion()
                .IsRequired();

            // Relationships
            HasMany(t => t.Users)
                .WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("UserRole", "dbo");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("UserId");
                });

        }
    }
}
