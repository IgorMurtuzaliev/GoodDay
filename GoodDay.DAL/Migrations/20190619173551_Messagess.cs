using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodDay.DAL.Migrations
{
    public partial class Messagess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
