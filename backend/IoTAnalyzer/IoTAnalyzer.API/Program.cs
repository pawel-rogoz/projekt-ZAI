using System.Text.Json.Serialization;
using Ardalis.SharedKernel;
using IoTAnalyzer.API.Configurations;
using IoTAnalyzer.Infrastructure;
using IoTAnalyzer.UseCases.Series.ListSeries;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Brak ConnectionStrings:Default (zmienna ConnectionStrings__Default).");
builder.Services.AddInfrastructureServices(connectionString);

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

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
