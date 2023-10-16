using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Data.Base
{
    public static class GLOBALS
    {
        public static readonly string AIRPORT_CACHE_KEY = "AIRPORT_{0}";
        public static readonly string AIRPORT_DISTANCE_CACHE_KEY = "DISTANCE_{0}_{1}_{2}"; // IST_SAW   |  SAW_IST

    }
}
