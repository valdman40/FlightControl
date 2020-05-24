using FlightControlWeb.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightPlanManager
    {
        FlightPlan getFlightPlan(int id);
        void addFlightPlan(FlightPlan flightPlan);
    }
}
