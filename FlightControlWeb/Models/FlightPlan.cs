using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        private string id;
        public string ID{ get { return id; } set { id = value; } }
//        private DateTime date_time;
  //      private Location initial_location;
    //    private Segment[] segments;
        private string company_name;
        public string Company { get { return company_name; } set { company_name = value; } }
        private int passangers;
        public int Passangers { get { return passangers; } set { passangers = value; } }
       // public Location Location_ID { get { return initial_location; } set { initial_location = value; } }
      //  public Segment[] Segments { get { return segments; } set { segments = value; } }
    }
}
