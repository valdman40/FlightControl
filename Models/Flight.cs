using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Flight
    {
        private string flight_id;
        private int passangers;
        private string company_name;
        private string date_time;
        private Location loc;
        private DateTime date;
        private bool is_external;
        public string Flight_id { get { return flight_id; } set { flight_id = value; } }
        public int Passangers { get { return passangers; } set { passangers = value; } }
        public string Company_name { get { return company_name; } set { company_name = value; } }
        public string Date_time { get { return date_time; } set { date_time = value; } }
        public Location Loc { get { return loc; } set { loc = value; } }
        public bool Is_external { get { return is_external; } set { is_external = value; } }
    }
}
