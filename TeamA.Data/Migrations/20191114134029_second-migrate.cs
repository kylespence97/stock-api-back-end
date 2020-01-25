using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamA.Data.Migrations
{
    public partial class secondmigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ResellPrice",
                table: "Stock",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResellPrice",
                table: "Stock");
        }
    }
}
