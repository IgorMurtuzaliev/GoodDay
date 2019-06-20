using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class Contact
    {
        public int Id { get; set; }        
        public bool Blocked { get; set; }
        public bool Confirmed { get; set; }
        public string ContactName { get; set; }

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
