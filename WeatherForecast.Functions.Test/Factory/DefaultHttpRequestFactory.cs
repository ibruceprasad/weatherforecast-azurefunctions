using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace WeatherForecastTest.Factory
{
    public static class DefaultHttpRequestFactory
    {
        public static HttpRequest GetDefaultHttpRequest() => new DefaultHttpContext().Request;
    }
}
