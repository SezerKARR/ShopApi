using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class Stock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ProductFilterValues",
                newName: "CustomValue");

            migrationBuilder.AddColumn<string>(
                name: "ProductSellerIds",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "FilterValueId",
                table: "ProductFilterValues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductSeller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSeller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSeller_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSeller_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductSellerId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_ProductSeller_ProductSellerId",
                        column: x => x.ProductSellerId,
                        principalTable: "ProductSeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilterValues_FilterValueId",
                table: "ProductFilterValues",
                column: "FilterValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSeller_ProductId",
                table: "ProductSeller",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSeller_SellerId",
                table: "ProductSeller",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductSellerId",
                table: "Stock",
                column: "ProductSellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFilterValues_FilterValues_FilterValueId",
                table: "ProductFilterValues",
                column: "FilterValueId",
                principalTable: "FilterValues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFilterValues_FilterValues_FilterValueId",
                table: "ProductFilterValues");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "ProductSeller");

            migrationBuilder.DropIndex(
                name: "IX_ProductFilterValues_FilterValueId",
                table: "ProductFilterValues");

            migrationBuilder.DropColumn(
                name: "ProductSellerIds",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FilterValueId",
                table: "ProductFilterValues");

            migrationBuilder.RenameColumn(
                name: "CustomValue",
                table: "ProductFilterValues",
                newName: "Value");
        }
    }
}
