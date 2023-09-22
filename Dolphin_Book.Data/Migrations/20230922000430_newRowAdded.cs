using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin_Book.Data.Migrations
{
    /// <inheritdoc />
    public partial class newRowAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalePercentage",
                table: "Toys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "exPrice",
                table: "Toys",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalePercentage",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "exPrice",
                table: "Books",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalePercentage",
                table: "Toys");

            migrationBuilder.DropColumn(
                name: "exPrice",
                table: "Toys");

            migrationBuilder.DropColumn(
                name: "SalePercentage",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "exPrice",
                table: "Books");
        }
    }
}
