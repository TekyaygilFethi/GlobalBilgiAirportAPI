using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Services.Base
{
    public class BaseService
    {
        private IConfiguration _configuration;

        public BaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? GetConfigurationValue(string key)
        {
            return _configuration.GetSection(key)?.Value;
        }
    }
}
