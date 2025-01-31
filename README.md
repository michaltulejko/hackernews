# HackerNewsASP – .NET 9 and .NET Aspire

This application is orchestrated with **.NET 9** and **.NET Aspire**.

## Prerequisites

- **Docker** installed and running (required for Redis).

## Getting Started

1. **Clone or download** the repository.
2. **Ensure Docker is running** so Redis can be pulled and launched if needed.

## Running the Application

From the main directory, run:

```bash
dotnet run --project .\HackerNewsASP.AppHost\
```

This launches the **HackerNewsASP.AppHost** project.

## Running Tests

```bash
dotnet test
```

## Additional Info

- **Aspire Dashboard**  
  Once the application is running, the Aspire dashboard is available at  
  `https://localhost:17258`  
  Use the exact URL from your console output—it includes a **login token**.

- **Swagger**  
  Available at:  
  `https://localhost:7151/swagger/index.html`  
  No authorization is required to access Swagger.


## Assumptions
- Api does not need to have fresh results every time it is called
- There is no need for a fully fledge controller
- User must request at least 1 article

## Given time
- Reconsider access to best articles, perhaps with pooling
- Better error handling
- More tests
- Add K6 for load testing
