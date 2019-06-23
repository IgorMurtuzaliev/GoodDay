using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class MessageViewModel
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string UserName { get; set; }
        public string ReceiverId { get; set; }
        public string MessageText { get; set; }
        public string TimeOfSending { get; set; }
        public string UserImage { get; set; }
        public string SharedUserId { get; set; }
        public string SharedUserName { get; set; }
        public string ResendUserFrom { get; set; }
        public ICollection<FileViewModel> FilePaths { get; set; }
        public MessageViewModel(Message message)
        {
            Id = message.Id.ToString();
            TimeOfSending = message.SendingTime.ToString("MM/dd/yyyy h:mm tt");
            if (!String.IsNullOrEmpty(message.SenderId))
            {
                SenderId = message.SenderId;
            }
            if (!String.IsNullOrEmpty(message.Receiverid))
            {
                ReceiverId = message.Receiverid;
            }
            if (!String.IsNullOrEmpty(message.Text))
            {
                MessageText = message.Text;
            }
            if (message.Files!= null)
            {
                var result = new List<FileViewModel>();
                foreach (var file in message.Files)
                {
                    result.Add(new FileViewModel(file));
                }
                FilePaths = result;
            }
            if (message.Sender != null)
            {
                UserName = message.Sender.UserName;
                if (message.Sender.FilePath != null)
                {
                    UserImage = message.Sender.FilePath;
                }
                else UserImage = "\\Shared\\user.png";
            }
            if (message.SharedUserId != null)
            {
                SharedUserId = message.SharedUserId;
                SharedUserName = message.SharedUserName;
            }
            if (message.SharedUserId != null)
            {
                SharedUserId = message.SharedUserId;
                SharedUserName = message.SharedUserName;
            }
            if (message.ResendUserFrom != null)
            {
                ResendUserFrom = message.ResendUserFrom;
            }
        }
    }
}
