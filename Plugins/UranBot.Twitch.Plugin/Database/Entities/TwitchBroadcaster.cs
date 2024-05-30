// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchBroadcaster : BaseEntity
{
    public string BroadcasterName { get; set; }
    
    public string TwitchId { get; set; }
    
    public long GuildId { get; set; }
    
    public virtual DiscordGuild Guild { get; set; }
}