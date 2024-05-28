using UranBot.Twitch.Plugin.Events.DeleteClip;
using UranBot.Twitch.Plugin.Events.PostClipManual;

namespace UranBot.Twitch.Plugin.Commands;

[Group("clips", "Commands for the Clips module")]
[RequireUserPermission(GuildPermission.Administrator)]
public class ClipsModule : InteractionModuleBase
{
    private readonly IUranInteractionService _uranInteractionService;

    public ClipsModule(IUranInteractionService uranInteractionService)
    {
        _uranInteractionService = uranInteractionService;
    }

    [SlashCommand("post", "Post a clip to the clip channel")]
    public Task PostClip(string twitchClipUrl)
        => _uranInteractionService.ExecuteInteraction(new PostClipManualEvent()
        {
            ClipUrl = twitchClipUrl, InteractionContext = Context
        });
    
    [SlashCommand("delete", "Deletes a clip posted in the clip channel")]
    public Task DeleteClip(string twitchClipUrl)
        => _uranInteractionService.ExecuteInteraction(new DeleteClipEvent()
        {
            ClipUrl = twitchClipUrl, InteractionContext = Context
        });
}