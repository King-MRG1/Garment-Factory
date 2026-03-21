using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Configurations
{
    public class OrderModelConfiguration : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(om => new { om.Order_Id, om.Model_Id });

            builder.Property(om => om.Quantity)
                .IsRequired();

            builder.HasOne(om => om.Order)
                .WithMany(o => o.OrderModels)
                .HasForeignKey(om => om.Order_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(om => om.Model)
                .WithMany(m => m.OrderModels)
                .HasForeignKey(om => om.Model_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
