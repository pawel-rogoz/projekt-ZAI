using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Ardalis.SharedKernel;
using IoTAnalyzer.API.Authentication;
using IoTAnalyzer.API.Configurations;
using IoTAnalyzer.API.Controllers.Auth;
using IoTAnalyzer.Infrastructure;
using IoTAnalyzer.Infrastructure.Persistance.Seed;
using IoTAnalyzer.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using IoTAnalyzer.UseCases.Series.ListSeries;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Brak ConnectionStrings:Default (zmienna ConnectionStrings__Default).");
builder.Services.AddInfrastructureServices(connectionString);
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection(AuthOptions.SectionName));

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.Assemblies = [typeof(ListSeriesQuery), typeof(InfrastructureServiceExtensions)];
    options.PipelineBehaviors = [typeof(LoggingBehavior<,>)];
});

builder.Services
    .AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
        options.AddApiResultConvention();
    })
    .AddJsonOptions(options => options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict);

builder.Services
    .AddAuthentication(AccessTokenAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, AccessTokenAuthenticationHandler>(AccessTokenAuthenticationHandler.SchemeName, null);
builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy(AuthController.LoginRateLimitPolicy, context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) }));
});

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

await app.Services.SeedAdminUserAsync();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

app.Run();
