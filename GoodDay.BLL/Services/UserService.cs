using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;
        private IContactService contactService;
        private UserManager<User> userManager;
        //private ChatHub 
        public UserService(IUnitOfWork _unitOfWork, IContactService _contactService, UserManager<User> _userManager)
        {
            unitOfWork = _unitOfWork;
            contactService = _contactService;
            userManager = _userManager;
        }

        public async Task<User> ShowUsersProfile(string id)
        {
            try
            {
                User user = await unitOfWork.Users.Get(id);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UserExists(string id)
        {
            return unitOfWork.Users.UserExists(id);
        }

    }
}
