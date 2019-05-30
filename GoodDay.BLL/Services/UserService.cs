using GoodDay.BLL.Interfaces;
using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodDay.BLL.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }
        public bool UserExists(string id)
        {
            return userRepository.UserExists(id);
        }
    }
}
