using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class DeletedDialog
    {
        public int Id { get; set; }
        public int DialogId { get; set; }
        public string DeleteByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeOfLastDeleting { get; set; }
    }
}
