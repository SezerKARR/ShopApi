using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class minprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "BasketItems",
                newName: "ProductSellerId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_ProductId",
                table: "BasketItems",
                newName: "IX_BasketItems_ProductSellerId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductSeller",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinPrice",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinPriceSellerId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_ProductSeller_ProductSellerId",
                table: "BasketItems",
                column: "ProductSellerId",
                principalTable: "ProductSeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_ProductSeller_ProductSellerId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinPriceSellerId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductSellerId",
                table: "BasketItems",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_ProductSellerId",
                table: "BasketItems",
                newName: "IX_BasketItems_ProductId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
