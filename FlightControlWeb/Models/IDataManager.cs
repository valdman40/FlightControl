using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
   public interface IDataManager
    {
        IEnumerable<Flight> GetAllFlights();
        Flight GetFlightById(int id);
        Flight AddFlight(Flight fp);
        void DeleteFlight(int id);
        Flight UpdateFlight(Flight f);



    }
}
