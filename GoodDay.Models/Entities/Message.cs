using System;
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
        public string SharedUserId { get; set; }
        public string SharedUserName { get; set; }
        public string ResendUserFrom { get; set; }
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
        [NotMapped]
        public virtual ICollection<File> Files { get; set; }

    }
}
