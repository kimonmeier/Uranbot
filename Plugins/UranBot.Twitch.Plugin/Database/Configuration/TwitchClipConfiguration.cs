namespace UranBot.Twitch.Plugin.Database.Configuration;

public sealed class TwitchClipConfiguration : IEntityTypeConfiguration<Entities.TwitchClip>
{
    public void Configure(EntityTypeBuilder<Entities.TwitchClip> builder)
    {
        builder
            .ToTable(nameof(Entities.TwitchClip), Const.Database.Schema.Twitch);

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.PostedAt)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.DiscordMessage)
            .WithMany()
            .HasForeignKey(x => x.DiscordMessageId);
        
        builder
            .HasIndex(x => x.BroadcasterId);
    }
}