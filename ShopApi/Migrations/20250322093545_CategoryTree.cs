using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class CategoryTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_MainCategories_mainCategoryId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "mainCategoryId",
                table: "Categories",
                newName: "MainCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_mainCategoryId",
                table: "Categories",
                newName: "IX_Categories_MainCategoryId");

            migrationBuilder.CreateTable(
                name: "CategoryTrees",
                columns: table => new
                {
                    AncestorId = table.Column<int>(type: "int", nullable: false),
                    DescendantId = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTrees", x => new { x.AncestorId, x.DescendantId });
                    table.ForeignKey(
                        name: "FK_CategoryTrees_Categories_AncestorId",
                        column: x => x.AncestorId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTrees_Categories_DescendantId",
                        column: x => x.DescendantId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrees_DescendantId",
                table: "CategoryTrees",
                column: "DescendantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_MainCategories_MainCategoryId",
                table: "Categories",
                column: "MainCategoryId",
                principalTable: "MainCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_MainCategories_MainCategoryId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryTrees");

            migrationBuilder.RenameColumn(
                name: "MainCategoryId",
                table: "Categories",
                newName: "mainCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_MainCategoryId",
                table: "Categories",
                newName: "IX_Categories_mainCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_MainCategories_mainCategoryId",
                table: "Categories",
                column: "mainCategoryId",
                principalTable: "MainCategories",
                principalColumn: "Id");
        }
    }
}
