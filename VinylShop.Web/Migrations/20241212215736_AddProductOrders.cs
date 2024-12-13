using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinylShop.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductEntity_Orders_OrderId",
                table: "OrderProductEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductEntity_Vinyls_ProductId",
                table: "OrderProductEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProductEntity",
                table: "OrderProductEntity");

            migrationBuilder.RenameTable(
                name: "OrderProductEntity",
                newName: "OrdersProduct");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProductEntity_ProductId",
                table: "OrdersProduct",
                newName: "IX_OrdersProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProductEntity_OrderId",
                table: "OrdersProduct",
                newName: "IX_OrdersProduct_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersProduct",
                table: "OrdersProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersProduct_Orders_OrderId",
                table: "OrdersProduct",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersProduct_Vinyls_ProductId",
                table: "OrdersProduct",
                column: "ProductId",
                principalTable: "Vinyls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersProduct_Orders_OrderId",
                table: "OrdersProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersProduct_Vinyls_ProductId",
                table: "OrdersProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersProduct",
                table: "OrdersProduct");

            migrationBuilder.RenameTable(
                name: "OrdersProduct",
                newName: "OrderProductEntity");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersProduct_ProductId",
                table: "OrderProductEntity",
                newName: "IX_OrderProductEntity_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersProduct_OrderId",
                table: "OrderProductEntity",
                newName: "IX_OrderProductEntity_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProductEntity",
                table: "OrderProductEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductEntity_Orders_OrderId",
                table: "OrderProductEntity",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductEntity_Vinyls_ProductId",
                table: "OrderProductEntity",
                column: "ProductId",
                principalTable: "Vinyls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
