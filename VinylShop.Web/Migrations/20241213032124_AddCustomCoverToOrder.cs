using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinylShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomCoverToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CustomCover",
                table: "OrdersProduct",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomCover",
                table: "OrdersProduct");
        }
    }
}
