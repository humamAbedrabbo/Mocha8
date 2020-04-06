using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.Migrations
{
    public partial class Add_Attachment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Attachements",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Attachements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RepositoryId",
                table: "Attachements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepositoryName",
                table: "Attachements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Attachements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Attachements");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Attachements");

            migrationBuilder.DropColumn(
                name: "RepositoryId",
                table: "Attachements");

            migrationBuilder.DropColumn(
                name: "RepositoryName",
                table: "Attachements");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Attachements");
        }
    }
}
