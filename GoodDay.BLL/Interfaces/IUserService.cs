using GoodDay.Models.Entities;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> ShowUsersProfile(string id);
        bool UserExists(string id);
        Task BlockUser(string id, string friendId);
        Task<bool> IsInContacts(string id, string friendId);
        Task<bool> IsUserBlocked(string id, string friendId);
        Task UnlockUser(string id, string friendId);
    }
}
