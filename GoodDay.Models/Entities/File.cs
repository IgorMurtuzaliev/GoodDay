using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        public string UserId { get; set; }
        public int MessageId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
        [NotMapped]
        public virtual Message Message { get; set; }
    }
}
