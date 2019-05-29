using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.EF;
using GoodDay.Models.Entities;
using GoodDay.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [EnableCorsAttribute("https://accounts.google.com")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAccountService service;

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private ApplicationDbContext db;
        public AccountController(UserManager<User> _userManager, IAccountService _service, SignInManager<User> _signInManager, ApplicationDbContext _db)
        {
            service = _service;
            userManager = _userManager;
            signInManager = _signInManager;

            db = _db;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var register = new RegisterDTO
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password,
                PasswordConfirm = model.PasswordConfirm
            };
            if (ModelState.IsValid)
            {
                IdentityResult result = await service.Create(register, HttpContext.Request.Host.ToString());
                if (result.Succeeded)
                {
                    return Ok(register);
                }
            }
            return Ok(model);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.EmailConfirmed == true)
                {
                    var userModel = new LoginDTO
                    {
                        Email = model.Email,
                        Password = model.Password

                    };
                    var token = await service.LogIn(userModel);
                    return Ok(new { token });
                }
                else return BadRequest(new { message = "Confirm your email" });
            }
            else return BadRequest(new { message = "Username or password is incorrect" });
        }

        [HttpGet]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return NotFound();
            }
            IdentityResult result = await service.ConfirmEmail(userId, code);
            if (result.Succeeded)
                return Ok("Welcome");
            else
                return NotFound();
        }


        [Route("signInWithGoogle")]
        public async Task<IActionResult> SignIn()
        {
            SignInWithGoogle();
            var id = User.Claims.First(c => c.Type == "Id").Value;
            User user = await userManager.FindByIdAsync(id);
            var token = await service.TokenGeneration(user.Email);
            return Ok(new { token });

        }

        public IActionResult SignInWithGoogle()
        {
            var authenticationProperties = signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(HandleExternalLogin)));
            return Challenge(authenticationProperties, "Google");

        }

        public async void HandleExternalLogin()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (!result.Succeeded) //user does not exist yet
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var newUser = new User
                {
                    Name = email,
                    Surname = email,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    Phone = "",

                };
                var createResult = await userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                    throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

                await userManager.AddLoginAsync(newUser, info);
                var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
                await userManager.AddClaimsAsync(newUser, newUserClaims);
                //await signInManager.SignInAsync(newUser, isPersistent: false);
                //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);               
            }

        }

    }
}