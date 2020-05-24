using FlightControlWeb.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IFlightManager
    {
        List<Flight> GetAllFlights(string date);
        List<Flight> GetInternalFlights(string date);
        void deleteFlight(int id);
        Flight getFlight(int id);
    }
}