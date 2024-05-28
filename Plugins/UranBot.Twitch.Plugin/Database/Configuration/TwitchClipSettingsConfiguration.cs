namespace UranBot.Twitch.Plugin.Database.Configuration;

public class TwitchClipSettingsConfiguration : IEntityTypeConfiguration<TwitchClipSettings>
{
    public void Configure(EntityTypeBuilder<TwitchClipSettings> builder)
    {
        builder
            .ToTable(nameof(TwitchClipSettings), Const.Database.Schema.Twitch);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.ShareMode)
            .HasConversion<int>();
        
        builder
            .HasOne(x => x.Broadcaster)
            .WithMany()
            .HasForeignKey(x => x.BroadcasterId);

        builder
            .HasOne(x => x.DiscordChannel)
            .WithMany()
            .HasForeignKey(x => x.DiscordChannelId);

        builder
            .HasOne(x => x.ApprovalChannel)
            .WithMany()
            .HasForeignKey(x => x.ApprovalChannelId);
    }
}