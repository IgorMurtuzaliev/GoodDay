using GoodDay.BLL.DTO;
using Microsoft.AspNetCore.Identity;
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
    }
}
