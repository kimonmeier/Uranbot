// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchAnnouncement : BaseEntity
{
    public long MessageId { get; set; }

    public virtual DiscordMessage Message { get; set; }

    public long BroadcasterId { get; set; }

    public virtual TwitchBroadcaster Broadcaster { get; set; }
}