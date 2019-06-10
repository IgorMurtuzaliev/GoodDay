using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ChatController(UserManager<User> _userManager, ChatHub _chatHub)
        {
            userManager = _userManager;
            chatHub = _chatHub;
        }

    
    }
}