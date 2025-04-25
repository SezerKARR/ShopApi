using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApi.Migrations
{
    /// <inheritdoc />
    public partial class Seller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_CreatedByUserId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "BrandUser");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Products",
                newName: "CreatedBySellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products",
                newName: "IX_Products_CreatedBySellerId");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Brand",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BrandId",
                table: "Users",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_SellerId",
                table: "Brand",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_Users_SellerId",
                table: "Brand",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_CreatedBySellerId",
                table: "Products",
                column: "CreatedBySellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Brand_BrandId",
                table: "Users",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brand_Users_SellerId",
                table: "Brand");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_CreatedBySellerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Brand_BrandId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BrandId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Brand_SellerId",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Brand");

            migrationBuilder.RenameColumn(
                name: "CreatedBySellerId",
                table: "Products",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreatedBySellerId",
                table: "Products",
                newName: "IX_Products_CreatedByUserId");

            migrationBuilder.CreateTable(
                name: "BrandUser",
                columns: table => new
                {
                    BrandAdminsId = table.Column<int>(type: "int", nullable: false),
                    ManagedBrandsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandUser", x => new { x.BrandAdminsId, x.ManagedBrandsId });
                    table.ForeignKey(
                        name: "FK_BrandUser_Brand_ManagedBrandsId",
                        column: x => x.ManagedBrandsId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandUser_Users_BrandAdminsId",
                        column: x => x.BrandAdminsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BrandUser_ManagedBrandsId",
                table: "BrandUser",
                column: "ManagedBrandsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
