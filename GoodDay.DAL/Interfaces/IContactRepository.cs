using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> Get(int? id);
        Task Add(Contact item);
        Task Edit(Contact item);
        Task Delete(int? id);
        bool IsUserInContact(User user, string friendId);
        Contact FindContact(User user, string friendId);
        Task Save();
    }
}
