﻿using System.ComponentModel.DataAnnotations;

namespace GoodDay.BLL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Input your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Input your phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Input your password"), MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}