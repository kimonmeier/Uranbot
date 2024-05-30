// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Public.Database.Entities;

public class DiscordGuild : BaseDiscordEntity
{
    public long? OwnerId { get; set; }
    
    public virtual DiscordUser? Owner { get; set; }
}