using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(RegisterViewModel model, string url);
        Task<object> TokenGeneration(string email);
        Task<object> LogIn(string email);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task EditClientProfile(EditUserInfoViewModel model);
        Task<User> FindByPhoneAsync(string phone);
        Task EditProfileImage(string id, IFormFile file);
        bool PhoneExists(string phone);
        Task ChangePassword(string id, ChangePasswordViewModel model);
    }
}
