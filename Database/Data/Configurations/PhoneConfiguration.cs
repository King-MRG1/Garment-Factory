using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Configurations
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Number)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(p => p.Worker)
                .WithMany(w => w.Phones)
                .HasForeignKey(p => p.Worker_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Trader)
                .WithMany(t => t.Phones)
                .HasForeignKey(p => p.Trader_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
