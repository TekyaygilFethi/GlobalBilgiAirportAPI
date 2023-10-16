using AirportDistanceCalculator.Data.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Services.User
{
    public interface IUserService
    {
        Data.POCO.User? CheckUser(UserDTO user);
    }
}
