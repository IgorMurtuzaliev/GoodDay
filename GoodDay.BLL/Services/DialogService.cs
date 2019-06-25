using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class DialogService : IDialogService
    {
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        private IFileManager fileManager;
        public DialogService(UserManager<User> _userManager, IUnitOfWork _unitOfWork, IFileManager _fileManager)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
            fileManager = _fileManager;
        }
        public async Task<Dialog> CreateDialog(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            User friend = await userManager.FindByIdAsync(friendId);
            var usersDialog = new Dialog
            {
                User1Id = id,
                User2Id = friendId
            };
            var friendsDialog = new Dialog
            {
                User1Id = friendId,
                User2Id = id
            };
            await unitOfWork.Dialogs.Add(usersDialog);
            if (!await HasUserDialog(friendId, id))
            {
                await unitOfWork.Dialogs.Add(friendsDialog);
            }
            return usersDialog;
        }
        public async Task<bool> HasUserDialog(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            return unitOfWork.Dialogs.UserHasDialog(user, friendId);
        }
        public async Task DeleteDialog(string userId, int dialogId)
        {
            Dialog dialog = await unitOfWork.Dialogs.FindDialog(dialogId);
            var isDialogDeleted = unitOfWork.DeletedDialogs.Check(dialogId);
            if (!isDialogDeleted)
            {
                var deletedDialog = new DeletedDialog
                {
                    DialogId = dialogId,
                    DeleteByUserId = userId,
                    IsDeleted = true,
                    TimeOfLastDeleting = DateTime.Now
                };
                await unitOfWork.DeletedDialogs.Add(deletedDialog);
            }
            else
            {
                if (!unitOfWork.DeletedDialogs.CheckForOutput(dialog.Id, userId))
                {
                    string userDeleted = "";
                    if(userId == dialog.User1Id)
                    {
                        userDeleted = dialog.User2Id;
                    }
                    if (userId == dialog.User2Id)
                    {
                        userDeleted = dialog.User1Id;
                    }
                    var delDialogWithFriend = unitOfWork.DeletedDialogs.GetForOutput(dialogId, userDeleted);
                    if (delDialogWithFriend.IsDeleted)
                    {
                        await unitOfWork.Dialogs.Delete(dialogId);
                        await unitOfWork.Dialogs.Save();
                        fileManager.DeleteDialogFiles(dialogId);
                        unitOfWork.DeletedDialogs.Delete(delDialogWithFriend.DialogId);
                    }
                    else
                    {
                        var deletedDialog = new DeletedDialog
                        {
                            DialogId = dialogId,
                            DeleteByUserId = userId,
                            IsDeleted = true,
                            TimeOfLastDeleting = DateTime.Now
                        };
                        await unitOfWork.DeletedDialogs.Add(deletedDialog);
                    }
                }
                else
                {
                    string userDeleted = "";
                    if (userId == dialog.User1Id)
                    {
                        userDeleted = dialog.User2Id;
                    }
                    if (userId == dialog.User2Id)
                    {
                        userDeleted = dialog.User1Id;
                    }
                    var delDialogWithFriend = unitOfWork.DeletedDialogs.GetForOutput(dialogId, userDeleted);
                    if (delDialogWithFriend.IsDeleted)
                    {
                        await unitOfWork.Dialogs.Delete(dialogId);
                        await unitOfWork.Dialogs.Save();
                        fileManager.DeleteDialogFiles(dialogId);
                        unitOfWork.DeletedDialogs.Delete(delDialogWithFriend.DialogId);
                    }
                    else
                    {
                    var delDialogOfClient = unitOfWork.DeletedDialogs.GetForOutput(dialogId, userId);
                    delDialogOfClient.IsDeleted = true;
                    await unitOfWork.DeletedDialogs.Edit(delDialogOfClient);
                    }

                }

            }
        }
    }
}
