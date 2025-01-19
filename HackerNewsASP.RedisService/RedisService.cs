using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HackerNewsASP.RedisService;

public class RedisService(IDistributedCache cache) : IRedisService
{
    private static readonly DistributedCacheEntryOptions Options = new DistributedCacheEntryOptions
    {
        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
    };

    public async Task SaveAsync<T>(string key, T obj) where T : class
    {
        var value = JsonSerializer.SerializeToUtf8Bytes(obj);

        await cache.SetAsync(key, value, Options);
    }
    public async Task SaveAsync<T>(string key, T obj, CancellationToken token) where T : class
    {
        var value = JsonSerializer.SerializeToUtf8Bytes(obj);

        await cache.SetAsync(key, value, Options, token);
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var cachedResult = await cache.GetAsync(key);
        if (cachedResult is null) return null;

        return JsonSerializer.Deserialize<T>(cachedResult);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token) where T : class
    {
        var cachedResult = await cache.GetAsync(key, token);
        if (cachedResult is null) return null;

        return JsonSerializer.Deserialize<T>(cachedResult);
    }
}