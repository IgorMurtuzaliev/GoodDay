using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Input your password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Input new password"), MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
        public string NewPasswordConfirm { get; set; }
    }
}
