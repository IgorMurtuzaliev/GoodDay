using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GoodDay.WebAPI.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        public string CurrentAvatar { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
