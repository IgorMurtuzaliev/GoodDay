using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class User:IdentityUser
    {
        public User()
        {
            UsersDialog = new List<Dialog>();
            InterlocutorsDialog = new List<Dialog>();
            UsersContacts = new List<Contact>();
            UserInContacts = new List<Contact>();
        }

        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }

        [Phone, Required(ErrorMessage = "Input you phone number")]
        public string Phone { get; set; }
        
        //public DateTime? LastLogin { get; set; }

        public int? FileId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Dialog> UsersDialog { get; set; }
        [JsonIgnore]
        public virtual ICollection<Dialog> InterlocutorsDialog { get; set; }
        [JsonIgnore]
        public virtual ICollection<Contact> UsersContacts { get; set; }
        [JsonIgnore]
        public virtual ICollection<Contact> UserInContacts { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual File File { get; set; }
    }
}
