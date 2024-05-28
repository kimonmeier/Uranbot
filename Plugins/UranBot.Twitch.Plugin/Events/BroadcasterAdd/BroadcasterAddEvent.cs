namespace UranBot.Twitch.Plugin.Events.BroadcasterAdd;

public class BroadcasterAddEvent : BaseCommandEvent
{
    public required string Name { get; init; }
}