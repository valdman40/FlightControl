using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Types
{
    public class Flight
    {
        public string flight_id { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double company_name { get; set; }
        public DateTime date_time { get; set; }
        public bool is_external { get; set; }
        public int passengers { get; set; }
    }
}
