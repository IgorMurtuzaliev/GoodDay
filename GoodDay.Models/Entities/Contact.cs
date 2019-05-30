using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class Contact
    {
        public int Id { get; set; }        
        public bool Blocked { get; set; }
        public string ContactName { get; set; }

        public string UserId { get; set; }
       
        public string UserFriendId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }

        
    }
}
