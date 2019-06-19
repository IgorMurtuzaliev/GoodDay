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
            try
            {
                User user = await userManager.FindByIdAsync(senderId);
                var dialog = new Dialog();
                if (user.UsersDialogs.Any(c => c.User2Id == recevierId || c.User1Id == recevierId))
                {
                    var usersDialog = user.UsersDialogs.Single(c => c.User2Id == recevierId || c.User1Id == recevierId);
                    dialog = usersDialog;
                }
                if (user.InterlocutorsDialogs.Any(c => c.User2Id == recevierId || c.User1Id == recevierId))
                {
                    var interlocutorsDialog = user.InterlocutorsDialogs.Single(c => c.User2Id == recevierId || c.User1Id == recevierId);
                    dialog = interlocutorsDialog;
                }
                var newMessage = new Message
                {
                    DialogId = dialog.Id,
                    SenderId = senderId,
                    Text = message,
                    SendingTime = time,
                    Receiverid = recevierId
                };
                await unitOfWork.Messages.Add(newMessage);
                await unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateDialog(string userId, string friendId)
        {
            User user = await userManager.FindByIdAsync(userId);
            User friend = await userManager.FindByIdAsync(friendId);
            var usersDialog = new Dialog
            {
                User1Id = userId,
                User2Id = friendId
            };
            await unitOfWork.Dialogs.Add(usersDialog);
        }

        public async Task<List<MessageViewModel>> GetAllDialogMessages(string userId, string friendId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var dialogList = user.UsersDialogs.Union(user.InterlocutorsDialogs);
            var dialog = dialogList.Single(c => (c.User2Id == friendId && c.User1Id == userId) || (c.User1Id == friendId && c.User2Id == userId));
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
            var dialogList = user.UsersDialogs.Union(user.InterlocutorsDialogs);
            var result = new List<DialogViewModel>();
            foreach (var item in dialogList)
            {
                var dialogVM = new DialogViewModel(item, user);
                result.Add(dialogVM);
                var messagesList = item.Messages;
                var messagesListVM = new List<MessageViewModel>();
                foreach (var message in messagesList)
                {
                    var messageVM = new MessageViewModel(message);
                    messagesListVM.Add(messageVM);
                }
                dialogVM.Messages = messagesListVM;
            }
            return result;
        }
        //public async Task<DialogViewModel> GetDialog(string userId, string friendId)
        //{
        //    User user = await userManager.FindByIdAsync(userId);
        //    var dialog = 
        //}

        public async Task<DialogViewModel> GetDialog(string userId, string friendId)
        {
            try
            {
                User user = await userManager.FindByIdAsync(userId);
                var dialogList = user.UsersDialogs.Union(user.InterlocutorsDialogs);
                var dialog = dialogList.Single(c => (c.User2Id == friendId && c.User1Id == userId) || (c.User1Id == friendId && c.User2Id == userId));
                var dialogVM = new DialogViewModel(dialog, user);
                dialogVM.Messages =  await GetAllDialogMessages(userId, friendId);
                return dialogVM;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> IsDialogExists(string senderId, string recevierId)
        {
            User user = await userManager.FindByIdAsync(senderId);
            return unitOfWork.Dialogs.UserHasDialog(user, recevierId);
        }
    }
}
