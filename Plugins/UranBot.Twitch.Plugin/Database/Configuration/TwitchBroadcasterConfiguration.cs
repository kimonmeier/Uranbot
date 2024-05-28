namespace UranBot.Twitch.Plugin.Database.Configuration;

public class TwitchBroadcasterConfiguration : IEntityTypeConfiguration<TwitchBroadcaster>
{
    public void Configure(EntityTypeBuilder<TwitchBroadcaster> builder)
    {
        builder
            .ToTable(nameof(TwitchBroadcaster), Const.Database.Schema.Twitch);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(x => x.BroadcasterName)
            .IsUnique();
    }
}