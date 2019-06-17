using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
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
        public DialogViewModel(Dialog dialog)
        {
            if (dialog != null)
            {
                { Id = dialog.Id; }
                if (!String.IsNullOrEmpty(dialog.ReceiverId))
                {
                    { FriendId = dialog.ReceiverId; }
                }
                if (dialog.Receiver!= null)
                {
                    { FriendName = dialog.Receiver.Name; }
                    { FriendSurname = dialog.Receiver.Surname; }
                    { FriendEmail = dialog.Receiver.Email; }
                    { FriendPhone = dialog.Receiver.Phone; }
                    if (dialog.Receiver.File != null)
                    {
                        { FriendImage = dialog.Receiver.File.Path; }
                    }
                    else FriendImage = "\\Shared\\user.png";
                }
            }
        }
    }
}
