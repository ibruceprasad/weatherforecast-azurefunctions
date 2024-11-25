using Azure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForcast.Functions.Helpers
{
    public static class FunctionsHelpers
    {
        public static string GetWetherApiUrl(string baseUrl, string place)
        {
            var url = QueryHelpers.AddQueryString(baseUrl, new Dictionary<string, string>
            {
                { "key", Environment.GetEnvironmentVariable("WeatherApiKey")},
                { "q", place }
            });
            return url;

        }


        public static string GetBodyMessageFromResponse(this Stream stream)
        {
            string body = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }
            return body;
        }


    }
}
