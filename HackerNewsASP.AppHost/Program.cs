var builder = DistributedApplication.CreateBuilder(args);

var redisCache = builder.AddRedis("cache")
    .WithImageTag("latest");

builder.AddProject<Projects.HackerNewsASP>("HackerNewsASP")
    .WithReference(redisCache)
    .WaitFor(redisCache);


builder.Build().Run();