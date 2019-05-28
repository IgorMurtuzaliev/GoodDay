using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendingTime { get; set; }

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        public virtual User User { get; set; }

    }
}
