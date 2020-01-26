using Microsoft.EntityFrameworkCore.Migrations;

namespace TTS.DAL.Migrations
{
    public partial class ManagerRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobStatus_JobStatusId",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobStatus",
                table: "JobStatus");

            migrationBuilder.RenameTable(
                name: "JobStatus",
                newName: "JobStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobStatuses",
                table: "JobStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusId",
                table: "Jobs",
                column: "JobStatusId",
                principalTable: "JobStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusId",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobStatuses",
                table: "JobStatuses");

            migrationBuilder.RenameTable(
                name: "JobStatuses",
                newName: "JobStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobStatus",
                table: "JobStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobStatus_JobStatusId",
                table: "Jobs",
                column: "JobStatusId",
                principalTable: "JobStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
