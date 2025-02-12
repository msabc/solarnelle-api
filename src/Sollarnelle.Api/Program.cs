using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Scalar.AspNetCore;
using Solarnelle.Api.Authorization;
using Solarnelle.Api.Filters;
using Solarnelle.Api.OpenAPI;
using Solarnelle.Application.Constants;
using Solarnelle.Configuration;
using Solarnelle.Infrastructure.DatabaseContext;
using Solarnelle.IoC;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Solarnelle is starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers(options =>
    {
        var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ApiExceptionFilterAttribute>>();
        options.Filters.Add(new ApiExceptionFilterAttribute(logger));
    });

    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
    });

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });

    builder.Services.RegisterApplicationDependencies(builder.Configuration);

    // Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        string? signingKey = builder.Configuration["SolarnelleSettings:IssuerSigningKey"];

        if (string.IsNullOrWhiteSpace(signingKey))
            throw new Exception($"Missing required application setting {nameof(SolarnelleSettings.IssuerSigningKey)}.");

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    // Authorization
    builder.Services.AddSingleton<IAuthorizationHandler, SolarnelleAuthorizationHandler>();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(SecurityPolicies.SolarnelleUserIdPolicyName, policy => policy.RequireClaim(SecurityClaims.SolarnelleClaimsPrincipalType));

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SolarnelleDbContext>();
        dbContext.Database.EnsureCreated();
    }

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Solanelle stopped due to an exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}