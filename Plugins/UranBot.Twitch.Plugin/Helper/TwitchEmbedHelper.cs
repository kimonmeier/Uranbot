using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Games;
using Game = TwitchLib.Api.Helix.Models.Games.Game;
using Stream = TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream;

namespace UranBot.Twitch.Plugin.Helper;

public static class TwitchEmbedHelper
{

    public static EmbedBuilder CreateClipEmbed(Clip clip)
    {
        EmbedBuilder embedBuilder = EmbedBuilderHelper.CreateDefaultEmbed();

        embedBuilder.Title = clip.Title;
        embedBuilder.ImageUrl = clip.ThumbnailUrl;
        embedBuilder.Url = clip.Url;
        embedBuilder.AddField("Views", clip.ViewCount);
        embedBuilder.AddField("Author", clip.CreatorName);
        embedBuilder.AddField("Link", clip.Url);
        
        return embedBuilder;
    }

    public async static Task<EmbedBuilder> CreateAnnouncement(TwitchAPI twitchApi, TwitchBroadcaster broadcaster, Stream stream)
    {
        EmbedBuilder embedBuilder = EmbedBuilderHelper.CreateDefaultEmbed();

        Game? game = (await twitchApi.Helix.Games.GetGamesAsync(new List<string>() { stream.GameId })).Games.Single();

        var url = $"https://twitch.tv/{broadcaster.BroadcasterName}";
        
        embedBuilder.WithTitle(stream.Title);
        embedBuilder.WithUrl(url);
        embedBuilder.WithImageUrl(stream.ThumbnailUrl.Replace("{width}", "1920").Replace("{height}", "1080"));
        embedBuilder.WithThumbnailUrl(game.BoxArtUrl.Replace("{width}", "450").Replace("{height}", "600"));
        embedBuilder.AddField("Game", game.Name);
        embedBuilder.AddField("Viewers", stream.ViewerCount);
        embedBuilder.AddField("Url", url);
        embedBuilder.WithColor(Color.Purple);
        
        return embedBuilder;
    }
}