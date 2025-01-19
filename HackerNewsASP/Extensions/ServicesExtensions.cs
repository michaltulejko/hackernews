using HackerNewsASP.HackerNewsClient;
using HackerNewsASP.HackerNewsService;
using HackerNewsASP.RedisService;

namespace HackerNewsASP.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IRedisService, RedisService.RedisService>();
        services.AddScoped<IHackerNewsClient, HackerNewsClient.HackerNewsClient>();
        services.AddScoped<IHackerNewsService, HackerNewsService.HackerNewsService>();
    }
}