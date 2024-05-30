using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UranBot.Twitch.Plugin.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitchAnnouncement",
                schema: "Twitch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<long>(type: "INTEGER", nullable: false),
                    BroadcasterId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchAnnouncement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchAnnouncement_DiscordMessage_MessageId",
                        column: x => x.MessageId,
                        principalTable: "DiscordMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitchAnnouncement_TwitchBroadcaster_BroadcasterId",
                        column: x => x.BroadcasterId,
                        principalSchema: "Twitch",
                        principalTable: "TwitchBroadcaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitchAnnouncementSettings",
                schema: "Twitch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BroadcasterId = table.Column<long>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<long>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchAnnouncementSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchAnnouncementSettings_DiscordChannel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "DiscordChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitchAnnouncementSettings_DiscordRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "DiscordRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitchAnnouncementSettings_TwitchBroadcaster_BroadcasterId",
                        column: x => x.BroadcasterId,
                        principalSchema: "Twitch",
                        principalTable: "TwitchBroadcaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAnnouncement_BroadcasterId",
                schema: "Twitch",
                table: "TwitchAnnouncement",
                column: "BroadcasterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAnnouncement_MessageId",
                schema: "Twitch",
                table: "TwitchAnnouncement",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAnnouncementSettings_BroadcasterId_ChannelId",
                schema: "Twitch",
                table: "TwitchAnnouncementSettings",
                columns: new[] { "BroadcasterId", "ChannelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAnnouncementSettings_ChannelId",
                schema: "Twitch",
                table: "TwitchAnnouncementSettings",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAnnouncementSettings_RoleId",
                schema: "Twitch",
                table: "TwitchAnnouncementSettings",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitchAnnouncement",
                schema: "Twitch");

            migrationBuilder.DropTable(
                name: "TwitchAnnouncementSettings",
                schema: "Twitch");
        }
    }
}
