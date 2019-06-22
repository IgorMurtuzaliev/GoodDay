using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private IHubContext<ChatHub> hubContext;
        private UserManager<User> userManager;
        private IChatService chatService;
        private IChatHub chatHub;
        public ChatController(UserManager<User> _userManager, IChatService _chatService, IHubContext<ChatHub> _hubContext, IChatHub _chatHub)
        {
            userManager = _userManager;
            chatService = _chatService;
            hubContext = _hubContext;
            chatHub = _chatHub;
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

        [HttpPost]
        [Authorize]
        [Route("send")]
        public async Task SendMessage(PostMessageViewModel postMessage)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            await chatHub.SendFaraway()
        }
        
    }
}