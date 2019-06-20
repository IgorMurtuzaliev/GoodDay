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
    public class ChatController : ControllerBase
    {

        public UserManager<User> userManager;
        private IChatService chatService;
        public ChatController(UserManager<User> _userManager, IChatService _chatService)
        {
            userManager = _userManager;
            chatService = _chatService;
        }
        [HttpGet]
        [Authorize]
        [Route("dialogs")]
        public async Task<List<DialogViewModel>> GetAllDialogs()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            return await chatService.GetAllDialogs(id);
        }
        [HttpGet]
        [Authorize]
        [Route("dialog/{friendId}")]
        public async Task<DialogViewModel> GetDialog(string friendId)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            return await chatService.GetDialog(id, friendId);
        }
    }
}