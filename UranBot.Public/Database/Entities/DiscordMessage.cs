// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Public.Database.Entities;

public class DiscordMessage : BaseDiscordEntity
{
    public string? Content { get; set; }
    
    public long ChannelId { get; set; }
    
    public virtual DiscordChannel Channel { get; set; }
    
    public long? UserId { get; set; }
    
    public virtual DiscordUser? User { get; set; }
    
    public bool IsDeleted { get; set; }
}