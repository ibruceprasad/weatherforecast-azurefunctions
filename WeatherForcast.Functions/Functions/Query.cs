using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WeatherForcast.Entity;
using WeatherForcast.Middleware;
using WeatherForcast.Functions.Constants;

namespace WeatherForcast.Functions
{
    public class Query
    {
        private readonly ILogger<Query> _logger;
        
        public Query(ILogger<Query> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Query))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] 
            HttpRequest req,
            [SqlInput(Consts.WEATHER_QUERY ,Consts.WEATHER_DB_CONNECTIONSTRING, System.Data.CommandType.Text,  Consts.WEATHER_QUERY_PARAM)]
            dynamic inputResult)
        {
            IReadOnlyList<ForecastRegister> forecastRegster = JsonSerializer.Deserialize<IReadOnlyList<ForecastRegister>>(inputResult);
            var weatherData = forecastRegster.FirstOrDefault()?.Data;
            if (string.IsNullOrEmpty(weatherData))
            {
                var failureMessage = $"Query failed - Weather data for {req.Query.ToDictionary()["Place"]} is not found";
                _logger.LogError($"{failureMessage}");
                return new NotFoundObjectResult(failureMessage);
            }

            return new OkObjectResult(forecastRegster.FirstOrDefault());
        }
    }
} 