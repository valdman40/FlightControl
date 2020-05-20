using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Initial_Location
    {
        private string longitude;
        private string latitude;
        private DateTime date_time;
        public string Longitude { get { return longitude; } set { longitude = value; } }
        public string Latitude { get { return latitude; } set { latitude = value; } }
        public DateTime Date { get { return date_time; } set { date_time = value; } }
    }
}
