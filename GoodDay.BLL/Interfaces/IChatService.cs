using GoodDay.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IChatService
    {
        Task<List<DialogViewModel>> GetAllDialogs(string userId);
        Task<List<MessageViewModel>> GetAllDialogMessages(string userId, string friendId);
        Task AddNewMessage(string senderId, string recevierId, string message, DateTime time);
        Task<bool> IsDialogExists(string senderId, string recevierId);
        Task<DialogViewModel> GetDialog(string userId, string friendId);
        Task CreateDialog(string userId, string friendId);
    }
}
