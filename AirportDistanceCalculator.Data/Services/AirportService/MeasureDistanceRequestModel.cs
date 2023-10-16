using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Data.Services.AirportService
{
    public class MeasureDistanceRequestModel
    {
        public string IATACode1 { get; set; }
        public string IATACode2 { get; set; }

        public DistanceMetricEnum DistanceMetric { get; set; } = DistanceMetricEnum.Kilometers;
    }

    public enum DistanceMetricEnum
    {
        Miles,
        Kilometers,
        Meters
    }
}
