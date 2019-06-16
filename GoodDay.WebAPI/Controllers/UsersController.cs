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
        private IContactService contactService;
        private IBlockListService blockListService;

        public UsersController(IUserService _userService, UserManager<User> _userManager, IContactService _contactService, IBlockListService _blockListService)
        {
            userService = _userService;
            userManager = _userManager;
            contactService = _contactService;
            blockListService = _blockListService;
        }
      

        [HttpGet("{friendId}")]
        [Authorize]
        public async Task<IActionResult> GetUsersProfile(string friendId)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if (friendId == null)
            {
                return BadRequest(new { message = "Id is not valid" });
            }
            else
            {
                if (!userService.UserExists(friendId))
                {
                   return BadRequest(new { message = "There are not users with this id" });
                }
                else
                {
                    var user = await userManager.FindByIdAsync(friendId);
                    var profile = new UserViewModel(user);
                    if (await userService.IsUserBlocked(id, friendId))
                    {
                        profile.IsBlocked = true;
                    }
                    else profile.IsBlocked = false;
                    if (await userService.IsInContacts(id, friendId))
                    {
                        var contact = await contactService.FindContact(id, friendId);
                        profile.ContactWithUserId = contact.Id;
                        profile.IsInContacts = true;
                    }
                    else profile.IsInContacts = false;

                    return Ok(profile);;
                }
            }
        }
        [HttpGet]
        [Authorize]
        [Route("block/{friendId}")]
        public async Task<IActionResult> BlockUser(string friendId)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if (friendId == null)
            {
                return BadRequest(new { message = "Id is not valid" });
            }
            else
            {
                if (!userService.UserExists(friendId))
                {
                    return BadRequest(new { message = "There are not users with this id" });
                }
                else
                {
                    await userService.BlockUser(id, friendId);
                    return Ok(); ;
                }
            }
        }
        [HttpGet]
        [Authorize]
        [Route("unlock/{friendId}")]
        public async Task<IActionResult> UnlockUser(string friendId)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if (friendId == null)
            {
                return BadRequest(new { message = "Id is not valid" });
            }
            else
            {
                if (!userService.UserExists(friendId))
                {
                    return BadRequest(new { message = "There are not users with this id" });
                }
                else
                {
                    await userService.UnlockUser(id, friendId);
                    return Ok(); ;
                }
            }
        }

        [HttpGet]
        [Authorize]
        [Route("blackList")]
        public async Task<ActionResult<IEnumerable<BlockListViewModel>>> GetBlackList()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value; ;
            var result = new List<BlockListViewModel>();
            var blocks =  await blockListService.GetBlockList(id);
            foreach (var item in blocks)
            {
                result.Add(new BlockListViewModel(item));
            }
            return Ok(result);
        }
    }
}