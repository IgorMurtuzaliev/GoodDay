using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodDay.DAL.Migrations
{
    public partial class fqq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePaths",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Files_MessageId",
                table: "Files",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Messages_MessageId",
                table: "Files",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Messages_MessageId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_MessageId",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "FilePaths",
                table: "Messages",
                nullable: true);
        }
    }
}
