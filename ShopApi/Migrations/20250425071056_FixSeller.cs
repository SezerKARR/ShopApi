using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class FixSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_ProductSeller_ProductSellerId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSeller_Products_ProductId",
                table: "ProductSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSeller_Users_SellerId",
                table: "ProductSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_ProductSeller_ProductSellerId",
                table: "Stock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSeller",
                table: "ProductSeller");

            migrationBuilder.RenameTable(
                name: "ProductSeller",
                newName: "ProductSellers");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSeller_SellerId",
                table: "ProductSellers",
                newName: "IX_ProductSellers_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSeller_ProductId",
                table: "ProductSellers",
                newName: "IX_ProductSellers_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSellers",
                table: "ProductSellers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_ProductSellers_ProductSellerId",
                table: "BasketItems",
                column: "ProductSellerId",
                principalTable: "ProductSellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellers_Products_ProductId",
                table: "ProductSellers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSellers_Users_SellerId",
                table: "ProductSellers",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_ProductSellers_ProductSellerId",
                table: "Stock",
                column: "ProductSellerId",
                principalTable: "ProductSellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_ProductSellers_ProductSellerId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellers_Products_ProductId",
                table: "ProductSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSellers_Users_SellerId",
                table: "ProductSellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_ProductSellers_ProductSellerId",
                table: "Stock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSellers",
                table: "ProductSellers");

            migrationBuilder.RenameTable(
                name: "ProductSellers",
                newName: "ProductSeller");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSellers_SellerId",
                table: "ProductSeller",
                newName: "IX_ProductSeller_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSellers_ProductId",
                table: "ProductSeller",
                newName: "IX_ProductSeller_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSeller",
                table: "ProductSeller",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_ProductSeller_ProductSellerId",
                table: "BasketItems",
                column: "ProductSellerId",
                principalTable: "ProductSeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSeller_Products_ProductId",
                table: "ProductSeller",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSeller_Users_SellerId",
                table: "ProductSeller",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_ProductSeller_ProductSellerId",
                table: "Stock",
                column: "ProductSellerId",
                principalTable: "ProductSeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
