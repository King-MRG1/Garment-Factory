using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseCreation.Data.Configurations
{
    public class RefreshTokenStoreConfigurationv : IEntityTypeConfiguration<RefreshTokenStore>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenStore> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.ExpiryDate)
                .IsRequired();

            builder.Property(r => r.IsRevoked)
                .HasDefaultValue(false);

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.Token)
           .IsUnique();

            builder.HasIndex(r => r.UserId);

            builder.HasIndex(r => r.ExpiryDate);
        }
    }
}
