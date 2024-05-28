using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UranBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Discord");

            migrationBuilder.CreateTable(
                name: "DiscordUser",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordGuild",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<long>(type: "INTEGER", nullable: true),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordGuild", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordGuild_DiscordUser_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Discord",
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscordChannel",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GuildId = table.Column<long>(type: "INTEGER", nullable: false),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_DiscordGuild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Discord",
                        principalTable: "DiscordGuild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordGuildMember",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordGuildMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordGuildMember_DiscordGuild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Discord",
                        principalTable: "DiscordGuild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscordGuildMember_DiscordUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Discord",
                        principalTable: "DiscordUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordMessage",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 25555, nullable: true),
                    ChannelId = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordMessage_DiscordChannel_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "Discord",
                        principalTable: "DiscordChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscordMessage_DiscordUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Discord",
                        principalTable: "DiscordUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscordReaction",
                schema: "Discord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<long>(type: "INTEGER", nullable: false),
                    EmoteName = table.Column<string>(type: "TEXT", nullable: false),
                    RequestJson = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordReaction_DiscordMessage_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Discord",
                        principalTable: "DiscordMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_DiscordId",
                schema: "Discord",
                table: "DiscordChannel",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_GuildId",
                schema: "Discord",
                table: "DiscordChannel",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordGuild_DiscordId",
                schema: "Discord",
                table: "DiscordGuild",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordGuild_OwnerId",
                schema: "Discord",
                table: "DiscordGuild",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordGuildMember_GuildId_UserId",
                schema: "Discord",
                table: "DiscordGuildMember",
                columns: new[] { "GuildId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordGuildMember_UserId",
                schema: "Discord",
                table: "DiscordGuildMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordMessage_ChannelId",
                schema: "Discord",
                table: "DiscordMessage",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordMessage_DiscordId",
                schema: "Discord",
                table: "DiscordMessage",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordMessage_UserId",
                schema: "Discord",
                table: "DiscordMessage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordReaction_MessageId_EmoteName",
                schema: "Discord",
                table: "DiscordReaction",
                columns: new[] { "MessageId", "EmoteName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUser_DiscordId",
                schema: "Discord",
                table: "DiscordUser",
                column: "DiscordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordGuildMember",
                schema: "Discord");

            migrationBuilder.DropTable(
                name: "DiscordReaction",
                schema: "Discord");

            migrationBuilder.DropTable(
                name: "DiscordMessage",
                schema: "Discord");

            migrationBuilder.DropTable(
                name: "DiscordChannel",
                schema: "Discord");

            migrationBuilder.DropTable(
                name: "DiscordGuild",
                schema: "Discord");

            migrationBuilder.DropTable(
                name: "DiscordUser",
                schema: "Discord");
        }
    }
}
