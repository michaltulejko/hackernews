using HackerNewsASP.Models.Dtos;
using HackerNewsASP.Tests.Models;
using System.Text.Json;

namespace HackerNewsASP.Tests.Integration;

public class HackerNewsIntegrationTest
{
    private IDistributedApplicationTestingBuilder _appHost;

    [SetUp]
    public async Task SetUp()
    {
        _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HackerNewsASP>();
        _appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    }

    [Test]
    public async Task GetBestStories_ReturnsCorrectResult_WhenRequestIsValid()
    {
        // Arrange
        await using var app = await _appHost.BuildAsync();
        var notificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("HackerNewsASP");
        await notificationService.WaitForResourceAsync("HackerNewsASP", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync("/news/best/10");
        var stories = JsonSerializer.Deserialize<List<StoryDto>>(await response.Content.ReadAsStreamAsync());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(stories.Count, Is.EqualTo(10));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-2)]
    public async Task GetBestStories_ReturnsValidationError_WhenRequestIsInvalid(int number)
    {
        // Arrange
        await using var app = await _appHost.BuildAsync();
        var notificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("HackerNewsASP");
        await notificationService.WaitForResourceAsync("HackerNewsASP", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync($"/news/best/{number}");
        var validationErrors =
            JsonSerializer.Deserialize<List<ValidationError>>(await response.Content.ReadAsStreamAsync());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(validationErrors?.FirstOrDefault()?.ErrorMessage, Is.EqualTo("The minimal number of stories is 1"));
    }
}