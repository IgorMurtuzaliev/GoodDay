﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.EF;
using GoodDay.Models.Entities;
using GoodDay.WebAPI.ViewModels;
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
            Mapper.Initialize(cfg => cfg.CreateMap<RegisterViewModel, RegisterDTO>());
            var register = Mapper.Map<RegisterViewModel, RegisterDTO>(model);
            Mapper.Reset();
            try
            {           
                if (accountService.PhoneExists(register.Phone) == false)
                {
                    IdentityResult result = await accountService.Create(register, HttpContext.Request.Host.ToString());
                    return Ok(result);
                }
                else
                {
                    return BadRequest("The phone number is already taken");
                }

            }
            catch(Exception ex) 
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
    var             user = await userManager.FindByEmailAsync(model.Login);
                    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        if (user.EmailConfirmed == true)
                        {
                            var userModel = new LoginDTO
                            {
                                Email = model.Login,
                                Password = model.Password
                            }; 
                            var token = await accountService.LogIn(userModel);
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
                            var userModel = new LoginDTO
                            {
                                Email = email,
                                Password = model.Password,
                                Phone = model.Login
                            };
                            var token = await accountService.LogIn(userModel);
                            return Ok(new { token });
                        }
                        else return BadRequest("Confirm your email" );
                    }
                    else return BadRequest("Phone or password is incorrect");
                }
            }
            catch(Exception ex)
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
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClientProfile()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            var user = await userManager.FindByIdAsync(id);
            return Ok(user);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditClientProfile(UserViewModel model)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if (id == null)
            {
                return BadRequest("Id is not valid");
            }
            else
            {
                var userModel = new UserDTO
                {
                    Id = id,
                    Name = model.Name,
                    Surname = model.Surname,
                };
                await accountService.EditClientProfile(userModel);
                return Ok();
            }
        }
        [HttpPut]
        [Authorize]
        [Route("editAvatar")]
        public async Task<IActionResult> EditProfileImage([FromForm]UserViewModel model)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            const int lengthMax = 2097152;
            const string correctType = "image/jpeg";
            var type = model.Avatar.ContentType;
            var length = model.Avatar.Length;
            if (type != correctType)
            {
                ModelState.AddModelError("Uploads", "Error, allowed image resolution jpg / jpeg");
                return BadRequest(ModelState);
            }

            if (length>lengthMax)
            {
                ModelState.AddModelError("Uploads", "Error, image size should not be more than 2 MB");
                return BadRequest(ModelState);
            }
            else
            {
                var userModel = new UserDTO
                {
                    Id = id,
                    CurrentAvatar = model.CurrentAvatar,
                    Avatar = model.Avatar
                };
                await accountService.EditProfileImage(userModel);
                return Ok();
            }       
        }
    }
}