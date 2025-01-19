namespace HackerNewsASP.RedisService;

public interface IRedisService
{
    public Task SaveAsync<T>(string key, T obj) where T : class;
    public Task SaveAsync<T>(string key, T obj, CancellationToken token) where T : class;
    public Task<T?> GetAsync<T>(string key) where T : class;
    public Task<T?> GetAsync<T>(string key, CancellationToken token) where T : class;

}