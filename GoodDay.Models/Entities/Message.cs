using System;
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
        public string SenderId { get; set; }
        public string Receiverid { get; set; }
        public int DialogId { get; set;}
        public int? FileId { get; set; }

        [NotMapped]
        public virtual User Sender { get; set; }
        [NotMapped]
        public virtual User Receiver { get; set; }
        [NotMapped]
        public virtual Dialog Dialog { get; set; }
        [NotMapped]
        public virtual File File { get; set; }

    }
}
