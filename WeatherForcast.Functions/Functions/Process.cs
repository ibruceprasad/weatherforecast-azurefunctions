using System;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using WeatherForcast.Entity;
using WeatherForcast.Functions;
using WeatherForcast.Functions.Helpers;

namespace WeatherForcast
{
    public class Process
    {
        private readonly ILogger _logger;
        private HttpClient httpClient;
        private const string SqlInputQuery = @"SELECT Id, Place, Data from dbo.Register";


        public Process(ILoggerFactory loggerFactory, IHttpClientFactory factory)
        {
            _logger = loggerFactory.CreateLogger<Process>();
            httpClient = factory.CreateClient("weatherapi");
        }

        [Function(nameof(Process))]
        [SqlOutput("dbo.Register", connectionStringSetting: "WeatherDbConnectionString")]
        public async Task<IList<ForecastRegister>> Run([TimerTrigger("0 */1 * * * *")] TimerInfo timer,
             [SqlInput(SqlInputQuery, "WeatherDbConnectionString")] IReadOnlyList<ForecastRegister> inputResult)
        {
            _logger.LogInformation($"Function: {nameof(Process)} triggered at: {DateTime.Now}");

            List<ForecastRegister> outputResult = new();

            foreach (ForecastRegister register in inputResult)
            {
                if (string.IsNullOrEmpty(register.Data))
                {
                    var url = FunctionsHelpers.GetWetherApiUrl(httpClient.BaseAddress.ToString(), register.Place);
                    var response = await httpClient.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    register.Data = result;
                    outputResult.Add(register);
                }
            }
            _logger.LogInformation($"Function {nameof(Process)} finished at: {DateTime.Now}");

            var scheduleMessage = timer.ScheduleStatus is not null ? 
                $"Function: {nameof(Process)} next schedule at: {timer.ScheduleStatus.Next}":
                $"Function: {nameof(Process)} next schedule at: Unknown";
                _logger.LogInformation(scheduleMessage);

            return outputResult;
        }


    }
}
