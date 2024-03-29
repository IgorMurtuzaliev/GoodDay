﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string FileId { get; set; }
    }
}
