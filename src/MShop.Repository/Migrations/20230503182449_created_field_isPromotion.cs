using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MShop.Repository.Migrations
{
    public partial class created_field_isPromotion : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSale",
                table: "Products",
                type: "bool",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSale",
                table: "Products");
        }
    }
}
