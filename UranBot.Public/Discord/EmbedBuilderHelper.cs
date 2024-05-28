namespace UranBot.Public.Discord;

public static class EmbedBuilderHelper
{
    public static EmbedBuilder CreateDefaultEmbed()
    {
        return new EmbedBuilder().ApplyDefault();
    }


    internal static EmbedBuilder CreateFailureMessageBuilder(string message)
        => CreateDefaultEmbed()
            .WithColor(Color.Red)
            .WithTitle("Command failed")
            .WithDescription(message);
    
    
    internal static EmbedBuilder CreateSuccessMessageBuilder(string message)
        => CreateDefaultEmbed()
            .WithColor(Color.Green)
            .WithTitle("Command succeeded")
            .WithDescription(message);


    public static EmbedBuilder ApplyDefault(this EmbedBuilder builder)
    {
        builder.Author = new EmbedAuthorBuilder()
        {
            Name = "Uranbot", Url = "https://k-meier.ch"
        };

        builder.Color = Color.Purple;
        builder.Footer = new EmbedFooterBuilder()
        {
            Text = "Bot developed with Heart by LuckyDev"
        };

        return builder;
    }
}