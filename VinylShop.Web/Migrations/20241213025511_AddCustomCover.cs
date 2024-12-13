using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinylShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CustomCover",
                table: "CartItems",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomCover",
                table: "CartItems");
        }
    }
}
