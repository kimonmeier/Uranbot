// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Twitch.Plugin.Database.Entities;

public class TwitchClipSettings : BaseEntity
{
    public long DiscordChannelId { get; set; }

    public virtual DiscordChannel DiscordChannel { get; set; }

    public long? ApprovalChannelId { get; set; }

    public virtual DiscordChannel? ApprovalChannel { get; set; }

    public long BroadcasterId { get; set; }

    public virtual TwitchBroadcaster Broadcaster { get; set; }

    public TwitchClipShareMode ShareMode { get; set; }

    public DateTime LastSynched { get; set; }
}