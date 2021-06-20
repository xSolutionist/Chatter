using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatter.Migrations.ChatterMessage
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "RecievingUser",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendingUser",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecievingUser",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SendingUser",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
