using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCreation.Migrations
{
    /// <inheritdoc />
    public partial class f : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_Inventory_Inventory_Id",
                table: "Fabrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_AspNetUsers_UserId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_Inventory_Inventory_Id",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Fabrics_Inventory_Id",
                table: "Fabrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_UserId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Inventory_Id",
                table: "Fabrics");

            migrationBuilder.RenameTable(
                name: "Inventory",
                newName: "Inventories");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Worker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Traders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Revenues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Phones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OrderModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Models",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Fabrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AdvanceAndDeductions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventories",
                table: "Inventories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Inventories_Inventory_Id",
                table: "Models",
                column: "Inventory_Id",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Inventories_Inventory_Id",
                table: "Models");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventories",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Traders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderModels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AdvanceAndDeductions");

            migrationBuilder.RenameTable(
                name: "Inventories",
                newName: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "Inventory_Id",
                table: "Fabrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Inventory",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_Inventory_Id",
                table: "Fabrics",
                column: "Inventory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_UserId",
                table: "Inventory",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_Inventory_Inventory_Id",
                table: "Fabrics",
                column: "Inventory_Id",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_AspNetUsers_UserId",
                table: "Inventory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Inventory_Inventory_Id",
                table: "Models",
                column: "Inventory_Id",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
