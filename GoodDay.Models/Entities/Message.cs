﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class Message
    {
        public Message()
        {

        }
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]
        public string Receiverid { get; set; }

        [ForeignKey("Dialog")]
        public int DialogId { get; set;}


        [NotMapped]
        public virtual User Sender { get; set; }
        [NotMapped]
        public virtual User Receiver { get; set; }
        [NotMapped]
        public virtual Dialog Dialog { get; set; }

        public string FilePaths { get; set; }

    }
}
