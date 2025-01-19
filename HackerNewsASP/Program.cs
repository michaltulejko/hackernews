using FluentValidation;
using HackerNewsASP.Extensions;
using HackerNewsASP.HackerNewsService;
using HackerNewsASP.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureSettings();

builder.AddServiceDefaults();

builder.AddRedisDistributedCache("cache");

builder.Services.ConfigureServices();

builder.Services.ConfigureHttpClients();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<StoriesRequestValidator>();


var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.ConfigureMiddleware();
app.MapGet("/news/best/{number:int}",
    async (int number, IValidator<StoriesRequest> validator, IHackerNewsService service) =>
    {
        var request = new StoriesRequest(number);

        var validationResult = await validator.ValidateAsync(request);

        return !validationResult.IsValid
            ? Results.BadRequest(validationResult.Errors)
            : Results.Ok((object?)await service.GetBestStoriesAsync(number));
    });

app.Run();