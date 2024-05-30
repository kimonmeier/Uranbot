﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UranBot.Database;

#nullable disable

namespace UranBot.Database.Migrations
{
    [DbContext(typeof(CoreUranDbContext))]
    [Migration("20240530044216_AddedDiscordRoleSync")]
    partial class AddedDiscordRoleSync
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasIndex("DiscordId");

                    b.HasIndex("GuildId");

                    b.ToTable("DiscordChannel", "Discord");
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

                    b.HasIndex("DiscordId");

                    b.HasIndex("OwnerId");

                    b.ToTable("DiscordGuild", "Discord");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordGuildMember", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("GuildId", "UserId");

                    b.ToTable("DiscordGuildMember", "Discord");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasMaxLength(25555)
                        .HasColumnType("TEXT");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("DiscordId");

                    b.HasIndex("UserId");

                    b.ToTable("DiscordMessage", "Discord");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordReaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmoteName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("MessageId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RequestJson")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("MessageId", "EmoteName")
                        .IsUnique();

                    b.ToTable("DiscordReaction", "Discord");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DiscordId");

                    b.HasIndex("GuildId");

                    b.ToTable("DiscordRole", "Discord");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DiscordId");

                    b.ToTable("DiscordUser", "Discord");
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

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordGuildMember", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordGuild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UranBot.Public.Database.Entities.DiscordUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("User");
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

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordReaction", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordMessage", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("UranBot.Public.Database.Entities.DiscordRole", b =>
                {
                    b.HasOne("UranBot.Public.Database.Entities.DiscordGuild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });
#pragma warning restore 612, 618
        }
    }
}
