﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Infrastructure;
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
        private IDialogService dialogService;
        private IChatHub chatHub;
        private IBlockListService blockListService;
        public ChatController(UserManager<User> _userManager, IChatService _chatService, IHubContext<ChatHub> _hubContext, IChatHub _chatHub, IDialogService _dialogService, IBlockListService _blockListService)
        {
            userManager = _userManager;
            chatService = _chatService;
            hubContext = _hubContext;
            chatHub = _chatHub;
            dialogService = _dialogService;
            blockListService = _blockListService;
        }
        [HttpGet]
        [Authorize]
        [Route("dialogs")]
        public async Task<List<DialogViewModel>> GetAllDialogs()
        {
            try
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                return await chatService.GetAllDialogs(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("dialog/{friendId}")]
        public async Task<ActionResult<DialogViewModel>> GetDialog(string friendId)
        {
            try
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                if (await chatService.IsDialogExists(id, friendId))
                {
                    return await chatService.GetDialog(id, friendId);
                }
                else return BadRequest("Add new message to start dialog");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("sendMessage")]
        public async Task<IActionResult> SendMessage([FromForm]PostMessageViewModel postMessage)
        {
            try
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                if(await blockListService.IsUserBlocked(id, postMessage.ReceiverId))
                {
                    return BadRequest("Unlock this user to send messages");
                }
                if (await blockListService.IsUserBlocked(postMessage.ReceiverId, id))
                {
                    return BadRequest("This user reseieved access to sendind messages");
                }
                if (postMessage.Attachment==null && postMessage.Text == null)
                {
                    return BadRequest("You cannot send the empty message!");
                }
                if (postMessage.Attachment != null)
                {
                    foreach (var file in postMessage.Attachment)
                    {
                        if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "audio/mp3" || file.ContentType == "video/mp4")
                        {
                            if ((file.ContentType == "image/jpg" || file.ContentType == "image/jpeg") && file.Length >= 2097152)
                            {
                                return BadRequest("Unsupported file length. JPG/JPEG must be before 2MB ");
                            }
                            else if (file.ContentType == "audio/mp3" && file.Length >= 10484760)
                            {
                                return BadRequest("Unsupported file length. Audio files must be before 10MB ");
                            }
                            else if (file.ContentType == "video/mp4" && file.Length >= 31457280)
                            {
                                return BadRequest("Unsupported file length. Video files must be before 10MB ");
                            }
                        }
                        else
                        {
                            return BadRequest("Unsupported file type. Files must be only of 'jpg/jpeg', 'mp3', 'mp4' formats! ");
                        }
                    }
                }

                UserIds receiver, caller;
                chatHub.FindCallerReceiverByIds(postMessage.ReceiverId, id, out caller, out receiver);
                bool dialogExists = await chatService.IsDialogExists(caller.userId, postMessage.ReceiverId);
                if (dialogExists)
                {
                    var message = await chatService.AddNewMessage(caller.userId, postMessage, DateTime.Now);
                    if (chatService.IsOnline(postMessage.ReceiverId))
                    {
                        await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                        await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                    }
                    else
                    {
                        await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                    }
                    return Ok(message);
                }
                else
                {
                    await chatService.CreateDialog(caller.userId, postMessage.ReceiverId);
                    var message = await chatService.AddNewMessage(caller.userId, postMessage, DateTime.Now);
                    if (receiver != null)
                    {
                        if (chatService.IsOnline(postMessage.ReceiverId))
                        {
                            await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                        else
                        {
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                    }
                    return Ok(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Authorize]
        [Route("shareUserLink")]
        public async Task<IActionResult> ShareUserLink([FromForm]ShareUserMessageViewModel viewModel)
        {
            try
            {
                if (viewModel.Link != null)
                {
                    var id = User.Claims.First(c => c.Type == "Id").Value;
                    if (await blockListService.IsUserBlocked(id, viewModel.ReceiverId))
                    {
                        return BadRequest("Unlock this user to send messages");
                    }
                    if (await blockListService.IsUserBlocked(viewModel.ReceiverId, id))
                    {
                        return BadRequest("This user reseieved access to sendind messages");
                    }
                    UserIds receiver, caller;
                    chatHub.FindCallerReceiverByIds(viewModel.ReceiverId, id, out caller, out receiver);
                    bool dialogExists = await chatService.IsDialogExists(caller.userId, viewModel.ReceiverId);
                    if (dialogExists)
                    {
                        var message = await chatService.ShareLink(caller.userId, viewModel, DateTime.Now);
                        if (chatService.IsOnline(viewModel.ReceiverId))
                        {
                            await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                        else
                        {
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                        return Ok(message);
                    }
                    else
                    {
                        await chatService.CreateDialog(caller.userId, viewModel.ReceiverId);
                        var message = await chatService.ShareLink(caller.userId, viewModel, DateTime.Now);
                        if (receiver != null)
                        {
                            if (chatService.IsOnline(viewModel.ReceiverId))
                            {
                                await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                                await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                            }
                            else
                            {
                                await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                            }
                        }
                        return Ok(message);
                    }
                }
                else
                {
                    return BadRequest("You didn't choose the user to share");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Authorize]
        [Route("resendMessage")]
        public async Task<IActionResult> ResendMessage([FromForm]ResendMessageViewModel viewModel)
        {
            try
            {
                if (viewModel.MessageId != null)
                {
                    var id = User.Claims.First(c => c.Type == "Id").Value;
                    if (await blockListService.IsUserBlocked(id, viewModel.ReceiverId))
                    {
                        return BadRequest("Unlock this user to send messages");
                    }
                    if (await blockListService.IsUserBlocked(viewModel.ReceiverId, id))
                    {
                        return BadRequest("This user reseieved access to sendind messages");
                    }
                    UserIds receiver, caller;
                    chatHub.FindCallerReceiverByIds(viewModel.ReceiverId, id, out caller, out receiver);
                    bool dialogExists = await chatService.IsDialogExists(caller.userId, viewModel.ReceiverId);
                    if (dialogExists)
                    {
                        var message = await chatService.ResendMessage(caller.userId, viewModel, DateTime.Now);
                        if (chatService.IsOnline(viewModel.ReceiverId))
                        {
                            await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                        else
                        {
                            await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                        }
                        return Ok(message);
                    }
                    else
                    {
                        await chatService.CreateDialog(caller.userId, viewModel.ReceiverId);
                        var message = await chatService.ResendMessage(caller.userId, viewModel, DateTime.Now);
                        if (receiver != null)
                        {
                            if (chatService.IsOnline(viewModel.ReceiverId))
                            {
                                await hubContext.Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
                                await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                            }
                            else
                            {
                                await hubContext.Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
                            }
                        }
                        return Ok(message);
                    }
                }
                else
                {
                    return BadRequest("You didn't choose the message to resend");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("deleteDialog/{dialogId}")]
        public async Task DeleteDialog(int dialogId)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            await dialogService.DeleteDialog(id, dialogId);
        }
    }
}