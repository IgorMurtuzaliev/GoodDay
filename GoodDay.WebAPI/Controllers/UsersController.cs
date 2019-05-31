using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
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
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private UserManager<User> userManager;

        public UsersController(IUserService _userService, UserManager<User> userManager)
        {
            userService = _userService;
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditClientProfile([FromForm]UserViewModel model)
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
                    await userService.EditClientProfile(userModel);
                    return Ok();               
            }
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersProfile(string id)
        {
            if(id == null)
            {
                return BadRequest("Id is not valid");
            }
            else
            {
                if (!userService.UserExists(id))
                {
                    return BadRequest("There are not users with this id");
                }
                else
                {
                    User user = await userService.ShowUsersProfile(id);
                    return Ok(user);
                }
            }
        }
    }
}