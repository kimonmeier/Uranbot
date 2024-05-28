using UranBot.Public.Events;

namespace UranBot.Public.Discord;

public static class InteractionExtensions
{
    public static Task SendFailureMessage(this BaseCommandEvent @event, string message)
    {
        EmbedBuilder embedBuilder = EmbedBuilderHelper.CreateFailureMessageBuilder(message);

        return @event.InteractionContext.Interaction.RespondAsync(embed: embedBuilder.Build());
    }

    public static Task SendSuccessMessage(this BaseCommandEvent @event, string message)
    {
        EmbedBuilder embedBuilder = EmbedBuilderHelper.CreateSuccessMessageBuilder(message);

        return @event.InteractionContext.Interaction.RespondAsync(embed: embedBuilder.Build());
    }
}