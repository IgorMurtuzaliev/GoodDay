using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class Dialog
    {
        public Dialog()
        {

        }
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
       
        [NotMapped]
        public virtual User Sender { get; set; }
        [NotMapped]
        public virtual User Receiver { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
