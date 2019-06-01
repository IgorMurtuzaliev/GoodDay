using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class User:IdentityUser
    {
        public User()
        {
            Dialogs = new List<Dialog>();
            Contacts = new List<Contact>();
        }

        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }

        [Phone, Required(ErrorMessage = "Input you phone number")]
        public string Phone { get; set; }
        
        //public DateTime? LastLogin { get; set; }

        public int? FileId { get; set; }

        public virtual ICollection<Dialog> Dialogs { get; set; }
        public virtual ICollection<Contact> Contacts{ get; set; }

        [NotMapped]
        public virtual File File { get; set; }
    }
}
