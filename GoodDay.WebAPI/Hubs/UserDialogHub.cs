using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI.Hubs
{

    public class UserDialogHub:Hub
    {
        public Task SendPrivateTweet(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveTweet", user, message);
        }
    }
}
