using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.WebRequestMethods;
using WeatherForcast.Middleware;
 
var builder = FunctionsApplication.CreateBuilder(args);

builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

var weatherApi = Environment.GetEnvironmentVariable("WeatherApiBaseUrl");
builder.Services.AddHttpClient("weatherapi", config =>
{
    config.BaseAddress = new Uri(uriString: weatherApi!);
});

builder.ConfigureFunctionsWebApplication();

builder.UseWhen<ValidateRequestMiddleware>(context =>
{
    return context.FunctionDefinition.Name == "Query" || context.FunctionDefinition.Name == "Register";
});

var app = builder.Build();
 
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

app.Run();
