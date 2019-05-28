using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class UserContact
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ContactId { get; set; }

        [NotMapped]
        public User User { get; set; }
        [NotMapped]
        public Contact Contact { get; set; }
    }
}
