using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherForcast.Functions;

namespace WeatherForcast.Middleware
{
    public class ValidateRequestMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ValidateRequestMiddleware> _logger;

        public ValidateRequestMiddleware(ILogger<ValidateRequestMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            _logger.LogInformation("ValidateRequestMiddleware called");
            var request = await context.GetHttpRequestDataAsync();
            var value = context.BindingContext.BindingData["Place"]?.ToString() ?? "";
            value = value.Trim();
            if (string.IsNullOrEmpty(value) || value.Length >20 )
            {
                var response = request.CreateResponse();
                response.StatusCode = HttpStatusCode.BadRequest;
                string responseBody = JsonSerializer.Serialize("Validation failed - Invalid Query parameters");
                await response.WriteStringAsync(responseBody);
                //return;
            }
            await next(context);
        }
    }
}