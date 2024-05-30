using Discord;
using Discord.Interactions;
using Serilog.Events;
using UranBot.Configuration;
using UranBot.Database;
using UranBot.EventHandler.ReactionAdded;
using UranBot.EventHandler.SyncDatabase;
using UranBot.Plugins;
using ILogger = Serilog.ILogger;

namespace UranBot;

public class BotManager
{
    private readonly TaskManager _taskManager;
    private readonly PluginManager _pluginManager;
    private readonly IConfiguration _configuration;
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly IServiceProvider _serviceProvider;

    public BotManager(TaskManager taskManager, PluginManager pluginManager, IConfiguration configuration, DiscordSocketClient discordSocketClient, IServiceProvider serviceProvider)
    {
        _taskManager = taskManager;
        _pluginManager = pluginManager;
        _configuration = configuration;
        _discordSocketClient = discordSocketClient;
        _serviceProvider = serviceProvider;
    }

    public async Task StartBot()
    {
        DiscordConfiguration discordConfiguration = _configuration.GetSection("Discord")?.Get<DiscordConfiguration>() ?? throw new ArgumentNullException();

        _discordSocketClient.Log += message =>
        {
            ILogger logger = Log.ForContext<DiscordSocketClient>();
            LogEventLevel logLevel;


            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    logLevel = LogEventLevel.Fatal;

                    break;
                case LogSeverity.Error:
                    logLevel = LogEventLevel.Error;

                    break;
                case LogSeverity.Warning:
                    logLevel = LogEventLevel.Warning;

                    break;
                case LogSeverity.Info:
                    logLevel = LogEventLevel.Information;

                    break;
                case LogSeverity.Debug:
                    logLevel = LogEventLevel.Debug;

                    break;

                case LogSeverity.Verbose:
                default:
                    logLevel = LogEventLevel.Verbose;

                    break;
            }


            logger.Write(logLevel, message.Exception, message.Message);

            return Task.CompletedTask;
        };

        await _discordSocketClient.LoginAsync(TokenType.Bot, discordConfiguration.Token);
        await _discordSocketClient.StartAsync();

        Guid synchroniseDatabaseTaskId = Guid.Empty;
        _discordSocketClient.Ready += async () =>
        {
            await _serviceProvider.GetRequiredService<InteractionService>().RegisterCommandsGloballyAsync();
            _taskManager.SetInitialized();

            synchroniseDatabaseTaskId = _taskManager.RegisterTask("Synchronise Database", x => x.GetRequiredService<ISender>().Send(new SyncDatabaseEvent()), TimeSpan.FromMinutes(15));
        };
        _discordSocketClient.JoinedGuild += _ =>
        {
            _taskManager.StartTask(synchroniseDatabaseTaskId);

            return Task.CompletedTask;
        };
        _discordSocketClient.LeftGuild += _ =>
        {
            _taskManager.StartTask(synchroniseDatabaseTaskId);

            return Task.CompletedTask;
        };
        _discordSocketClient.UserJoined += _ =>
        {
            _taskManager.StartTask(synchroniseDatabaseTaskId);

            return Task.CompletedTask;
        };
        _discordSocketClient.UserLeft += (_, _) =>
        {
            _taskManager.StartTask(synchroniseDatabaseTaskId);

            return Task.CompletedTask;
        };
        _discordSocketClient.InteractionCreated += (x) =>
        {
            _serviceProvider.GetRequiredService<InteractionService>().ExecuteCommandAsync(new SocketInteractionContext(_discordSocketClient, x), _serviceProvider)
                .ConfigureAwait(false);

            return Task.CompletedTask;
        };
        _discordSocketClient.ReactionAdded += DiscordSocketClientOnReactionAdded;

        _pluginManager.StartPlugins();
        _taskManager.StartTaskManager();
    }

    private async Task DiscordSocketClientOnReactionAdded(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        if (_discordSocketClient.GetUser(reaction.UserId).IsBot)
        {
            return;
        }

        using IServiceScope scope = _serviceProvider.CreateScope();
        DiscordReaction? discordReaction = scope.ServiceProvider.GetRequiredService<CoreUranDbContext>().Set<DiscordReaction>()
            .SingleOrDefault(x => x.Message.DiscordId == message.Id && x.EmoteName == reaction.Emote.Name);

        if (discordReaction is null)
        {
            return;
        }

        await scope.ServiceProvider.GetRequiredService<ISender>().Send(new ReactionTriggeredEvent()
        {
            Reaction = discordReaction
        });
    }

    public async Task StopBot()
    {
        _pluginManager.StopPlugins();
        _taskManager.Dispose();

        await _discordSocketClient?.StopAsync()!;
    }
}