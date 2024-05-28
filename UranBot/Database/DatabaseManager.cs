namespace UranBot.Database;

public class DatabaseManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseManager> _logger;
    private readonly List<Type> _dbTypes;

    public DatabaseManager(IServiceProvider serviceProvider, ILogger<DatabaseManager> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _dbTypes = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("UranBot") ?? false)
            .SelectMany(asmb => asmb.GetExportedTypes().Where(type => type.BaseType == typeof(BaseUranDbContext))).ToList();
    }

    public void ExecuteMigrations()
    {
        foreach (Type type in _dbTypes)
        {
            BaseUranDbContext baseDbContext = (BaseUranDbContext)_serviceProvider.GetRequiredService(type);
            
            _logger.LogInformation("Migrating Database for Context {0}", baseDbContext.GetType().Name); 
            
            baseDbContext.Database.Migrate();
        }
    }
}