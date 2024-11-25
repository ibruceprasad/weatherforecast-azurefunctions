using FluentAssertions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using WeatherForcast.Entity;
using WeatherForcast.Functions;
using WeatherForecastTest.Factory;

namespace WeatherForecast.Functions.Test.Tests
{
    public class QueryFunctionTests
    {
        private readonly Mock<ILogger<Query>> logger;

        public QueryFunctionTests()
        {
            logger = new Mock<ILogger<Query>>();
        }

        [Fact]
        public async Task Query_GivenInputBindData_ReturnSuccessHttpResponse()
        {
            // Arrange
            var query = new Query(logger.Object);
            var httpRequest = DefaultHttpRequestFactory.GetDefaultHttpRequest();
            string jsonContent = await File.ReadAllTextAsync("TestData/LondonWeatherData.json");
            var result = new ForecastRegister() { Id = Guid.NewGuid(), Place = "London", Data = jsonContent };
            var resultList = new List<ForecastRegister>() { result };
            var inputBindResult = JsonSerializer.Serialize(resultList);

            // Act
            IActionResult actionResult = query.Run(httpRequest, inputBindResult);
            var okResult = actionResult as OkObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            actionResult.Should().BeOfType<OkObjectResult>();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task Query_GivenNoInputBindData_ReturnNotFoundHttpResponse()
        {
            // Arrange
            var query = new Query(logger.Object);
            var httpRequest = DefaultHttpRequestFactory.GetDefaultHttpRequest();
            var queryParams = new Dictionary<string, StringValues>
            {
                { "Place", "London" },
            };
            httpRequest.Query = new QueryCollection(queryParams);
            var resultList = new List<ForecastRegister>();
            var inputBindResult = JsonSerializer.Serialize(resultList);

            // Act
            IActionResult actionResult = query.Run(httpRequest, inputBindResult);
            var okResult = actionResult as NotFoundObjectResult;

            //Assert
            okResult.Should().NotBeNull();
            actionResult.Should().BeOfType<NotFoundObjectResult>();
            okResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            okResult.Value.Should().Be("Query failed - Weather data for London is not found");
        }

    }
}