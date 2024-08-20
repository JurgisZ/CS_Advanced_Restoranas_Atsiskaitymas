using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services
{
    internal class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public User? ValidateUser(string userLogInName, string userLogInPassCode)
        {
            var users = _userRepository.GetAll();
            var user = users.Find(x => (x.UserLogInName == userLogInName) && (x.UserLogInPassCode == userLogInPassCode));

            return user;
        }

    }
}
