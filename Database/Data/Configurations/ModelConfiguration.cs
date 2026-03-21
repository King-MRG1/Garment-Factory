using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Configurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Model_Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Price_Trader)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.Price_Stitcher)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.Price_Ironer)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.Price_Cutter)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.Total_Units)
                .HasDefaultValue(0);
        }
    }
}
