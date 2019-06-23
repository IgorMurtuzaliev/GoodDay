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
        Task<MessageViewModel> AddNewMessage(string senderId,  PostMessageViewModel postMessage, DateTime time);
        Task<MessageViewModel> ShareLink(string senderId, ShareUserMessageViewModel viewModel, DateTime time);
        Task<MessageViewModel> ResendMessage(string senderId, ResendMessageViewModel viewModel, DateTime time);
        Task<bool> IsDialogExists(string senderId, string recevierId);
        Task<DialogViewModel> GetDialog(string userId, string friendId);
        Task CreateDialog(string userId, string friendId);
        bool IsOnline(string id);
    }
}
