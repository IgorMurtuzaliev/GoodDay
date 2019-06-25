using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodDay.Models.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            UsersDialogs = new List<Dialog>();
            InterlocutorsDialogs = new List<Dialog>();
            UsersContacts = new List<Contact>();
            UserInContacts = new List<Contact>();
            UsersBlockList = new List<BlockList>();
            UserInBlockList = new List<BlockList>();
        }

        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Input you phone number")]
        public string Phone { get; set; }
        public DateTime LastTimeOnline { get; set; }
        public string FilePath { get; set; }

        [JsonIgnore]
        public virtual ICollection<Dialog> UsersDialogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Dialog> InterlocutorsDialogs { get; set; }
        [JsonIgnore]
        public virtual ICollection<Contact> UsersContacts { get; set; }
        [JsonIgnore]
        public virtual ICollection<Contact> UserInContacts { get; set; }
        [JsonIgnore]
        public virtual ICollection<BlockList> UsersBlockList { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }
        [JsonIgnore]
        public virtual ICollection<BlockList> UserInBlockList { get; set; }

    }
}
