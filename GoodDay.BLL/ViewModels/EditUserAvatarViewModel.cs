using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class EditUserAvatarViewModel
    {
        public string Id { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
