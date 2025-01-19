using HackerNewsASP.HackerNewsClient;
using HackerNewsASP.Models.Dtos;
using HackerNewsASP.Models.Exceptions;
using Polly;
using System.Collections.Concurrent;
using System.Net;

namespace HackerNewsASP.HackerNewsService;

public class HackerNewsService(IHackerNewsClient client) : IHackerNewsService
{
    public async Task<List<StoryDto>> GetBestStoriesAsync(int numberOfStories)
    {
        var bestStories = await client.GetBestStoriesIdsAsync();
        var stories = new ConcurrentBag<StoryDto>();

        if (bestStories is not null)
            await Parallel.ForEachAsync(bestStories, async (storyId, token) =>
            {
                try
                {
                    var storyDetails = await client.GetStoryDetailsAsync(storyId, token);
                    if (storyDetails is not null)
                        stories.Add(storyDetails);
                }
                catch (ExecutionRejectedException ex)
                {
                    throw new HandledException(ex.Message, HttpStatusCode.TooManyRequests);
                }

            });

        return stories.OrderByDescending(x => x.Score).Take(numberOfStories).ToList();
    }
}