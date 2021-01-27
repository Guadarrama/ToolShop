using Microsoft.EntityFrameworkCore.Migrations;

namespace toolShop.Migrations
{
    public partial class UserProductsUpdate04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPurchases_ProductId",
                table: "UserPurchases",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPurchases_Products_ProductId",
                table: "UserPurchases",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPurchases_Products_ProductId",
                table: "UserPurchases");

            migrationBuilder.DropIndex(
                name: "IX_UserPurchases_ProductId",
                table: "UserPurchases");
        }
    }
}
