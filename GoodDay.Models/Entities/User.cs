using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public DateTime LastLogin { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
