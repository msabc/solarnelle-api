<h1 align="center">Solarnelle</h1>

Solarnelle is .NET 9 Web API created as part of a company challenge.

## About

The API implements [**Domain-driven design**](https://en.wikipedia.org/wiki/Domain-driven_design):

It is devided is several key layers: 
- Configuration
    - holds configuration classes used to implement the [options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-9.0)
- IoC
    - the central place for the entire DI setup
- Api
    - the main entry point
- Application
    - the business logic
- Domain
    - contains the domain models
- Infrastructure
    - contains the database context
- Tests
    - contains tests (only unit tests for now)
##
The project features the following technologies:
- .NET 9 Web API
- SQL Server
- Moq
- xUnit
- Scalar - for Open API UI
- NLog - for logging

## Run the project

1. Clone the project
2. Open the solution (**Solarnelle.Api.sln**) in Visual Studio
3. Set **Solarnelle.Api** as the Startup project
4. Run the project
