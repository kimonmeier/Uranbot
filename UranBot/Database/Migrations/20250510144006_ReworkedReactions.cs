using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UranBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReworkedReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RemoveRequestJson",
                schema: "Discord",
                table: "DiscordReaction",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
            
            migrationBuilder.RenameColumn(
                name: "RequestJson",
                schema: "Discord",
                table: "DiscordReaction",
                newName: "AddRequestJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoveRequestJson",
                schema: "Discord",
                table: "DiscordReaction");

            migrationBuilder.RenameColumn(
                name: "AddRequestJson",
                schema: "Discord",
                table: "DiscordReaction",
                newName: "RequestJson");
        }
    }
}
