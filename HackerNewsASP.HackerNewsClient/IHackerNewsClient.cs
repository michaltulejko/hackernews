using HackerNewsASP.Models.Dtos;

namespace HackerNewsASP.HackerNewsClient;

public interface IHackerNewsClient
{
    public Task<List<int>?> GetBestStoriesIdsAsync();
    public Task<StoryDto?> GetStoryDetailsAsync(int storyId, CancellationToken token);
}