using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherForcast.Entity;
using WeatherForcast.Functions;
using WeatherForcast.Functions.Helpers;
using WeatherForecast.Functions.Test.Factory;
using WeatherForecastTest.Factory;

namespace WeatherForecast.Functions.Test.Tests
{


    public class RegisterFunctionTests
    {
        private readonly Mock<ILogger<Register>> logger;
        public RegisterFunctionTests()
        {
            logger = new Mock<ILogger<Register>>();
        }

        [Fact]
        public async Task Register_GivenEmptyInputBindData_ReturnSuccessHttpResponseData()
        {
            // Arrange
            var register = new Register(logger.Object);
            var functionContext = new Mock<FunctionContext>();
            var httpRequestData = new MockHttpRequestData(functionContext.Object, new Uri("http://localhost:7093/api/register/Adelaide"));
            var data = new List<ForecastRegister>();

          
            // Act
            var response = await register.Run(httpRequestData, data, functionContext.Object);


            // Assert
            var bodyMessage = response.HttpResponseData.Body.GetBodyMessageFromResponse();
            var responseData = JsonSerializer.Deserialize<ForecastRegister>(bodyMessage);

            var sqlBindData = response.ForecastRegister;

            response.HttpResponseData.Should().NotBeNull();
            response.HttpResponseData.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            bodyMessage.Should().NotBeNullOrEmpty();
            responseData.Should().NotBeNull();
            responseData.Id.Should().NotBeEmpty();
            responseData.Place.Should().Be("Adelaide");
            responseData.Data.Should().BeNull();

            responseData.Should().BeEquivalentTo(sqlBindData);

        }

        [Fact]
        public async Task Register_GivenExistingInputBindData_ReturnConflictHttpResponseData()
        {
            // Arrange
            var register = new Register(logger.Object);
            var functionContext = new Mock<FunctionContext>();
            var httpRequestData = new MockHttpRequestData(functionContext.Object, new Uri("http://localhost:7093/api/register/Adelaide"));
            var data = new List<ForecastRegister>() { new ForecastRegister() { Id = new Guid(), Place = "Adelaide", Data = null} };
            // Act
            var response = await register.Run(httpRequestData, data, functionContext.Object);

            // Assert
            var bodyMessage = response.HttpResponseData.Body.GetBodyMessageFromResponse();
            var sqlBindData = response.ForecastRegister;
            response.HttpResponseData.Should().NotBeNull();
            response.HttpResponseData.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
            bodyMessage.Should().NotBeNullOrEmpty();
            sqlBindData.Should().BeNull();

        }
    }
}
