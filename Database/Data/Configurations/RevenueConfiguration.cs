using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Configurations
{
    public class RevenueConfiguration : IEntityTypeConfiguration<Revenue>
    {
        public void Configure(EntityTypeBuilder<Revenue> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Revenue_Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Revenue_Description)
                .HasColumnType("nvarchar")
                .HasMaxLength(500);

            builder.Property(r => r.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(r => r.Trader)
                .WithMany(t => t.Revenues)
                .HasForeignKey(r => r.Trader_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
