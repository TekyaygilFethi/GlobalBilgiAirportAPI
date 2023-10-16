using AirportDistanceCalculator.Data.Services.AirportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Services.AirportService
{
    public interface IAirportService
    {
        Task<string> MeasureDistance(MeasureDistanceRequestModel requestModel);
    }
}
