using HackerNewsASP.HackerNewsClient;
using HackerNewsASP.Models.Configuration;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using System.Threading.RateLimiting;

namespace HackerNewsASP.Extensions;

public static class HttpClientsExtensions
{
    public static void ConfigureHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IHackerNewsClient, HackerNewsClient.HackerNewsClient>("HackerNewsClient",
                (serviceProvider, client) =>
                {
                    var settings = serviceProvider.GetRequiredService<IOptions<HackerNewsConfig>>().Value;

                    client.BaseAddress = new Uri(settings.BaseUrl);
                })
            .AddResilienceHandler("hacker-pipeline", builder =>
            {
                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 5,
                    Delay = TimeSpan.FromMilliseconds(200),
                    BackoffType = DelayBackoffType.Exponential
                });

                builder.AddTimeout(TimeSpan.FromSeconds(15));

                builder.AddRateLimiter(new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 1000,
                    SegmentsPerWindow = 5,
                    Window = TimeSpan.FromSeconds(1)
                }));
            });
    }
}