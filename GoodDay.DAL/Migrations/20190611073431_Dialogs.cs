using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodDay.DAL.Migrations
{
    public partial class Dialogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_UserId",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_UserId",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Dialogs");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Dialogs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Dialogs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_ReceiverId",
                table: "Dialogs",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_SenderId",
                table: "Dialogs",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_ReceiverId",
                table: "Dialogs",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_SenderId",
                table: "Dialogs",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_ReceiverId",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_SenderId",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_ReceiverId",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_SenderId",
                table: "Dialogs");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Dialogs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Dialogs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Dialogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_UserId",
                table: "Dialogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_UserId",
                table: "Dialogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
