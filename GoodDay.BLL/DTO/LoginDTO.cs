using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodDay.BLL.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Input your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Input your password"), MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
