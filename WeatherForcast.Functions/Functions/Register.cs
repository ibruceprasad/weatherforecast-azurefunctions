using Azure;
using Azure.Core;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
using WeatherForcast.Entity;
using WeatherForcast.Functions.Models;
using WeatherForcast.Middleware;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace WeatherForcast.Functions
{
    public class Register
    {
        private readonly ILogger<Register> _logger;
        private const string inputQuery = "SELECT * FROM Register WHERE Place = @Place";
        private const string inputQueryParamters = "@place={place}";
        public Register(ILogger<Register> logger)
        {
            _logger = logger;
        }

   
        [Function(nameof(Register))]     
        public async Task<OutputType> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="register/{place}")] HttpRequestData request,
            [SqlInput(inputQuery,"WeatherDbConnectionString",System.Data.CommandType.Text, inputQueryParamters)]
            IReadOnlyList<ForecastRegister> registers,
   
            FunctionContext executionContext)
        {
            var placeParam = request.Url.Segments[3].Trim();
            //ForecastRegister[] forecastRegster = JsonConvert.DeserializeObject<ForecastRegister[]>(inputResult);
            var place = registers.FirstOrDefault()?.Place;
            HttpResponseData response = request.CreateResponse();

            if (place is null)
            {
                var regiter  = new ForecastRegister()
                {
                    Id = Guid.NewGuid(),
                    Place = placeParam
                };

                response = request.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync(JsonConvert.SerializeObject(regiter));
                return new OutputType()
                {
                    ForecastRegister = regiter,
                    HttpResponseData = response
                };
            }

            response = request.CreateResponse(System.Net.HttpStatusCode.Conflict);
            await response.WriteStringAsync($"Register failed - Registeration request for {place} is already taken");
            return new OutputType()
            {
                ForecastRegister = null,
                HttpResponseData = response
            };

            //return new ConflictObjectResult($"Registeration request for place: {place} is already taken");
            //throw new DuplicateRegisterException(place);
        }
    }
}


