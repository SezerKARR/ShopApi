using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class filterId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFilterValues_FilterValues_FilterValueId",
                table: "ProductFilterValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFilterValues_Filters_FilterId",
                table: "ProductFilterValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductFilterValues_FilterId",
                table: "ProductFilterValues");

            migrationBuilder.DropColumn(
                name: "ProductSellerIds",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FilterId",
                table: "ProductFilterValues");

            migrationBuilder.AlterColumn<int>(
                name: "FilterValueId",
                table: "ProductFilterValues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFilterValues_FilterValues_FilterValueId",
                table: "ProductFilterValues",
                column: "FilterValueId",
                principalTable: "FilterValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFilterValues_FilterValues_FilterValueId",
                table: "ProductFilterValues");

            migrationBuilder.AddColumn<string>(
                name: "ProductSellerIds",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "FilterValueId",
                table: "ProductFilterValues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FilterId",
                table: "ProductFilterValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilterValues_FilterId",
                table: "ProductFilterValues",
                column: "FilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFilterValues_FilterValues_FilterValueId",
                table: "ProductFilterValues",
                column: "FilterValueId",
                principalTable: "FilterValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFilterValues_Filters_FilterId",
                table: "ProductFilterValues",
                column: "FilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
