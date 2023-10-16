using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportDistanceCalculator.API.Controllers.Base
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
