namespace UranBot.Database.Configurations;

public sealed class DiscordGuildConfiguration : IEntityTypeConfiguration<DiscordGuild>
{
    public void Configure(EntityTypeBuilder<DiscordGuild> builder)
    {
        builder
            .ToTable(nameof(DiscordGuild), Const.Database.Schema.Discord);

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId);
        
        builder
            .HasIndex(x => x.DiscordId);
    }
}