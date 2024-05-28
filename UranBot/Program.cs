using Discord;
using Discord.Commands;
using Discord.Interactions;
using EFCoreSecondLevelCacheInterceptor;
using MediatR.Pipeline;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using UranBot;
using UranBot.Database;
using UranBot.EventHandler;
using UranBot.Plugins;
using UranBot.Public.Background;

ManualResetEvent exitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    exitEvent.Set();
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddJsonFile("appsettings.json", optional: true);
    })
    .UseSerilog()
    .ConfigureServices(services =>
    {

        #region Database

        services.AddDbContextWithProfile<CoreUranDbContext>();
        services.AddEFSecondLevelCache(options =>
        {
            services.AddSingleton<DatabaseManager>();
            options.UseMemoryCacheProvider().ConfigureLogging(true).UseCacheKeyPrefix("CACHE_");
            options.CacheAllQueries(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(30));
            // Fallback on db if the caching provider fails.
            options.UseDbCallsIfCachingProviderIsDown(TimeSpan.FromMinutes(1));
        });

        #endregion

        #region Plugins

        PluginLoader.RegisterPlugins(services);
        services.AddSingleton<PluginManager>();

        #endregion

        #region Mediatr

        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionLoggingHandler<,,>));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        #endregion

        #region Discord

        services.AddSingleton<DiscordSocketConfig>(op => new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers
        });
        services.AddSingleton<InteractionService>(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>().Rest));
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<BotManager>();

        #endregion

        #region Task-Manager

        services.AddSingleton<TaskManager>();
        services.AddSingleton<ITaskManager>(x => x.GetRequiredService<TaskManager>());

        services.AddSingleton<UranInteractionService>();
        services.AddSingleton<IUranInteractionService>(x => x.GetRequiredService<UranInteractionService>());

        services.AddSingleton<ReactionService>();
        services.AddSingleton<IReactionService>(x => x.GetRequiredService<ReactionService>());

        services.AddSingleton<DiscordService>();
        services.AddSingleton<IDiscordService>(x => x.GetRequiredService<DiscordService>());

        #endregion

    })
    .Build();

try
{
    Log.ForContext<Program>().Debug("Starting Database with Migrations");
    host.Services.GetRequiredService<DatabaseManager>().ExecuteMigrations();

    BotManager botManager = host.Services.GetRequiredService<BotManager>();

    await botManager.StartBot();

    exitEvent.WaitOne();

    await botManager.StopBot();
}
catch (Exception e)
{
    Log.Fatal(e, "During the application Loop an exception occured");
}

Log.CloseAndFlush();