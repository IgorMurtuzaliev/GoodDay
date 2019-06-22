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
    public class UserIds
    {
        public string userId;
        public string connectionId;
    }
    public class ChatHub : Hub, IChatHub
    {
        private IChatService chatService;
        public ChatHub(IChatService _chatService)
        {
            chatService = _chatService;
        }
        public static List<UserIds> usersList = new List<UserIds>();

        public async override Task OnConnectedAsync()
        {
            var id = Context.User.Claims.First(c => c.Type == "Id").Value;
            UpdateList(id);
            await base.OnConnectedAsync();
        }

        void UpdateList(string callerId)
        {
            var index = usersList.FindIndex(i => i.userId == callerId);
            if (index != -1 && usersList[index].connectionId != Context.ConnectionId)
            {
                usersList[index].connectionId = Context.ConnectionId;
            }
            else
            {
                usersList.Add(new UserIds { connectionId = Context.ConnectionId, userId = callerId });
            }
        }
        void FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver)
        {
            receiver = usersList.Find(i => i.userId == receiverId);
            caller = usersList.Find(i => i.userId == id);
        }
        //public async Task SendFaraway(PostMessageViewModel postMessage, string id)
        //{
        //    UserIds receiver, caller;
        //    FindCallerReceiverByIds(postMessage.ReceiverId ,id,  out caller, out receiver);
        //    bool dialogExists = await chatService.IsDialogExists(caller.userId, postMessage.ReceiverId);
        //    if (dialogExists)
        //    {
        //        await chatService.AddNewMessage(caller.userId, postMessage.ReceiverId, postMessage.Text, DateTime.Now);
        //        await Clients.Clients(caller.connectionId).SendAsync("SendMyself", postMessage.Text);
        //        await Clients.Client(receiver.connectionId).SendAsync("Send", postMessage.Text, caller.userId);
        //    }
        //    else
        //    {
        //        await chatService.CreateDialog(caller.userId, postMessage.ReceiverId);
        //        await chatService.AddNewMessage(caller.userId, postMessage.ReceiverId, postMessage.Text, DateTime.Now);
        //        if (receiver != null)
        //        {
        //            await Clients.Clients(caller.connectionId).SendAsync("SendMyself", postMessage.Text);
        //            await Clients.Client(receiver.connectionId).SendAsync("Send", postMessage.Text, caller.userId);
        //        }
        //    }
        //}
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            usersList.Remove(usersList.Find(c => c.connectionId == Context.ConnectionId));
            await base.OnDisconnectedAsync(exception);
        }
        public void Disconnect(string id)
        {
            if (usersList.Any(c => c.userId == id))
            {
                usersList.Remove(usersList.Find(c => c.userId == id));
            }

        }
        public bool IsOnline(string id)
        {
            if (usersList.Any(c => c.userId == id))
            {
                return true;
            }
            else return false;
        }
        void IChatHub.UpdateList(string callerId)
        {
            throw new NotImplementedException();
        }

        void IChatHub.FindCallerReceiverByIds(string receiverId, string id, out UserIds caller, out UserIds receiver)
        {
            receiver = usersList.Find(i => i.userId == receiverId);
            caller = usersList.Find(i => i.userId == id);
        }
    }
}
