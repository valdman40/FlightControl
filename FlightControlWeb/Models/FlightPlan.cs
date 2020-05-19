using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        private int passangers;
        private string company_name;
        private DateTime date_time;
        private Location initial_location;
        private Segment[] segments;
        public DateTime Date { get { return date_time; } set { date_time = value; } }  
        public int Passangers { get { return passangers; } set { passangers = value; } }
        public string Company_name { get { return company_name; } set { company_name = value; } }
        public Location Initial_location { get { return initial_location; } set { initial_location = value; } }
        public Segment[] Segments { get { return segments; } set { segments = value; } }
    }
}
