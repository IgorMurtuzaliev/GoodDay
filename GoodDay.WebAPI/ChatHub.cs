using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI
{
    public class ChatHub: Hub
    {
        public void Echo(string message)
        {
            Clients.All.SendAsync("Send", message);
        }
    }
}
