using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin_Book.Data.Migrations
{
    /// <inheritdoc />
    public partial class createdSettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFame",
                table: "Books",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFame",
                table: "Authors",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area1 = table.Column<double>(type: "float", nullable: false),
                    Area2 = table.Column<double>(type: "float", nullable: false),
                    Area3 = table.Column<double>(type: "float", nullable: false),
                    Area4 = table.Column<double>(type: "float", nullable: false),
                    Area5 = table.Column<double>(type: "float", nullable: false),
                    Area6 = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropColumn(
                name: "IsFame",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsFame",
                table: "Authors");
        }
    }
}
