using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;

using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.DAL.EF;
using GoodDay.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAccountService accountService;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> _userManager, IAccountService _accountService, SignInManager<User> _signInManager)
        {
            accountService = _accountService;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (accountService.PhoneExists(model.Phone) == false)
                {
                    IdentityResult result = await accountService.Create(model, HttpContext.Request.Host.ToString());
                    return Ok(result);
                }
                else
                {
                    return BadRequest("The phone number is already taken");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (model.Login.Contains("@"))
                {
                    var user = await userManager.FindByEmailAsync(model.Login);
                    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        if (user.EmailConfirmed == true)
                        {
                            var email = user.Email;
                            var token = await accountService.LogIn(email);
                            return Ok(new { token });
                        }
                        else return BadRequest("Confirm your email");
                    }
                    else return BadRequest("Email or password is incorrect");
                }
                else
                {
                    User user = await accountService.FindByPhoneAsync(model.Login);
                    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        if (user.EmailConfirmed == true)
                        {
                            var email = user.Email;
                            var token = await accountService.LogIn(email);
                            return Ok(new { token });
                        }
                        else return BadRequest("Confirm your email");
                    }
                    else return BadRequest("Phone or password is incorrect");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return NotFound();
            }
            IdentityResult result = await accountService.ConfirmEmail(userId, code);
            if (result.Succeeded)
                return Redirect("http://localhost:4200/login");
            else
                return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClientProfile()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var profile = new UserViewModel(user);
                return Ok(profile);
            }
            else return BadRequest();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditClientProfile(EditUserInfoViewModel model)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if (id == null)
            {
                return BadRequest("Id is not valid");
            }
            else
            {
                model.Id = id;
                await accountService.EditClientProfile(model);
                return Ok();
            }
        }
        [HttpPut]
        [Authorize]
        [Route("editAvatar")]
        public async Task<IActionResult> EditProfileImage()
        {
            try
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                var file = HttpContext.Request.Form.Files[0];
                const int lengthMax = 2097152;
                const string correctType = "image/jpeg";
                var type = file.ContentType;
                var length = file.Length;
                if (type != correctType)
                {
                    return BadRequest("Error, allowed image resolution jpg / jpeg");
                }

                if (length>lengthMax)
                {
                    return BadRequest("Error, image size should not be more than 2 MB");
                }
                else
                {
                    await accountService.EditProfileImage(id, file);
                    return Ok();
                }  
                }
              catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }   
        }
        [HttpPut]
        [Authorize]
        [Route("editPassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if (accountService.ChangePassword(id, model).IsCompletedSuccessfully)
            {
                return Ok();
            }
            else return BadRequest("ergergerge");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}