using HackerNewsASP.Models.Dtos;
using HackerNewsASP.RedisService;
using System.Net.Http.Json;

namespace HackerNewsASP.HackerNewsClient;

public class HackerNewsClient(HttpClient httpClient, IRedisService cache) : IHackerNewsClient
{
    private const string BEST_STORIES_KEY = "bestStoriesKey";
    private static string STORY_DETAILS_KEY(int storyId) => $"storyDetails_{storyId}";

    public async Task<List<int>?> GetBestStoriesIdsAsync()
    {
        var cachedResult = await cache.GetAsync<List<int>>(BEST_STORIES_KEY);
        if (cachedResult is { Count: > 0 })
            return cachedResult;

        var result = await httpClient.GetFromJsonAsync<List<int>>("beststories.json");
        if (result is not null)
        {
            await cache.SaveAsync(BEST_STORIES_KEY, result);
        }

        return result;
    }

    public async Task<StoryDto?> GetStoryDetailsAsync(int storyId, CancellationToken token)
    {
        var key = STORY_DETAILS_KEY(storyId);

        var cachedResult = await cache.GetAsync<StoryDto>(key, token);
        if (cachedResult is not null)
            return cachedResult;

        var result = await httpClient.GetFromJsonAsync<StoryDto>($"item/{storyId}.json", token);
        if (result is not null)
        {
            await cache.SaveAsync(key, result, token);
        }

        return result;
    }
}