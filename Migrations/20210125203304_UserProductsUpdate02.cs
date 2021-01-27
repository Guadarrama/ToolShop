using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace toolShop.Migrations
{
    public partial class UserProductsUpdate02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPurchases",
                table: "UserPurchases");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "UserPurchases",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "UserPurchaseId",
                table: "UserPurchases",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPurchases",
                table: "UserPurchases",
                column: "UserPurchaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPurchases",
                table: "UserPurchases");

            migrationBuilder.DropColumn(
                name: "UserPurchaseId",
                table: "UserPurchases");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "UserPurchases",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPurchases",
                table: "UserPurchases",
                column: "ProductId");
        }
    }
}
