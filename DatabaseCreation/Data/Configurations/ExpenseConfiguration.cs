using DatabaseCreation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseCreation.Data.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Expense_Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Expense_Description)
                .HasColumnType("nvarchar")
                .HasMaxLength(500);

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(e => e.Trader)
                .WithMany(t => t.Expenses)
                .HasForeignKey(e => e.Trader_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
