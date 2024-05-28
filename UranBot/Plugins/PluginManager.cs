using System.Reflection;
using Discord.Interactions;

namespace UranBot.Plugins;

public class PluginManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly InteractionService _interactionService;
    private readonly ILogger<PluginManager> _logger;
    
    public PluginManager(IServiceProvider serviceProvider, ILogger<PluginManager> logger, InteractionService interactionService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _interactionService = interactionService;
    }

    public void StartPlugins()
    {
        IEnumerable<IPlugin> plugins = _serviceProvider.GetServices<IPlugin>();

        foreach (IPlugin plugin in plugins)
        {
            _logger.LogInformation("Starting {0}", plugin.Name);
            
            plugin.Start();
            
            _logger.LogInformation("Started {0}", plugin.Name);
        }

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("UranBot") ?? false))
        {
            _interactionService.AddModulesAsync(assembly, _serviceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    public void StopPlugins()
    {
        IEnumerable<IPlugin> plugins = _serviceProvider.GetServices<IPlugin>();

        foreach (IPlugin plugin in plugins)
        {
            _logger.LogInformation("Stopping {0}", plugin.Name);
            
            plugin.Stop();
            
            _logger.LogInformation("Stopped {0}", plugin.Name);
        }
    }

}