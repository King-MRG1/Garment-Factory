using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Configurations
{
    public class TraderConfiguration : IEntityTypeConfiguration<Trader>
    {
        public void Configure(EntityTypeBuilder<Trader> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Trader_Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Address)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Trader_Type)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
