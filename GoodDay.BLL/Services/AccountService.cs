using AutoMapper;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GoodDay.BLL.Services
{
    public class AccountService:IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService emailService;

        public AccountService(UserManager<User> _userManager, SignInManager<User> _signInManager, IEmailService emailService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        public async Task<IdentityResult> Create(RegisterDTO model, string url)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RegisterDTO, User>()
            .ForMember("UserName", opt => opt.MapFrom(c => c.Email)));
            User user = Mapper.Map<RegisterDTO, User>(model);

            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var encode = HttpUtility.UrlEncode(code);
                var callbackurl = new StringBuilder("https://").AppendFormat(url).AppendFormat("/api/account/confirmemail").AppendFormat($"?userId={user.Id}&code={encode}");
                await emailService.SendEmail(user.Email, "Тема письма", $"Please confirm your account by <a href='{callbackurl}'>clicking here</a>.");
                await userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }
    }
}
