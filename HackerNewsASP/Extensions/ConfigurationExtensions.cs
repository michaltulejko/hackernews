using HackerNewsASP.Models.Configuration;

namespace HackerNewsASP.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureSettings(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<HackerNewsConfig>(builder.Configuration.GetSection("HackerNews"));
    }
}