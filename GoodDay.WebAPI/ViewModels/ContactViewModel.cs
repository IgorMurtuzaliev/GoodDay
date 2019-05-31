﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI.ViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public bool Blocked { get; set; }
        public string ContactName { get; set; }
        public string UserId { get; set; }
        public string UserFriendId { get; set; }
    }
}