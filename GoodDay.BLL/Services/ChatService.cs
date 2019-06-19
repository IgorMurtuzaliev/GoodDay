using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class ChatService : IChatService
    {
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        public ChatService(UserManager<User> _userManager, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
        }
        public async Task AddNewMessage(string senderId, string recevierId, string message, DateTime time)
        {
            User user = await userManager.FindByIdAsync(senderId);
            var dialog = user.UsersDialogs.Single(c => c.ReceiverId == recevierId);
            var newMessage = new Message
            {
                DialogId = dialog.Id,
                SenderId = senderId,
                Text = message,
                SendingTime = time
            };
            await unitOfWork.Messages.Add(newMessage);
            await unitOfWork.Save();
        }

        public async Task CreateDialog(string userId, string friendId)
        {
            User user = await userManager.FindByIdAsync(userId);
            User friend = await userManager.FindByIdAsync(friendId);
            var usersDialog = new Dialog
            {
                SenderId = userId,
                ReceiverId = friendId
            };
            var friendsDialog = new Dialog
            {
                SenderId = friendId,
                ReceiverId = userId
            };
            await unitOfWork.Dialogs.Add(usersDialog);
            if (!await IsDialogExists(friendId, userId))
            {
                await unitOfWork.Dialogs.Add(friendsDialog);
            }
        }

        public async Task<List<MessageViewModel>> GetAllDialogMessages(string userId, string friendId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var dialog = user.UsersDialogs.Single(c => c.ReceiverId == friendId);
            var result = new List<MessageViewModel>();
            var messagesList = dialog.Messages;
            foreach (var item in messagesList)
            {
                result.Add(new MessageViewModel(item));
            }
            return result;
        }

        public async Task<List<DialogViewModel>> GetAllDialogs(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var dialogList = user.UsersDialogs;
            var result = new List<DialogViewModel>();
            foreach(var item in dialogList)
            {
                result.Add( new DialogViewModel(item));
            }
            return result;
        }

        public async Task<DialogViewModel> GetDialog(string userId, string friendId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var dialog = user.UsersDialogs.Single(c => c.ReceiverId == friendId);
            return new DialogViewModel(dialog);

        }

        public async Task<bool> IsDialogExists(string senderId, string recevierId)
        {
            User user = await userManager.FindByIdAsync(senderId);
            return unitOfWork.Dialogs.UserHasDialog(user, recevierId);
        }
    }
}
