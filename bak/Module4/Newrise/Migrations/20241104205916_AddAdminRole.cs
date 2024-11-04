using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newrise.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Participants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Participants",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Participants");
        }
    }
}
