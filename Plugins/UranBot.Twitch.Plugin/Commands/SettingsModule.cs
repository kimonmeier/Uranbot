using UranBot.Twitch.Plugin.Events.ClipDisable;
using UranBot.Twitch.Plugin.Events.ClipEnable;
using UranBot.Twitch.Plugin.Events.ClipSharedModeChange;

namespace UranBot.Twitch.Plugin.Commands;

[Group("settings", "Settings")]
[RequireUserPermission(GuildPermission.Administrator)]
public class SettingsModule : InteractionModuleBase
{
    [Group("broadcaster", "Settings for broadcasters")]
    public class BroadcastSettingsModule : InteractionModuleBase
    {
        private readonly IUranInteractionService _uranInteractionService;

        public BroadcastSettingsModule(IUranInteractionService uranInteractionService)
        {
            _uranInteractionService = uranInteractionService;
        }

        [SlashCommand("add", "Adds a new Broadcaster")]
        public Task Add(string name)
            => _uranInteractionService.ExecuteInteraction(new BroadcasterAddEvent()
            {
                InteractionContext = Context, Name = name
            });


        [SlashCommand("remove", "Removes a new Broadcaster")]
        public Task Delete(string name)
            => _uranInteractionService.ExecuteInteraction(new BroadcasterRemoveEvent()
            {
                InteractionContext = Context, Name = name
            });
    }

    [Group("clips", "Settings for clips")]
    public class ClipSettingsModule : InteractionModuleBase
    {
        private readonly IUranInteractionService _uranInteractionService;

        public ClipSettingsModule(IUranInteractionService uranInteractionService)
        {
            _uranInteractionService = uranInteractionService;
        }

        [SlashCommand("enable", "Enables the clip handling for a broadcaster")]
        public Task Enable(string broadcasterName, ISocketMessageChannel messageChannel)
            => _uranInteractionService.ExecuteInteraction(new ClipEnableEvent()
            {
                Channel = messageChannel, BroadcasterName = broadcasterName, InteractionContext = Context
            });

        [SlashCommand("mode", "Changes the mode of the clip handling")]
        public Task SetMode(string broadcasterName, TwitchClipShareMode shareMode, ISocketMessageChannel? approvalChannel = null)
            => _uranInteractionService.ExecuteInteraction(new ClipSharedModeChangeEvent()
            {
                BroadcasterName = broadcasterName, ShareMode = shareMode, ApprovalChannel = approvalChannel, InteractionContext = Context
            });

        [SlashCommand("disable", "Disables the clip handling for a broadcaster")]
        public Task Disable(string broadcasterName)
            => _uranInteractionService.ExecuteInteraction(new ClipDisableEvent()
            {
                BroadcasterName = broadcasterName, InteractionContext = Context
            });
    }
}