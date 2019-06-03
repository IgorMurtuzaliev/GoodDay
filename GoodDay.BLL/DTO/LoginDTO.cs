using System.ComponentModel.DataAnnotations;

namespace GoodDay.BLL.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Password { get; set; }
        
    }
}
