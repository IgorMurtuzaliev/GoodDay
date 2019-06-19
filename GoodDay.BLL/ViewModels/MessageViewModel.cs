using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class MessageViewModel
    {
        private int Id { get; set; }
        private string  SenderId { get; set; }
        private string ReceiverId { get; set; }
        private string MessageText { get; set; }
        private DateTime TimeOfSending { get; set; }
        public string FilePath { get; set; }
        public MessageViewModel(Message message)
        {

        }
    }
}
