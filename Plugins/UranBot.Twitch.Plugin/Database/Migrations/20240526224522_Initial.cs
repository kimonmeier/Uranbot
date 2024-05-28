using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UranBot.Twitch.Plugin.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Twitch");

            migrationBuilder.CreateTable(
                name: "TwitchBroadcaster",
                schema: "Twitch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BroadcasterName = table.Column<string>(type: "TEXT", nullable: false),
                    TwitchId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchBroadcaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitchClip",
                schema: "Twitch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordMessageId = table.Column<long>(type: "INTEGER", nullable: false),
                    ClipId = table.Column<string>(type: "TEXT", nullable: false),
                    BroadcasterId = table.Column<long>(type: "INTEGER", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchClip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchClip_DiscordMessage_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitchClip_TwitchBroadcaster_BroadcasterId",
                        column: x => x.BroadcasterId,
                        principalSchema: "Twitch",
                        principalTable: "TwitchBroadcaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitchClipSettings",
                schema: "Twitch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordChannelId = table.Column<long>(type: "INTEGER", nullable: false),
                    ApprovalChannelId = table.Column<long>(type: "INTEGER", nullable: true),
                    BroadcasterId = table.Column<long>(type: "INTEGER", nullable: false),
                    ShareMode = table.Column<int>(type: "INTEGER", nullable: false),
                    LastSynched = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchClipSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchClipSettings_DiscordChannel_ApprovalChannelId",
                        column: x => x.ApprovalChannelId,
                        principalTable: "DiscordChannel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TwitchClipSettings_DiscordChannel_DiscordChannelId",
                        column: x => x.DiscordChannelId,
                        principalTable: "DiscordChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitchClipSettings_TwitchBroadcaster_BroadcasterId",
                        column: x => x.BroadcasterId,
                        principalSchema: "Twitch",
                        principalTable: "TwitchBroadcaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwitchBroadcaster_BroadcasterName",
                schema: "Twitch",
                table: "TwitchBroadcaster",
                column: "BroadcasterName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchClip_BroadcasterId",
                schema: "Twitch",
                table: "TwitchClip",
                column: "BroadcasterId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchClip_DiscordMessageId",
                schema: "Twitch",
                table: "TwitchClip",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchClipSettings_ApprovalChannelId",
                schema: "Twitch",
                table: "TwitchClipSettings",
                column: "ApprovalChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchClipSettings_BroadcasterId",
                schema: "Twitch",
                table: "TwitchClipSettings",
                column: "BroadcasterId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchClipSettings_DiscordChannelId",
                schema: "Twitch",
                table: "TwitchClipSettings",
                column: "DiscordChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitchClip",
                schema: "Twitch");

            migrationBuilder.DropTable(
                name: "TwitchClipSettings",
                schema: "Twitch");

            migrationBuilder.DropTable(
                name: "TwitchBroadcaster",
                schema: "Twitch");
        }
    }
}
