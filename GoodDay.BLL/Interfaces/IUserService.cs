using GoodDay.Models.Entities;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> ShowUsersProfile(string id);
        bool UserExists(string id);

    }
}
