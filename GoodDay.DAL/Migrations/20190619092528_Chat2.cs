using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodDay.DAL.Migrations
{
    public partial class Chat2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_ReceiverId",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_SenderId",
                table: "Dialogs");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Dialogs",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Dialogs",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Dialogs_SenderId",
                table: "Dialogs",
                newName: "IX_Dialogs_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Dialogs_ReceiverId",
                table: "Dialogs",
                newName: "IX_Dialogs_User1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_User1Id",
                table: "Dialogs",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_User2Id",
                table: "Dialogs",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_User1Id",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_User2Id",
                table: "Dialogs");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "Dialogs",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "Dialogs",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Dialogs_User2Id",
                table: "Dialogs",
                newName: "IX_Dialogs_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Dialogs_User1Id",
                table: "Dialogs",
                newName: "IX_Dialogs_ReceiverId");

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
    }
}
