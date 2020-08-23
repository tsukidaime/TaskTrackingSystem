using Microsoft.EntityFrameworkCore.Migrations;

namespace TTS.DAL.Migrations
{
    public partial class DeleteJobProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Jobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
