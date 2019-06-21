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
        public ChatController(UserManager<User> _userManager, IChatService _chatService, IHubContext<ChatHub> _hubContext)
        {
            userManager = _userManager;
            chatService = _chatService;
            hubContext = _hubContext;
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

        [Route("files")]
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        var imageMessage = new Models.Entities.File
                        {
                            Name = file.Name,
                            Stream = memoryStream.ToString()
                        };

                        await hubContext.Clients.All.SendAsync("ImageMessage", imageMessage);
                    }
                }
            }
            return Ok();
        }
    }
}