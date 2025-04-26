using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class SellerCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_ProductSellers_ProductSellerId",
                table: "Coupon");

            migrationBuilder.RenameColumn(
                name: "ProductSellerId",
                table: "Coupon",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Coupon_ProductSellerId",
                table: "Coupon",
                newName: "IX_Coupon_SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_Users_SellerId",
                table: "Coupon",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_Users_SellerId",
                table: "Coupon");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Coupon",
                newName: "ProductSellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Coupon_SellerId",
                table: "Coupon",
                newName: "IX_Coupon_ProductSellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_ProductSellers_ProductSellerId",
                table: "Coupon",
                column: "ProductSellerId",
                principalTable: "ProductSellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
