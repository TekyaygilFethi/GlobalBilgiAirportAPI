using AirportDistanceCalculator.Business.Repositories;
using AirportDistanceCalculator.Business.Services.Base;
using AirportDistanceCalculator.Business.Services.User;
using AirportDistanceCalculator.Data.Services.UserService;
using GlobalBilgiQuiz.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Services.UserService
{
    public class UserService:BaseService, IUserService
    {
        private IRepository<Data.POCO.User> _userRepository;
        public UserService(IRepository<Data.POCO.User> userRepository, IConfiguration configuration): base(configuration) 
        {
            _userRepository = userRepository;
        }

        public Data.POCO.User? CheckUser(UserDTO user)
        {
            var salt = base.GetConfigurationValue("Salt");
            return _userRepository
                .GetSingle(_ => _.Username == user.Username && _.Password == CryptographyHelper.Encode(user.Password, salt ?? ""));
        }
    }
}
