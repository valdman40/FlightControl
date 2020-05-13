using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class CacheManager : IDataManager
    {
        private static List<Flight> flist = new List<Flight>()
        {
            new Flight{Flight_id = 1},
            new Flight{Flight_id = 2},
            new Flight{Flight_id = 3}};

        public Flight AddFlight(Flight fp)
        {
            flist.Add(fp);
            return fp;
        }

        public void DeleteFlight(int id)
        {
            Flight f = flist.Where(x => x.Flight_id == id).FirstOrDefault();
            if (f == null)
            {
                throw new Exception("product not found");
            }
            flist.Remove(f);
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return flist;
        }

        public Flight GetFlightById(int id)
        {
            Flight f = flist.Where(x => x.Flight_id == id).FirstOrDefault();
            if (f == null)
            {
                throw new Exception("product not found");
            }
            return f;
        }

        public Flight UpdateFlight(Flight f)
        {
            Flight f1 = flist.Where(x => x.Flight_id == f.Flight_id).FirstOrDefault();
            f1.Company_name = f.Company_name;
            return f;
        }
    }
}
