using System.Reflection;
using ILogger = Serilog.ILogger;

namespace UranBot.Plugins;

public class PluginLoader
{
    private readonly static ILogger Logger = Log.ForContext<PluginLoader>();

    public static void RegisterPlugins(IServiceCollection serviceCollection)
    {
        string pluginPath = Path.Combine(Environment.CurrentDirectory, "Plugins");
        Logger.Information("Index all Plugins");

        if (!Directory.Exists(pluginPath))
        {
            Logger.Warning("The Folder \"Plugin\" doesn't exist, so no Plugins could be loaded");
            return;
        }
        
        List<Assembly> pluginAssemblies = new List<Assembly>();
        IEnumerable<string> pluginDirectories = Directory.EnumerateDirectories(pluginPath);

        foreach (string pluginDirectory in pluginDirectories)
        {
            pluginAssemblies.AddRange(LoadPluginAssemblies(pluginDirectory));
        }

        foreach (Assembly pluginAssembly in pluginAssemblies)
        {
            RegisterPluginOfAssembly(pluginAssembly, serviceCollection);
        }
        
        Logger.Information("All Plugins were registered in the service discovery");
    }

    private static IList<Assembly> LoadPluginAssemblies(string directory)
    {
        List<Assembly> assemblies = new List<Assembly>();
        
        Logger.Information("Found Plugin in Directory {0}", Path.GetDirectoryName(directory));

        var pluginFiles = Directory.GetFiles(directory, "*.Plugin.dll");

        Logger.Information("Found {0} Dlls to load", pluginFiles.Length);
        
        foreach (string pluginFile in pluginFiles)
        {
            try
            {
                assemblies.Add(Assembly.LoadFrom(pluginFile));
            }
            catch (Exception e)
            {
                Logger.Error(e, "While loading the Plugin {0} an exception occured", Path.GetDirectoryName(directory));
            }
        }

        return assemblies;
    }

    private static void RegisterPluginOfAssembly(Assembly assembly, IServiceCollection serivces)
    {
        IEnumerable<Type> serviceDiscoveries = assembly.GetTypes().Where(x => x.GetInterfaces().Any(z => z == typeof(IServiceDiscovery)));

        foreach (Type serviceDiscoveryType in serviceDiscoveries)
        {
            IServiceDiscovery serviceDiscovery = (IServiceDiscovery)Activator.CreateInstance(serviceDiscoveryType)!;

            serviceDiscovery.RegisterServices(serivces);
        }
    }
}