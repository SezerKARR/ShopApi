using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class Coupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilterId",
                table: "ProductFilterValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductSellerId = table.Column<int>(type: "int", nullable: false),
                    Reduction = table.Column<int>(type: "int", nullable: true),
                    MinLimit = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupon_ProductSellers_ProductSellerId",
                        column: x => x.ProductSellerId,
                        principalTable: "ProductSellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilterValues_FilterId",
                table: "ProductFilterValues",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_ProductSellerId",
                table: "Coupon",
                column: "ProductSellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFilterValues_Filters_FilterId",
                table: "ProductFilterValues",
                column: "FilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFilterValues_Filters_FilterId",
                table: "ProductFilterValues");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropIndex(
                name: "IX_ProductFilterValues_FilterId",
                table: "ProductFilterValues");

            migrationBuilder.DropColumn(
                name: "FilterId",
                table: "ProductFilterValues");
        }
    }
}
