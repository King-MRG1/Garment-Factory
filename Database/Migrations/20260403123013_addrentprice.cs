using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class addrentprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_Trader",
                table: "Models",
                newName: "Price_Trader_Rent");

            migrationBuilder.AddColumn<decimal>(
                name: "Price_Trader_Cash",
                table: "Models",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price_Trader_Cash",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "Price_Trader_Rent",
                table: "Models",
                newName: "Price_Trader");
        }
    }
}
