using Microsoft.EntityFrameworkCore.Migrations;

namespace OTSSite.Data.Migrations
{
    public partial class addedModelProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Articles",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "published",
                table: "Articles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "published",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Articles",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
