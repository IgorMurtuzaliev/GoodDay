using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        public UserManager<User> userManager;
        private ChatHub chatHub;
        private IBlockListService blockListService;
        public ChatController(UserManager<User> _userManager, ChatHub _chatHub, IBlockListService _blockListService)
        {
            userManager = _userManager;
            chatHub = _chatHub;
            blockListService = _blockListService;
        }

        //public async Task<ActionResult<Dialog>> StartDialog(string friendId)
        //{
        //    var id = User.Claims.First(c => c.Type == "Id").Value;
        //    var isFriendBlocked = await  blockListService.IsUserBlocked(id, friendId);
        //    var isUserBlocked =  await blockListService.IsUserBlocked(friendId, id);
        //    if ( isUserBlocked)
        //    {
        //        return BadRequest("This user blocked you");
        //    }
        //    if (isFriendBlocked)
        //    {
        //        return BadRequest("You blocked this user");
        //    }
        //    else
        //    {
                
        //    }
        //}

    }
}