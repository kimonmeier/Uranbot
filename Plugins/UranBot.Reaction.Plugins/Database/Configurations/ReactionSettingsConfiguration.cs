using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UranBot.Reaction.Plugins.Database.Entities;

namespace UranBot.Reaction.Plugins.Database.Configurations;

public class ReactionSettingsConfiguration : IEntityTypeConfiguration<ReactionSettings>
{
    public void Configure(EntityTypeBuilder<ReactionSettings> builder)
    {
        builder
            .ToTable(nameof(ReactionSettings), Const.Database.Schema.Reaction);

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
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId);

        builder
            .HasIndex([nameof(ReactionSettings.MessageId), nameof(ReactionSettings.EmoteName)])
            .IsUnique();
    }
}