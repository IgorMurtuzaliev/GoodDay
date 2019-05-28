using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodDay.BLL.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Input your email")]
        public string Email { get; set; }
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
