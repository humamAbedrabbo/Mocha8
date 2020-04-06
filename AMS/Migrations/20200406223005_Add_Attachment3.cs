using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.Migrations
{
    public partial class Add_Attachment3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Attachements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Attachements");
        }
    }
}
