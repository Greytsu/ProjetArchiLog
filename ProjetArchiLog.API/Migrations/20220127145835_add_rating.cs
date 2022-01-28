using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetArchiLog.API.Migrations
{
    public partial class add_rating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Product");
        }
    }
}
