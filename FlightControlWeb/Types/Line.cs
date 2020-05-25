using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Types
{
    public class Line
    {
        public Point start { get; set; }
        public Point end { get; set; }
        public Point getPointOnSegment(double partial)
        {
            Point returnVal = new Point();
            returnVal.X = start.X + partial * (end.X - start.X);
            returnVal.Y = start.Y + partial * (end.Y - start.Y);
            return returnVal;
        }


    }
}
