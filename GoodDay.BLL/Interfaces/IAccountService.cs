using GoodDay.BLL.DTO;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(RegisterDTO model, string url);
        Task<object> TokenGeneration(string email);
        Task<object> LogIn(LoginDTO model);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<User> GetClientAccount(string id);
        Task EditClientProfile(UserDTO model);
        Task<User> FindByPhoneAsync(string phone);
        Task EditProfileImage(UserDTO model);
        bool PhoneExists(string phone);
    }
}
