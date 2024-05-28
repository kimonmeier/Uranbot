using EFCoreSecondLevelCacheInterceptor;
using UranBot.Public.Database;

namespace UranBot.Public;

public static class ConfigureServices
{
    public static void AddDbContextWithProfile<T>(this IServiceCollection serviceCollection) where T : BaseUranDbContext
    {
        serviceCollection.AddDbContext<T>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
        });
    }
    
}