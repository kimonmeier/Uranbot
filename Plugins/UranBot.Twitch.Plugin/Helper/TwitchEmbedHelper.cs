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
    
}