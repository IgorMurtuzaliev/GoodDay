using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI
{
    public class ChatHub: Hub
    {
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task Echo(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
    }
}
