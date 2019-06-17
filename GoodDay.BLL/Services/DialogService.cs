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
    public class DialogService: IDialogService
    {
        private UserManager<User> userManager;
        private IDialogRepository dialogRepository;
        public DialogService(UserManager<User> _userManager, IDialogRepository _dialogRepository)
        {
            userManager = _userManager;
            dialogRepository = _dialogRepository;
        }
        public  async Task<Dialog> CreateDialog(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            User friend = await userManager.FindByIdAsync(friendId);
            var usersDialog = new Dialog
            {
                SenderId = id,
                ReceiverId = friendId
            };
            var friendsDialog = new Dialog
            {
                SenderId = friendId,
                ReceiverId = id
            };
            await dialogRepository.Add(usersDialog);
            if(!await HasUserDialog(friendId, id))
            {
                await dialogRepository.Add(friendsDialog);
            }
            return usersDialog;
        }
        public async Task<bool> HasUserDialog(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            return dialogRepository.UserHasDialog(user, friendId);
        }

    }
}
