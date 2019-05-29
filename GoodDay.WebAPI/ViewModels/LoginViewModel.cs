using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Input your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Input your password"), MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
