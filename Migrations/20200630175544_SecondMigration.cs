using Microsoft.EntityFrameworkCore.Migrations;

namespace toolShop.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UserCarts_UserCartId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserCartId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserCartId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "UserCarts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserCarts_ProductId",
                table: "UserCarts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCarts_Products_ProductId",
                table: "UserCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCarts_Products_ProductId",
                table: "UserCarts");

            migrationBuilder.DropIndex(
                name: "IX_UserCarts_ProductId",
                table: "UserCarts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "UserCarts");

            migrationBuilder.AddColumn<int>(
                name: "UserCartId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserCartId",
                table: "Products",
                column: "UserCartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UserCarts_UserCartId",
                table: "Products",
                column: "UserCartId",
                principalTable: "UserCarts",
                principalColumn: "UserCartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
