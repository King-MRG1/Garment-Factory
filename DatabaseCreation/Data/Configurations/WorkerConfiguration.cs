using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseCreation.Data.Configurations
{
    public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Worker_Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Worker_Type)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(w => w.Address)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
