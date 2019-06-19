using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodDay.DAL.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Receiverid",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiverid",
                table: "Messages");
        }
    }
}
