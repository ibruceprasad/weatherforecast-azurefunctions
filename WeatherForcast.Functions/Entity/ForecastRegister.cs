using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForcast.Entity
{
    public class ForecastRegister
    {
        public Guid Id { get; set; }
        public string Place { get; set; }
        public string Data { get; set; }
    }
}
