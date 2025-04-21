using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class BrandCategoryAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandCategory_Brand_BrandId",
                table: "BrandCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BrandCategory_Categories_CategoryId",
                table: "BrandCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrandCategory",
                table: "BrandCategory");

            migrationBuilder.RenameTable(
                name: "BrandCategory",
                newName: "BrandCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BrandCategory_CategoryId",
                table: "BrandCategories",
                newName: "IX_BrandCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandCategory_BrandId",
                table: "BrandCategories",
                newName: "IX_BrandCategories_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrandCategories",
                table: "BrandCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandCategories_Brand_BrandId",
                table: "BrandCategories",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrandCategories_Categories_CategoryId",
                table: "BrandCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandCategories_Brand_BrandId",
                table: "BrandCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BrandCategories_Categories_CategoryId",
                table: "BrandCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrandCategories",
                table: "BrandCategories");

            migrationBuilder.RenameTable(
                name: "BrandCategories",
                newName: "BrandCategory");

            migrationBuilder.RenameIndex(
                name: "IX_BrandCategories_CategoryId",
                table: "BrandCategory",
                newName: "IX_BrandCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandCategories_BrandId",
                table: "BrandCategory",
                newName: "IX_BrandCategory_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrandCategory",
                table: "BrandCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandCategory_Brand_BrandId",
                table: "BrandCategory",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrandCategory_Categories_CategoryId",
                table: "BrandCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
