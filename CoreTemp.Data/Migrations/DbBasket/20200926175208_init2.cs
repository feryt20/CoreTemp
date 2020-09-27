using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTemp.Data.Migrations.DbBasket
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Baskets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Baskets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Baskets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ProductId",
                table: "Baskets",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Baskets_ProductId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Baskets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Baskets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Baskets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
