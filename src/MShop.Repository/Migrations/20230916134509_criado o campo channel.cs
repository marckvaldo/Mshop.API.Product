using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MShop.Repository.Migrations
{
    public partial class criadoocampochannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "Products",
                type: "Varchar(100)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "Products");
        }
    }
}
