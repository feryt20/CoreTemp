using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTemp.Data.Migrations.DbMain
{
    public partial class productEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandGroupId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FilePassword",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsFile",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsOtherUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OtherImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OtherUrl",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandGroupId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FilePassword",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Products",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFile",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOtherUrl",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherImageUrl",
                table: "Products",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherUrl",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
