using AutoMapper;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Infrastructure;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GoodDay.BLL.Services
{
    public class AccountService:IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailSender emailService;
        private readonly IUnitOfWork unitOfWork;

        public AccountService(UserManager<User> _userManager, SignInManager<User> _signInManager, IEmailSender _emailService, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            emailService = _emailService;
            unitOfWork = _unitOfWork;
        }
        public async Task<IdentityResult> Create(RegisterDTO model, string url)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RegisterDTO, User>()
                    .ForMember("UserName", opt => opt.MapFrom(src => src.Email)));
            User user = Mapper.Map<RegisterDTO, User>(model);
            Mapper.Reset();
            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var encode = HttpUtility.UrlEncode(code);
                var callbackurl = new StringBuilder("https://").AppendFormat(url).AppendFormat("/api/account/confirmemail").AppendFormat($"?userId={user.Id}&code={encode}");
                await emailService.SendEmailAsync(user.Email, "Тема письма", $"Please confirm your account by <a href='{callbackurl}'>clicking here</a>.asnfasnfoansfansofjnajsnfaojsnfajns)))asbasjubgaousbgasjubgasgbaosaubgbwegbeg/asfasfa/asf/a.asfa.sfa.sf..asf.asfasf as fasfas fa sa.f as as fas .as fa s.fas a.sf asfas fa.s a.sf a");
                return result;
            }
            
            return result;
        }

        public async Task<object> TokenGeneration(string email)
        {
            IdentityOptions options = new IdentityOptions();
            var user = await userManager.FindByEmailAsync(email);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim("Id", user.Id.ToString()),
                 }),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256),
                Audience = AuthOptions.AUDIENCE,
                Issuer = AuthOptions.ISSUER
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
        public async Task<object> LogIn(LoginDTO model)
        {
            return await TokenGeneration(model.Email);
        }
        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            User user = await userManager.FindByIdAsync(userId);
            IdentityResult success = await userManager.ConfirmEmailAsync(user, code);
            return success;
        }

    
        public async Task<User> GetClientAccount(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            return user;
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
        public async Task<User> FindByPhoneAsync(string phone)
        {

            User user = await unitOfWork.Users.FindByPhone(phone);
            return user;
            
        }
    }
}
