using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class Dialog
    {
        public Dialog()
        {

        }
        public int Id { get; set; }

        [ForeignKey("User1")]
        public string User1Id { get; set; }

        [ForeignKey("User2")]
        public string User2Id { get; set; }
       
        [NotMapped]
        public virtual User User1 { get; set; }
        [NotMapped]
        public virtual User User2 { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
