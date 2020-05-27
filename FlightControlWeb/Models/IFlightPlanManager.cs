using FlightControlWeb.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightPlanManager
    {
        FlightPlan getFlightPlan(string id);
        FlightPlan addFlightPlan(FlightPlan flightPlan);
    }
}
