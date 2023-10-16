using AirportDistanceCalculator.API.Controllers.Base;
using AirportDistanceCalculator.Business.Services.AirportService;
using AirportDistanceCalculator.Data.Base;
using AirportDistanceCalculator.Data.Services.AirportService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AirportDistanceCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : BaseController
    {
        private readonly IAirportService _airportService;
        public AirportController(IAirportService airportService, IConfiguration configuration) : base(configuration)
        {
            _airportService = airportService;
        }

        [Authorize(AuthenticationSchemes = "Bearer"), HttpGet, Route("distance")]
        public async Task<ResponseObject<string>> MeasureDistance([FromQuery] MeasureDistanceRequestModel requestModel)
        {
            var userId = User.Claims.SingleOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
            var distanceMessage = await _airportService.MeasureDistance(requestModel);

            return new ResponseObject<string>()
            {
                Data = distanceMessage,
            };

        }
    }
}
