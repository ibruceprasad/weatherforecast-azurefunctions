using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForcast.Functions.Constants
{
    public static class Consts
    {
        public const string WEATHER_QUERY   = "SELECT * FROM Register WHERE Place = @Place";
        public const string WEATHER_QUERY_PARAM = "@Place={Query.Place}";
        public const string WEATHER_DB_CONNECTIONSTRING = "WeatherDbConnectionString";
    }
}
