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
            .HasOne(x => x.Guild)
            .WithMany()
            .HasForeignKey(x => x.GuildId);

        builder
            .HasIndex([nameof(TwitchBroadcaster.BroadcasterName), nameof(TwitchBroadcaster.GuildId)])
            .IsUnique();
    }
}