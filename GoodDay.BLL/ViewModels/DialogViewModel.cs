using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class DialogViewModel
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string FriendName { get; set; }
        public string FriendSurname { get; set; }
        public string FriendEmail { get; set; }
        public string FriendPhone { get; set; }
        public string FriendImage { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageSenderImage { get; set; }

        public List<MessageViewModel> Messages { get; set; }
        public DialogViewModel(Dialog dialog, User user)
        {
            if (dialog != null)
            {
                Id = dialog.Id;
                if (user.Id == dialog.User1Id)
                {
                    if (!String.IsNullOrEmpty(dialog.User2Id))
                    {
                        FriendId = dialog.User2Id;
                    }
                    if (dialog.User2 != null)
                    {
                        FriendName = dialog.User2.Name;
                        FriendSurname = dialog.User2.Surname;
                        FriendEmail = dialog.User2.Email;
                        FriendPhone = dialog.User2.Phone;
                        if (dialog.User2.FilePath != null)
                        {
                            FriendImage = dialog.User2.FilePath;
                        }
                        else FriendImage = "\\Shared\\user.png";
                    }

                }
            }
            if (user.Id == dialog.User2Id)
            {
                if (!String.IsNullOrEmpty(dialog.User1Id))
                {
                    FriendId = dialog.User1Id;
                }
                if (dialog.User1 != null)
                {
                    FriendName = dialog.User1.Name;
                    FriendSurname = dialog.User1.Surname;
                    FriendEmail = dialog.User1.Email;
                    FriendPhone = dialog.User1.Phone;
                    if (dialog.User1.FilePath != null)
                    {
                        FriendImage = dialog.User1.FilePath;
                    }
                    else FriendImage = "\\Shared\\user.png";
                }
            }
            if (dialog.Messages.Count != 0)
            {
                var lastmessage = dialog.Messages.LastOrDefault();
                LastMessage = lastmessage.Text;
                if (lastmessage.Sender.FilePath != null)
                {
                    LastMessageSenderImage = lastmessage.Sender.FilePath;
                }
                else LastMessageSenderImage = "\\Shared\\user.png";
            }
        }
    }
}

