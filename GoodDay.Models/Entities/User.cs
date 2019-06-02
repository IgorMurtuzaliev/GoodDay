using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GoodDay.Models.Entities
{
    public class User:IdentityUser
    {
        public User()
        {

        }

        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }

        [Phone, Required(ErrorMessage = "Input you phone number")]
        public string Phone { get; set; }
        
        //public DateTime? LastLogin { get; set; }

        public int? FileId { get; set; }

        public virtual ICollection<Dialog> ResponsingForDialog { get; set; }
        public virtual ICollection<Dialog> RequestingToDialog { get; set; }
        public virtual ICollection<Contact> UsersContacts{ get; set; }
        public virtual ICollection<Contact> UserInContact { get; set; }
        [NotMapped]
        public virtual File File { get; set; }
    }
}
