using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseCreation.Data.Configurations
{
    public class FabricConfiguration : IEntityTypeConfiguration<Fabric>
    {
        public void Configure(EntityTypeBuilder<Fabric> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Fabric_Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Metres)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(f => f.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(f => f.Trader)
                .WithMany(t => t.Fabrics)
                .HasForeignKey(f => f.Trader_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
