using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseCreation.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Total_Cost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(o => o.Trader)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.Trader_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
