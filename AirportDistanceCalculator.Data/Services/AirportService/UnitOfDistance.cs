using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Data.Services.AirportService
{
    public class UnitOfDistance
    {
        private readonly double _fromMetersFactor;

        public UnitOfDistance(DistanceMetricEnum metric)
        {
            if (metric == DistanceMetricEnum.Meters)
                _fromMetersFactor = 1;
            else if (metric == DistanceMetricEnum.Kilometers)
                _fromMetersFactor = 0.001;
            else if (metric == DistanceMetricEnum.Miles)
                _fromMetersFactor = 0.000621371192;
        }

        public double ConvertFromMeters(double input) => input * _fromMetersFactor;

    }
}
