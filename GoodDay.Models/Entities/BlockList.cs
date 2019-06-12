using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class BlockList
    {
        public string Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Friend")]
        public string FriendId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
        [NotMapped]
        public virtual User Friend { get; set; }


    }
}
