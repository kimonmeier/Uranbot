namespace UranBot.Database.Configurations;

public class DiscordRoleConfiguration : IEntityTypeConfiguration<DiscordRole>
{
    public void Configure(EntityTypeBuilder<DiscordRole> builder)
    {
        builder
            .ToTable(nameof(DiscordRole), Const.Database.Schema.Discord);

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
            .HasIndex(x => x.DiscordId);
    }
}