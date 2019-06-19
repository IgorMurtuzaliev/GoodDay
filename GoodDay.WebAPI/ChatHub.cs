﻿using GoodDay.BLL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI
{
    public class UserIds
    {
        public string userId;
        public string connectionId;
    }
    public class ChatHub: Hub
    {
        private IAccountService accountService;
        private IChatService chatService;
        private UserManager<User> userManager;
        public ChatHub(IAccountService _accountService, IChatService _chatService, UserManager<User> _userManager)
        {
            accountService = _accountService;
            chatService = _chatService;
            userManager = _userManager;
        }
        //[Authorize(AuthenticationSchemes = "Bearer")]
        static List<UserIds> usersList = new List<UserIds>();

        public async override Task OnConnectedAsync()
        {
            var id = Context.User.Claims.First(c => c.Type == "Id").Value;
            //User user = await userManager.FindByIdAsync(id);
            var callerId = id;
            UpdateList(callerId);
            await base.OnConnectedAsync();
        }
        public async Task Send(string message, string receiverId)
        {
            UserIds receiver, caller;
            FindCallerReceiverByIds(receiverId, out caller, out receiver);
            //await chatService.AddNewMessage(caller.userId, receiverId, message, DateTime.Now);
            await Clients.Clients(caller.connectionId).SendAsync("SendMyself", message);
            if (receiver != null)
            {
                await Clients.Clients(receiver.connectionId).SendAsync("Send", message, caller.userId);
            }
            
        }
        void UpdateList(string callerId)
        {
            var index = usersList.FindIndex(i => i.userId == callerId);
            if(index!=-1 && usersList[index].connectionId != Context.ConnectionId)
            {
                usersList[index].connectionId = Context.ConnectionId;
            }
            else
            {
                usersList.Add(new UserIds { connectionId = Context.ConnectionId, userId = callerId });
            }
        }
        void FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver)
        {
            receiver = usersList.Find(i => i.userId == receiverId);
            caller = usersList.Find(i => i.connectionId == Context.ConnectionId);
        }
        public async Task SendFaraway(string message, string receiverId)
        {
            UserIds receiver, caller;
            FindCallerReceiverByIds(receiverId, out caller, out receiver);
            bool dialogExists = await chatService.IsDialogExists(caller.userId, receiverId);
            if (dialogExists)
            {
                await chatService.AddNewMessage(caller.userId, receiverId, message, DateTime.Now);
                await Clients.Client(receiver.connectionId).SendAsync("Send", message, caller.userId);
            }
            else
            {
                await chatService.CreateDialog(receiverId, caller.userId);
                await chatService.AddNewMessage(caller.userId, receiverId, message, DateTime.Now);
                var chatRoomVM = await chatService.GetDialog(caller.userId, receiverId);
                var res = JsonConvert.SerializeObject(chatRoomVM);
                if (receiver != null)
                {
                    await Clients.Client(receiver.connectionId).SendAsync("AddNewDialog", message, res);
                }
            }
            
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            usersList.Remove(usersList.Find(c => c.connectionId == Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }
    }
}
