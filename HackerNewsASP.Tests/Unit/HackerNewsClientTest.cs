using HackerNewsASP.Models.Dtos;
using HackerNewsASP.RedisService;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;

namespace HackerNewsASP.Tests.Unit;

public class HackerNewsClientTest
{
    private Mock<IRedisService> _mockCache;
    private HttpClient _httpClient;
    private HackerNewsClient.HackerNewsClient _client;
    private Mock<HttpMessageHandler> _handlerMock;

    [SetUp]
    public void SetUp()
    {
        _mockCache = new Mock<IRedisService>();
        _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        _handlerMock.Protected()
            .Setup("Dispose", ItExpr.IsAny<bool>())
            .Verifiable();

        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("https://test.com")
        };

        _client = new HackerNewsClient.HackerNewsClient(_httpClient, _mockCache.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    [Test]
    public async Task GetBestStoriesIdsAsync_ReturnsCachedIds_WhenCacheIsNotEmpty()
    {
        // Arrange
        var cachedIds = new List<int> { 1, 2, 3 };
        _mockCache.Setup(x => x.GetAsync<List<int>>(It.IsAny<string>()))
            .ReturnsAsync(cachedIds);

        // Act
        var result = await _client.GetBestStoriesIdsAsync();

        // Assert
        Assert.That(result, !Is.Null);
        Assert.That(cachedIds, Is.EqualTo(result));
        _mockCache.Verify(x => x.GetAsync<List<int>>(It.IsAny<string>()), Times.Once);
        _mockCache.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetBestStoriesIdsAsync_FetchesIdsFromApiAndCachesThem_WhenCacheIsEmpty()
    {
        // Arrange
        _mockCache.Setup(x => x.GetAsync<List<int>>(It.IsAny<string>()))
            .ReturnsAsync((List<int>?)null);

        var expectedIds = new List<int> { 4, 5, 6 };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.PathAndQuery == "/beststories.json"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedIds)
            });

        // Act
        var result = await _client.GetBestStoriesIdsAsync();

        // Assert
        Assert.That(result, !Is.Null);
        Assert.That(expectedIds, Is.EqualTo(result));
        _mockCache.Verify(x => x.GetAsync<List<int>>(It.IsAny<string>()), Times.Once);
        _mockCache.Verify(x => x.SaveAsync(It.IsAny<string>(), expectedIds), Times.Once);
        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        _mockCache.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetStoryDetailsAsync_ReturnsCachedStory_WhenCacheIsNotEmpty()
    {
        // Arrange
        const int storyId = 123;

        var cachedStory = new StoryDto { Id = storyId, Title = "Cached Story" };
        _mockCache.Setup(x => x.GetAsync<StoryDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedStory);

        // Act
        var result = await _client.GetStoryDetailsAsync(storyId, CancellationToken.None);

        // Assert
        Assert.That(result, !Is.Null);
        Assert.That(cachedStory.Id, Is.EqualTo(result.Id));
        _mockCache.Verify(x => x.GetAsync<StoryDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockCache.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetStoryDetailsAsync_FetchesStoryFromApiAndCachesIt_WhenCacheIsEmpty()
    {
        // Arrange
        const int storyId = 123;
        _mockCache.Setup(x => x.GetAsync<StoryDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((StoryDto?)null);

        var expectedStory = new StoryDto { Id = storyId, Title = "Fetched Story" };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.PathAndQuery == $"/item/{storyId}.json"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedStory)
            });

        // Act
        var result = await _client.GetStoryDetailsAsync(storyId, CancellationToken.None);

        // Assert
        Assert.That(result, !Is.Null);
        Assert.That(expectedStory.Id, Is.EqualTo(result.Id));
        _mockCache.Verify(x => x.GetAsync<StoryDto>(It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _mockCache.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<StoryDto>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        _mockCache.VerifyNoOtherCalls();
    }
}
