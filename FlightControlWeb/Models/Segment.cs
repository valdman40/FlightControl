using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Segment
    {
        private string longitude;
        private string latitude;
        private TimeSpan timespan_seconds;
        public string Longitude { get { return longitude; } set { longitude = value; } }
        public string Latitude { get { return latitude; } set { latitude = value; } }
        public TimeSpan Date { get { return timespan_seconds; } set { timespan_seconds = value; } }
    }
}
