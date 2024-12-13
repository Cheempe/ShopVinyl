using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinylShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Cover",
                table: "Vinyls",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Vinyls");
        }
    }
}
