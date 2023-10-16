using AirportDistanceCalculator.Business.CacheFolder.Service;
using AirportDistanceCalculator.Business.Helpers;
using AirportDistanceCalculator.Data.BackgroundServices;
using AirportDistanceCalculator.Data.Base;
using AirportDistanceCalculator.Data.Exceptions;
using AirportDistanceCalculator.Data.Services.AirportService;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Services.AirportService
{
    public class AirportService:IAirportService
    {
        private readonly ICacheService _cacheService;
        public AirportService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<string> MeasureDistance(MeasureDistanceRequestModel requestModel)
        {
            ValidateRequestModel(requestModel);
            PreprocessRequestModel(requestModel);

            var distanceCacheKey = string.Format(GLOBALS.AIRPORT_DISTANCE_CACHE_KEY, requestModel.IATACode1, requestModel.IATACode2, requestModel.DistanceMetric.ToString());
            var distanceObject = _cacheService.Get<AirportDistanceCacheObject>(distanceCacheKey);
            if (distanceObject == null)
            {
                distanceCacheKey = string.Format(GLOBALS.AIRPORT_DISTANCE_CACHE_KEY, requestModel.IATACode2, requestModel.IATACode1, requestModel.DistanceMetric.ToString());
                distanceObject = _cacheService.Get<AirportDistanceCacheObject>(distanceCacheKey);
            }

            if (distanceObject == null)
            {
                var airport1 = _cacheService.Get<Airport>(string.Format(GLOBALS.AIRPORT_CACHE_KEY, requestModel.IATACode1));
                if (airport1 == null)
                    throw new AppException($"Havalimanı bulunamadı! IATA Kodu: {requestModel.IATACode1}");

                var airport2 = _cacheService.Get<Airport>(string.Format(GLOBALS.AIRPORT_CACHE_KEY, requestModel.IATACode2));
                if (airport2 == null)
                    throw new AppException($"Havalimanı bulunamadı! IATA Kodu: {requestModel.IATACode2}");

                var distance = new Coordinate(airport1.Location.Latitude, airport1.Location.Longitude)
                   .DistanceTo(new Coordinate(airport2.Location.Latitude, airport2.Location.Longitude), requestModel.DistanceMetric);

                AirportDistanceCacheObject adco = new AirportDistanceCacheObject()
                {
                    Airport1 = new AirportCacheObject { Name = airport1.Name, IATACode = airport1.IATA},
                    Airport2 = new AirportCacheObject { Name = airport2.Name, IATACode = airport2.IATA},
                    Distance = distance
                };

                _cacheService.Set(distanceCacheKey, adco);

                 return $"{airport1.Name}({airport1.IATA}) havalimanı ile {airport2.Name}({airport2.IATA}) havalimanı arasındaki mesafe: {distance} {requestModel.DistanceMetric.ToString()}";
            }

            return $"{distanceObject.Airport1.Name}({distanceObject.Airport1.IATACode}) havalimanı ile {distanceObject.Airport2.Name}({distanceObject.Airport2.IATACode}) havalimanı arasındaki mesafe: {distanceObject.Distance} {requestModel.DistanceMetric.ToString()}";

        }

        private void ValidateRequestModel(MeasureDistanceRequestModel requestModel)
        {
            if (requestModel.IATACode1 == requestModel.IATACode2)
                throw new AppException("Aynı havalimanı kodlarını giremezsiniz!");
            if (requestModel.IATACode1.CheckIsNullOrEmpty() || requestModel.IATACode1.CheckIsNullOrEmpty())
                throw new AppException("IATA kodları boş olamaz lütfen geçerli bir değer giriniz!");
        }

        private void PreprocessRequestModel(MeasureDistanceRequestModel requestModel)
        {
            requestModel.IATACode1 = requestModel.IATACode1.ToUpper();
            requestModel.IATACode2 = requestModel.IATACode2.ToUpper();
        }
    }
}
