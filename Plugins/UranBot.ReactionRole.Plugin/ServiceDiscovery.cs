using Microsoft.Extensions.DependencyInjection;
using UranBot.Public;
using UranBot.Public.Services;

namespace UranBot.ReactionRole.Plugin;

public class ServiceDiscovery : IServiceDiscovery
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IPlugin, ReactionRolePlugin>();
    }
}