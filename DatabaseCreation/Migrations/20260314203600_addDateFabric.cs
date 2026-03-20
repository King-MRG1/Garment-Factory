using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCreation.Migrations
{
    /// <inheritdoc />
    public partial class addDateFabric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateAdded",
                table: "Fabrics",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Fabrics");
        }
    }
}
