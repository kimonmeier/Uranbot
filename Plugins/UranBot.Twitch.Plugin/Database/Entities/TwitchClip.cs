// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchClip : BaseEntity
{
    public long DiscordMessageId { get; set; }

    public virtual DiscordMessage DiscordMessage { get; set; }

    public string ClipId { get; set; }

    public long BroadcasterId { get; set; }

    public virtual TwitchBroadcaster Broadcaster { get; set; }

    public DateTime PostedAt { get; set; }
}