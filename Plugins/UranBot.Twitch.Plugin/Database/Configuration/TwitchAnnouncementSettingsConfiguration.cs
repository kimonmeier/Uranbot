namespace UranBot.Twitch.Plugin.Database.Configuration;

public class TwitchAnnouncementSettingsConfiguration : IEntityTypeConfiguration<TwitchAnnouncementSettings>
{
    public void Configure(EntityTypeBuilder<TwitchAnnouncementSettings> builder)
    {
        builder
            .ToTable(nameof(TwitchAnnouncementSettings), Const.Database.Schema.Twitch);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Broadcaster)
            .WithMany()
            .HasForeignKey(x => x.BroadcasterId);

        builder
            .HasOne(x => x.Channel)
            .WithMany()
            .HasForeignKey(x => x.ChannelId);

        builder
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId);
        
        builder
            .HasIndex([nameof(TwitchAnnouncementSettings.BroadcasterId), nameof(TwitchAnnouncementSettings.ChannelId)])
            .IsUnique();
    }
}