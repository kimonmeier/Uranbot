namespace UranBot.Database.Configurations;

public sealed class DiscordUserConfiguration : IEntityTypeConfiguration<DiscordUser>
{
    public void Configure(EntityTypeBuilder<DiscordUser> builder)
    {
        builder
            .ToTable(nameof(DiscordUser), Const.Database.Schema.Discord);

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .HasIndex(x => x.DiscordId);
    }
}