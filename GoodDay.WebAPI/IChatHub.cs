using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI
{
    public interface IChatHub
    {
        Task OnConnectedAsync();
        void UpdateList(string callerId);
        void FindCallerReceiverByIds(string receiverId, out UserIds caller, out UserIds receiver);
        Task SendFaraway(string message, string receiverId);
        void Disconnect(string id);
        Task OnDisconnectedAsync(Exception exception);
        bool IsOnline(string id);
    }
}
