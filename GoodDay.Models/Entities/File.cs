using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class File
    {
        public File()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
        [NotMapped]
        public virtual Message Message { get; set; }
    }
}
