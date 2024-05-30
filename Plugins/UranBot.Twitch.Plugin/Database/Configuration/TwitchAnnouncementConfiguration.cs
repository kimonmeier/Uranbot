namespace UranBot.Twitch.Plugin.Database.Configuration;

public class TwitchAnnouncementConfiguration : IEntityTypeConfiguration<TwitchAnnouncement>
{
    public void Configure(EntityTypeBuilder<TwitchAnnouncement> builder)
    {
        builder
            .ToTable(nameof(TwitchAnnouncement), Const.Database.Schema.Twitch);

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
            .HasOne(x => x.Message)
            .WithMany()
            .HasForeignKey(x => x.MessageId);

        builder
            .HasIndex(x => x.BroadcasterId)
            .IsUnique();
    }
}