using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Location
    {
        private string longitude;
        private string latitude;
        public string Longitude { get { return longitude; } set { longitude = value; } }
        public string Latitude { get { return latitude; } set { latitude = value; } }
    }
}
