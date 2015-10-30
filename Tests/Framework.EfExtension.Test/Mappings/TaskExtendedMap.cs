﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.EfExtension.Test
{
    public partial class TaskExtendedMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<TaskExtended>
    {
        public TaskExtendedMap()
        {
            // table
            ToTable("TaskExtended", "dbo");

            // keys
            HasKey(t => t.TaskId);

            // Properties
            Property(t => t.TaskId)
                .HasColumnName("TaskId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();
            Property(t => t.Browser)
                .HasColumnName("Browser")
                .HasMaxLength(200)
                .IsOptional();
            Property(t => t.Os)
                .HasColumnName("OS")
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
            HasRequired(t => t.Task)
                .WithOptional(t => t.TaskExtended)
                .WillCascadeOnDelete(false);
        }
    }
}
