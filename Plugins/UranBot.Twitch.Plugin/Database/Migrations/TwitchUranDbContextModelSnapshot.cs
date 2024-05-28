﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UranBot.Twitch.Plugin.Database;

#nullable disable

namespace UranBot.Twitch.Plugin.Database.Migrations
{
    [DbContext(typeof(TwitchUranDbContext))]
    partial class TwitchUranDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordChannel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("DiscordChannel", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordGuild", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("DiscordGuild", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("UserId");

                    b.ToTable("DiscordMessage", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("DiscordUser", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("UranBot.Twitch.Plugin.Database.Entities.TwitchBroadcaster", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BroadcasterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TwitchId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BroadcasterName")
                        .IsUnique();

                    b.ToTable("TwitchBroadcaster", "Twitch");
                });

            modelBuilder.Entity("UranBot.Twitch.Plugin.Database.Entities.TwitchClip", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("BroadcasterId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClipId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("DiscordMessageId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PostedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BroadcasterId");

                    b.HasIndex("DiscordMessageId");

                    b.ToTable("TwitchClip", "Twitch");
                });

            modelBuilder.Entity("UranBot.Twitch.Plugin.Database.Entities.TwitchClipSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ApprovalChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("BroadcasterId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DiscordChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastSynched")
                        .HasColumnType("TEXT");

                    b.Property<int>("ShareMode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalChannelId");

                    b.HasIndex("BroadcasterId");

                    b.HasIndex("DiscordChannelId");

                    b.ToTable("TwitchClipSettings", "Twitch");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordChannel", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordGuild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordGuild", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordMessage", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordChannel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UranBot.Public.Database.Entities.DiscordUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Channel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UranBot.Twitch.Plugin.Database.Entities.TwitchClip", b =>
                {
                    b.HasOne("UranBot.Twitch.Plugin.Database.Entities.TwitchBroadcaster", "Broadcaster")
                        .WithMany()
                        .HasForeignKey("BroadcasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UranBot.Public.Database.Entities.DiscordMessage", "DiscordMessage")
                        .WithMany()
                        .HasForeignKey("DiscordMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Broadcaster");

                    b.Navigation("DiscordMessage");
                });

            modelBuilder.Entity("UranBot.Twitch.Plugin.Database.Entities.TwitchClipSettings", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordChannel", "ApprovalChannel")
                        .WithMany()
                        .HasForeignKey("ApprovalChannelId");

                    b.HasOne("UranBot.Twitch.Plugin.Database.Entities.TwitchBroadcaster", "Broadcaster")
                        .WithMany()
                        .HasForeignKey("BroadcasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UranBot.Public.Database.Entities.DiscordChannel", "DiscordChannel")
                        .WithMany()
                        .HasForeignKey("DiscordChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovalChannel");

                    b.Navigation("Broadcaster");

                    b.Navigation("DiscordChannel");
                });
#pragma warning restore 612, 618
        }
    }
}
