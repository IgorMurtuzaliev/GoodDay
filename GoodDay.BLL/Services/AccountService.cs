using AutoMapper;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Infrastructure;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using File = GoodDay.Models.Entities.File;

namespace GoodDay.BLL.Services
{
    public class AccountService:IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailSender emailService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment appEnvironment;

        public AccountService(UserManager<User> _userManager, SignInManager<User> _signInManager, IEmailSender _emailService, IUnitOfWork _unitOfWork, IHostingEnvironment _appEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            emailService = _emailService;
            unitOfWork = _unitOfWork;
            appEnvironment = _appEnvironment;
        }
        public async Task<IdentityResult> Create(RegisterDTO model, string url)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RegisterDTO, User>()
                    .ForMember("UserName", opt => opt.MapFrom(src => src.Email)));
            User user = Mapper.Map<RegisterDTO, User>(model);
            Mapper.Reset();
            try
            {
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encode = HttpUtility.UrlEncode(code);
                    var callbackurl = new StringBuilder("https://").AppendFormat(url).AppendFormat("/api/account/confirmemail").AppendFormat($"?userId={user.Id}&code={encode}");
                    await emailService.SendEmailAsync(user.Email, "Тема письма", $"Please confirm your account by <a href='{callbackurl}'>clicking here</a>.We hope you enjoy it and tell your friends about our chat!");
                    return result;
                }
            
            return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
            try
            {
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    await unitOfWork.Users.Edit(user);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> FindByPhoneAsync(string phone)
        {
            User user = await unitOfWork.Users.FindByPhone(phone);
            return user;        
        }
         public bool PhoneExists(string phone)
        {
            return unitOfWork.Users.PhoneExists(phone);
        }
        public async Task EditProfileImage(UserDTO model)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            try
            {
                string path = appEnvironment.WebRootPath + "\\Avatar\\" + user.UserName + "\\" + model.Avatar.FileName;
                string directory = Path.Combine(appEnvironment.WebRootPath + "\\Avatar\\" + user.UserName);
               
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.Avatar.CopyToAsync(fileStream);
                }
                File newfile = new File { Name = model.Avatar.FileName, Path = path, UserId = user.Id };
                await unitOfWork.Files.Add(newfile);
                user.FileId = newfile.Id;
                await unitOfWork.Users.Edit(user);
                await unitOfWork.Save();
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
