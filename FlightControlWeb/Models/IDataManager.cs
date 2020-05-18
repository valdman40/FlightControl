using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
   public interface IDataManager
    {
        IEnumerable<Flight> GetAllFlights();
        Flight GetFlightById(string id);
        Flight AddFlight(Flight fp);
        void DeleteFlight(string id);
        Flight UpdateFlight(Flight f);



    }
}
