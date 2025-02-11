using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Solarnelle.Api.OpenAPI;
using Solarnelle.Application.Constants;
using Solarnelle.Application.Security;
using Solarnelle.Configuration;
using Solarnelle.Infrastructure.DatabaseContext;
using Solarnelle.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
    string? securityToken = builder.Configuration["SolarnelleSettings:SecurityToken"];

    if (string.IsNullOrWhiteSpace(securityToken))
        throw new Exception($"Missing required application setting {nameof(SolarnelleSettings.SecurityToken)}.");

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityToken)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Authorization
builder.Services.AddSingleton<IAuthorizationHandler, SolarnelleAuthorizationHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(SecurityPolicies.SolarnelleUserIdPolicyName, policy => policy.RequireClaim(SecurityClaims.SolarnelleClaimsPrincipalType));

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
