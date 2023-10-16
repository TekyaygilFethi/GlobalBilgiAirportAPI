using AirportDistanceCalculator.Data.BackgroundServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Data.Services.AirportService
{
    public class AirportDistanceCacheObject
    {
        public AirportCacheObject Airport1{ get; set; }
        public AirportCacheObject Airport2{ get; set; }
        public double? Distance { get; set; }
    }
}
