using AirportDistanceCalculator.API.Controllers.Base;
using AirportDistanceCalculator.Business.JWTFolder;
using AirportDistanceCalculator.Business.Services.User;
using AirportDistanceCalculator.Data.Base;
using AirportDistanceCalculator.Data.Exceptions;
using AirportDistanceCalculator.Data.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AirportDistanceCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService, IConfiguration configuration):base(configuration)
        {
            _userService = userService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] UserDTO user)
        {
            var dbUser = _userService.CheckUser(user);
            if (dbUser != null)
            {
                Claim roleClaim = new Claim("Role", "Admin");
                var jwt = JwtHelper.GetJwtToken(dbUser.Id.ToString(), _configuration, TimeSpan.FromHours(12), new Claim[] { roleClaim });

                return Ok(new ResponseObject<string>
                {
                    Message = "JWT Token",
                    Data = jwt,
                });
            }
            throw new AppException("User bulunamadı!");
        }

        [HttpGet, Route("Hata")]
        public IActionResult Hata()
        {
            throw new AppException("Initial Hata!");
        }


    }
}
