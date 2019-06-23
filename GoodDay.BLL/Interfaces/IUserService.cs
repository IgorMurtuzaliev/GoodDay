using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> ShowUsersProfile(string id, string friendId);
        bool UserExists(string id);

    }
}
