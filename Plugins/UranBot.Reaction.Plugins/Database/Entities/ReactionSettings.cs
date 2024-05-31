// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Reaction.Plugins.Database.Entities;

public class ReactionSettings : BaseEntity
{
    public long MessageId { get; set; }
    
    public virtual DiscordMessage Message { get; set; }
    
    public string EmoteName { get; set; }
    
    public long RoleId { get; set; }
    
    public virtual DiscordRole Role { get; set; }
    
}