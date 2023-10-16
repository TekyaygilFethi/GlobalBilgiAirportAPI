using AirportDistanceCalculator.Business.CacheFolder.Service;
using AirportDistanceCalculator.Business.HttpRequestFolder;
using AirportDistanceCalculator.Data.BackgroundServices;
using AirportDistanceCalculator.Data.Base;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace AirportDistanceCalculator.BackgroundServices.Managers.RecurringJobs
{
    public class CacheAirportsJobManager
    {
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        public CacheAirportsJobManager(ICacheService cacheService, IConfiguration configuration)
        {
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task Perform()
        {
            using var _httpHelper = new HttpRequestHelper();

            string IATALines = File.ReadAllText(@".\Files\alliatadata.txt");

            string[] IATALineArray = IATALines.Replace("\r", "").Split("\n");

            var baseUrl = _configuration.GetSection("IATACodeBaseUrl")?.Value;

            foreach(var iata in IATALineArray)
            {
                try
                {
                    var airportData = await _httpHelper.Get<Airport>(string.Format(baseUrl, iata));

                    _cacheService.Set(string.Format(GLOBALS.AIRPORT_CACHE_KEY, iata), airportData);
                }
                catch (Exception) { continue; }
            }
        }
    }
}
