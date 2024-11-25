using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.WebRequestMethods;
using WeatherForcast.Middleware;
 

var builder = FunctionsApplication.CreateBuilder(args);
builder.Services.AddHttpClient("weatherapi", config =>
{
    config.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
});

builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
builder.ConfigureFunctionsWebApplication();


builder.UseWhen<ValidateRequestMiddleware>(context =>
{
    return context.FunctionDefinition.Name == "Query" || context.FunctionDefinition.Name == "Register";
});









//.UseMiddleware<GlobalExceptionHandlerMiddleware>()
//    .UseMiddleware<ValidateRequestMiddleware>();

//(context =>
//{
//    //return context.FunctionDefinition.Name == "Register" || 
//    return context.FunctionDefinition.Name == "Query";
//})



var app = builder.Build();
 
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

app.Run();
