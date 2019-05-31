using GoodDay.BLL.DTO;
using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IUserService
    {
        Task EditClientProfile(UserDTO model);
        Task<User> ShowUsersProfile(string id);
        bool UserExists(string id);
    }
}
