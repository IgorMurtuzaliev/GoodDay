using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IDialogRepository
    {
        Task<IEnumerable<Dialog>> GetAll();
        Dialog GetDialog(User user, string friendId);
        Task Add(Dialog item);
        bool UserHasDialog(User user, string friendId);
        Task Delete(int? id);
        Task Save();
    }
}
