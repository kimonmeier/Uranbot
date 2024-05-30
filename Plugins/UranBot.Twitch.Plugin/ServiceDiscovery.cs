using EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api.Core.Enums;
using UranBot.Public;
using UranBot.Public.Services;
using UranBot.Twitch.Plugin.Configuration;
using UranBot.Twitch.Plugin.Database;
using UranBot.Twitch.Plugin.Tasks;

namespace UranBot.Twitch.Plugin;

public class ServiceDiscovery : IServiceDiscovery
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContextWithProfile<TwitchUranDbContext>();
        services.AddSingleton<TwitchAPI>(x =>
        {

            TwitchConfiguration twitchConfiguration = x.GetRequiredService<IConfiguration>().GetSection("Twitch").Get<TwitchConfiguration>() ?? throw new ArgumentNullException("Twitch config missing");

            return new TwitchAPI()
            {
                Settings =
                {
                    ClientId = twitchConfiguration.ClientId, AccessToken = twitchConfiguration.AccessToken, Scopes = new List<AuthScopes>()
                    {
                        AuthScopes.Any
                    }
                }
            };
        });

        services.AddTransient<ClipTask>();
        services.AddTransient<AnnouncementTask>();

        services.AddSingleton<IPlugin, TwitchPlugin>();
    }
}