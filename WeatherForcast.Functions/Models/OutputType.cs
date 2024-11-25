using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForcast.Entity;

namespace WeatherForcast.Functions.Models
{
    public class OutputType
    {
        [SqlOutput("dbo.Register", connectionStringSetting: "WeatherDbConnectionString")]
        public ForecastRegister ForecastRegister { get; set; }
        public HttpResponseData HttpResponseData { get; set; }
    }
}
