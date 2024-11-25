using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Azure;
using System.Text.Json;

namespace WeatherForcast.Middleware
{
    public class GlobalExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var request = await context.GetHttpRequestDataAsync();
                var respose = request.CreateResponse();
                switch (ex)
                {
                    case DuplicateRegisterException exception:
                        respose.StatusCode = HttpStatusCode.Conflict;
                        break;
                    default:
                        respose.StatusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                var message = ex?.Message ?? "Unknown Error";
                string responseBody = JsonSerializer.Serialize(message);
                await respose.WriteStringAsync(responseBody);
                var functionName = context.FunctionDefinition.Name;

                Console.WriteLine($"An error occurred in function '{functionName} ': {message}");
                context.GetInvocationResult().Value = respose;
            }
        }

    }
}
