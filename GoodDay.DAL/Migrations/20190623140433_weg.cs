using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodDay.DAL.Migrations
{
    public partial class weg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SharedUserId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharedUserName",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharedUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SharedUserName",
                table: "Messages");
        }
    }
}
