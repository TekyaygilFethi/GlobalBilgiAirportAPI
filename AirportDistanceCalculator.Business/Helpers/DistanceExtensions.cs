using AirportDistanceCalculator.Data.Services.AirportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Helpers
{
    public static class DistanceExtensions
    {
        private readonly static double EARTH_RADIUS_METERS = 6376500.0;

        public static double DistanceTo(this Coordinate baseCoordinates, Coordinate targetCoordinates, DistanceMetricEnum distanceMetric)
        {
            var unitOfDistance = new UnitOfDistance(distanceMetric);

            var d1 = baseCoordinates.Latitude * (Math.PI / 180.0);
            var num1 = baseCoordinates.Longitude * (Math.PI / 180.0);
            var d2 = targetCoordinates.Latitude * (Math.PI / 180.0);
            var num2 = targetCoordinates.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            var distance = EARTH_RADIUS_METERS * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

            return unitOfDistance.ConvertFromMeters(distance);
        }

    }
}
