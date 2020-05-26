using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IDataManager
    {
       List<dynamic> ExcuteQuery(string QueryString);
       List<dynamic> ExcuteQuery<T>(string QueryString,T genericObject);
    }
}
