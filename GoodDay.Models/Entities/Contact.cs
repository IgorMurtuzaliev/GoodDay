using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string FriendId { get; set; }
        public bool Blocked { get; set; }

        [NotMapped]
        public virtual User Friend { get; set; }
        public virtual ICollection<UserContact> UserContacts { get; set; }
    }
}
