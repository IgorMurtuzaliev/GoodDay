using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAll();
        Task<Contact> Get(int? id);
        Task Add(Contact item);
        Task Edit(Contact item);
        Task Delete(int? id);
        bool IsUserInContact(string id, string friendId);
        Task<Contact> FindContact(string id, string friendId);
        Task Save();
    }
}
