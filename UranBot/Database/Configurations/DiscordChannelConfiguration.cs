namespace UranBot.Database.Configurations;

public sealed class DiscordChannelConfiguration : IEntityTypeConfiguration<DiscordChannel>
{
    public void Configure(EntityTypeBuilder<DiscordChannel> builder)
    {
        builder
            .ToTable(nameof(DiscordChannel), Const.Database.Schema.Discord);

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
            .HasIndex(x => x.DiscordId);
        
        builder
            .HasIndex(x => x.GuildId);
    }
}