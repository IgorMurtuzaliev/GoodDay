using GoodDay.BLL.Infrastructure;
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
        private IFileManager fileManager;
        public ChatService(UserManager<User> _userManager, IUnitOfWork _unitOfWork, IFileManager _fileManager)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
            fileManager = _fileManager;
        }
        public async Task<MessageViewModel> AddNewMessage(string senderId, PostMessageViewModel postMessage, DateTime time)
        {
            try
            {
                User user = await userManager.FindByIdAsync(senderId);
                var dialog = new Dialog();
                if (user.UsersDialogs.Any(c => c.User2Id == postMessage.ReceiverId || c.User1Id == postMessage.ReceiverId))
                {
                    var usersDialog = user.UsersDialogs.Single(c => c.User2Id == postMessage.ReceiverId || c.User1Id == postMessage.ReceiverId);
                    dialog = usersDialog;
                }
                if (user.InterlocutorsDialogs.Any(c => c.User2Id == postMessage.ReceiverId || c.User1Id == postMessage.ReceiverId))
                {
                    var interlocutorsDialog = user.InterlocutorsDialogs.Single(c => c.User2Id == postMessage.ReceiverId || c.User1Id == postMessage.ReceiverId);
                    dialog = interlocutorsDialog;
                }
                var newMessage = new Message
                {
                    DialogId = dialog.Id,
                    SenderId = senderId,
                    Text = postMessage.Text,
                    SendingTime = time,
                    Receiverid = postMessage.ReceiverId

                };
                await unitOfWork.Messages.Add(newMessage);
                await unitOfWork.Save();
                if (postMessage.Attachment != null)
                {
                    var files = await fileManager.UploadMessagesFiles(dialog.Id, newMessage.Id, postMessage.Attachment);
                }
                var messageVM = new MessageViewModel(newMessage);
                return messageVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateDialog(string userId, string friendId)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<MessageViewModel>> GetAllDialogMessages(string userId, string friendId)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<DialogViewModel>> GetAllDialogs(string userId)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DialogViewModel> GetDialog(string userId, string friendId)
        {
            try
            {
                User user = await userManager.FindByIdAsync(userId);
                var dialogList = user.UsersDialogs.Union(user.InterlocutorsDialogs);
                var dialog = dialogList.Single(c => (c.User2Id == friendId && c.User1Id == userId) || (c.User1Id == friendId && c.User2Id == userId));
                var dialogVM = new DialogViewModel(dialog, user);
                dialogVM.Messages = await GetAllDialogMessages(userId, friendId);
                return dialogVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> IsDialogExists(string senderId, string recevierId)
        {
            try
            {
                User user = await userManager.FindByIdAsync(senderId);
                return unitOfWork.Dialogs.UserHasDialog(user, recevierId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsOnline(string id)
        {
            if (UserIds.usersList.Any(c => c.userId == id))
            {
                return true;
            }
            else return false;
        }
    }
}
