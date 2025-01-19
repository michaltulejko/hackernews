using HackerNewsASP.Models.Dtos;

namespace HackerNewsASP.HackerNewsService;

public interface IHackerNewsService
{
    public Task<List<StoryDto>> GetBestStoriesAsync(int numberOfStories);

}