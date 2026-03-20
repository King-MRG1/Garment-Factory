using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseCreation.Data.Configurations
{
    public class AdvanceAndDeductionConfiguration : IEntityTypeConfiguration<AdvanceAndDeduction>
    {
        public void Configure(EntityTypeBuilder<AdvanceAndDeduction> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnType("nvarchar")
                .HasMaxLength(500);

            builder.Property(a => a.Type)
                .HasConversion<string>()
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(a => a.Worker)
                .WithMany(w => w.AdvanceAndDeductions)
                .HasForeignKey(a => a.Worker_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
