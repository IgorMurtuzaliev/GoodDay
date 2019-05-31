using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        public UserService(IUserRepository _userRepository, UserManager<User> _userManager, IUnitOfWork _unitOfWork)
        {
            userRepository = _userRepository;
            userManager = _userManager;
            unitOfWork = _unitOfWork;
        }

        public async Task EditClientProfile(UserDTO model)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Name = model.Name;
                user.Surname = model.Surname;
                await unitOfWork.Users.Edit(user);
            }
            
        }
     
        public async Task<User> ShowUsersProfile(string id)
        {
            User user = await unitOfWork.Users.Get(id);
            return user;
        }
        
        public bool UserExists(string id)
        {
            return userRepository.UserExists(id);
        }
    }
}
