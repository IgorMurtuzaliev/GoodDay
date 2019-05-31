using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodDay.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Input your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input your surname")]
        public string Surname { get; set; }
    }
}
