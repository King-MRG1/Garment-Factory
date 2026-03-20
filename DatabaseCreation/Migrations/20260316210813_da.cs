using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCreation.Migrations
{
    /// <inheritdoc />
    public partial class da : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_Iron",
                table: "Models",
                newName: "Price_Ironer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_Ironer",
                table: "Models",
                newName: "Price_Iron");
        }
    }
}
