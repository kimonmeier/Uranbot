namespace UranBot.Database.Configurations;

public class DiscordReactionConfiguration : IEntityTypeConfiguration<DiscordReaction>
{
    public void Configure(EntityTypeBuilder<DiscordReaction> builder)
    {
        builder
            .ToTable(nameof(DiscordReaction), Const.Database.Schema.Discord);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Message)
            .WithMany()
            .HasForeignKey(x => x.MessageId);

        builder
            .Ignore(x => x.Request);
        
        builder
            .HasIndex([nameof(DiscordReaction.MessageId), nameof(DiscordReaction.EmoteName)])
            .IsUnique();
    }
}