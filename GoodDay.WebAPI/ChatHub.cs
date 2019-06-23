using GoodDay.BLL.Infrastructure;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
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

    public class ChatHub : Hub, IChatHub
    {
        private IChatService chatService;
        public ChatHub(IChatService _chatService)
        {
            chatService = _chatService;
        }

        public async override Task OnConnectedAsync()
        {
            var id = Context.User.Claims.First(c => c.Type == "Id").Value;
            UpdateList(id);
            await base.OnConnectedAsync();
        }

        void UpdateList(string callerId)
        {
            var index = UserIds.usersList.FindIndex(i => i.userId == callerId);
            if (index != -1 && UserIds.usersList[index].connectionId != Context.ConnectionId)
            {
                UserIds.usersList[index].connectionId = Context.ConnectionId;
            }
            else
            {
                UserIds.usersList.Add(new UserIds { connectionId = Context.ConnectionId, userId = callerId });
            }
        }
        void FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver)
        {
            receiver = UserIds.usersList.Find(i => i.userId == receiverId);
            caller = UserIds.usersList.Find(i => i.userId == id);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            UserIds.usersList.Remove(UserIds.usersList.Find(c => c.connectionId == Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }
        public void Disconnect(string id)
        {
            if (UserIds.usersList.Any(c => c.userId == id))
            {
                UserIds.usersList.Remove(UserIds.usersList.Find(c => c.userId == id));
            }

        }
        void IChatHub.UpdateList(string callerId)
        {
            throw new NotImplementedException();
        }

        void IChatHub.FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver)
        {
            receiver = UserIds.usersList.Find(i => i.userId == receiverId);
            caller = UserIds.usersList.Find(i => i.userId == id);
        }
    }
}
