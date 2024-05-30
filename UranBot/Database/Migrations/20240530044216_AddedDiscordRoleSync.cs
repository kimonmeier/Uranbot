using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UranBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiscordRoleSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordRole",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<long>(type: "INTEGER", nullable: false),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRole_DiscordGuild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Discord",
                        principalTable: "DiscordGuild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_DiscordId",
                schema: "Discord",
                table: "DiscordRole",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_GuildId",
                schema: "Discord",
                table: "DiscordRole",
                column: "GuildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordRole",
                schema: "Discord");
        }
    }
}
