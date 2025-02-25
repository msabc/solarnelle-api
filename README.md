<h1 align="center">Solarnelle</h1>

Solarnelle is .NET 9 Web API created as part of a company challenge. 

## About

Solarnelle is used to track solar power plant data. 
Each solar power plant has **production** and **forecast** data. 

## Architecture

The API implements [**Domain-driven design**](https://en.wikipedia.org/wiki/Domain-driven_design)
and is divided into several projects (layers):

- Configuration
    - holds configuration classes used to implement the [options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-9.0)
- IoC
    - central place for the entire DI ([dependency inversion](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion)) setup
- Api
    - contains the main entry point
- Application
    - contains the business logic
- Domain
    - contains the domain models
- Infrastructure
    - contains the database context
- Tests
    - contains tests (only unit tests for now)

## Technology

The project features the following technologies:

- .NET 9 Web API
- EFCore 9 - Code first approach - for database access
- SQL Server
- Scalar - for Open API UI
- NLog - for logging
  - configure logging to your liking by modifying the **NLog.config** file using the instructions provided [**here**](https://nlog-project.org/config/)
- Moq
- xUnit

## Authorization and authentication
- Solarnelle uses ASP.NET Core Identity as the authentication provider
- Solarnelle uses JWT Bearer Authentication

## Run the project

> **_Note:_** Solarnelle will create a database if one doesn't exist based on the **SolarnelleConnectionString** setting.

1. Clone the project
2. Open the solution (**Solarnelle.Api.sln**) in Visual Studio
3. Set **Solarnelle.Api** as the Startup project
4. Run the project