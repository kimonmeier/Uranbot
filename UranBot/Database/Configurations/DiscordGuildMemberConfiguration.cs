namespace UranBot.Database.Configurations;

public sealed class DiscordGuildMemberConfiguration : IEntityTypeConfiguration<DiscordGuildMember>
{
    public void Configure(EntityTypeBuilder<DiscordGuildMember> builder)
    {
        builder
            .ToTable(nameof(DiscordGuildMember), Const.Database.Schema.Discord);

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Guild)
            .WithMany()
            .HasForeignKey(x => x.GuildId);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder
            .HasIndex([
                nameof(DiscordGuildMember.GuildId), nameof(DiscordGuildMember.UserId)
            ]);
    }
}