using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Types
{
    public class FlightPlan
    {
        public int passengers { get; set; }
        public string company_name { get; set; }
        public Initial_Location initial_location { get; set; }
        public Segment[] segments { get; set; }


    }
}
