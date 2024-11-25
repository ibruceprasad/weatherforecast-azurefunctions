using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForcast.Middleware
{
    public class DuplicateRegisterException : Exception
    {

        public DuplicateRegisterException(string place) : base($"Registeration request for place: {place} is already taken.")
        {

        }


    }
}
