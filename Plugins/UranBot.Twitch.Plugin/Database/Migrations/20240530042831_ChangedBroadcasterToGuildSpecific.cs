using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UranBot.Twitch.Plugin.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBroadcasterToGuildSpecific : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TwitchBroadcaster_BroadcasterName",
                schema: "Twitch",
                table: "TwitchBroadcaster");

            migrationBuilder.AddColumn<long>(
                name: "GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchBroadcaster_BroadcasterName_GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster",
                columns: new[] { "BroadcasterName", "GuildId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchBroadcaster_GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_TwitchBroadcaster_DiscordGuild_GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster",
                column: "GuildId",
                principalTable: "DiscordGuild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TwitchBroadcaster_DiscordGuild_GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster");

            migrationBuilder.DropIndex(
                name: "IX_TwitchBroadcaster_BroadcasterName_GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster");

            migrationBuilder.DropIndex(
                name: "IX_TwitchBroadcaster_GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster");

            migrationBuilder.DropColumn(
                name: "GuildId",
                schema: "Twitch",
                table: "TwitchBroadcaster");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchBroadcaster_BroadcasterName",
                schema: "Twitch",
                table: "TwitchBroadcaster",
                column: "BroadcasterName",
                unique: true);
        }
    }
}
