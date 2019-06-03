using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GoodDay.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CurrentAvatar { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
