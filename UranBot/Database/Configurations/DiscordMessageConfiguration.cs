namespace UranBot.Database.Configurations;

public sealed class DiscordMessageConfiguration : IEntityTypeConfiguration<DiscordMessage>
{
    public void Configure(EntityTypeBuilder<DiscordMessage> builder)
    {
        builder
            .ToTable(nameof(DiscordMessage), Const.Database.Schema.Discord);

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Content)
            .HasMaxLength(25_555);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.Channel)
            .WithMany()
            .HasForeignKey(x => x.ChannelId);
        
        builder
            .HasIndex(x => x.DiscordId);
        
        builder
            .HasIndex(x => x.UserId);
    }
}