using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.EF;
using GoodDay.Models.Entities;
using GoodDay.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
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
                Password = model.Password,
                PasswordConfirm = model.PasswordConfirm
            };
            //Mapper.Initialize(cfg => cfg.CreateMap<RegisterViewModel, RegisterDTO>());
            //var register = Mapper.Map<RegisterViewModel, RegisterDTO>(model);
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

    }
}