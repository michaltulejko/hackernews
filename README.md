Application running on .NET 9 with .Net Aspire for orchestration
Before you start the app, make sure you have running docker on your machine, for Redis.

To start the app run commands from the main directory
Running application: `dotnet run --project .\HackerNewsASP.AppHost\`
Running tests: `dotnet test`
	
Aspire Dashboard will be available on `https://localhost:17258` remember to use the URL from the console as there is a login token.
Swagger is available on `https://localhost:7151/swagger/index.html` without authorization.
