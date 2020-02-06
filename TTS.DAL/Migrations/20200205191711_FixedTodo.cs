using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TTS.DAL.Migrations
{
    public partial class FixedTodo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Statuses_JobStatusId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Jobs_JobId",
                table: "Todo");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobStatusId",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todo",
                table: "Todo");

            migrationBuilder.DropColumn(
                name: "JobStatusId",
                table: "Jobs");

            migrationBuilder.RenameTable(
                name: "Todo",
                newName: "Todos");

            migrationBuilder.RenameIndex(
                name: "IX_Todo_JobId",
                table: "Todos",
                newName: "IX_Todos_JobId");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Jobs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todos",
                table: "Todos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_StatusId",
                table: "Jobs",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Statuses_StatusId",
                table: "Jobs",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Jobs_JobId",
                table: "Todos",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Statuses_StatusId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Jobs_JobId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_StatusId",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todos",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Jobs");

            migrationBuilder.RenameTable(
                name: "Todos",
                newName: "Todo");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_JobId",
                table: "Todo",
                newName: "IX_Todo_JobId");

            migrationBuilder.AddColumn<Guid>(
                name: "JobStatusId",
                table: "Jobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todo",
                table: "Todo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobStatusId",
                table: "Jobs",
                column: "JobStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Statuses_JobStatusId",
                table: "Jobs",
                column: "JobStatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Jobs_JobId",
                table: "Todo",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
