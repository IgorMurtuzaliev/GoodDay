using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private UserManager<User> userManager;

        public UsersController(IUserService _userService, UserManager<User> _userManager)
        {
            userService = _userService;
            userManager = _userManager;
        }
      

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUsersProfile(string id)
        { 
            if(id == null)
            {
                return BadRequest(new { message = "Id is not valid" });
            }
            else
            {
                if (!userService.UserExists(id))
                {
                   return BadRequest(new { message = "There are not users with this id" });
                }
                else
                {
                    var user = await userManager.FindByIdAsync(id);
                    var profile = new UserViewModel(user);
                    return Ok(profile);;
                }
            }
        }
    }
}