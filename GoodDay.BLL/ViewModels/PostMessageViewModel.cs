using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class PostMessageViewModel
    {
        public string Text { get; set; }
        public string ReceiverId { get; set; }
        public IFormFileCollection Attachment { get; set; }
    }
}
