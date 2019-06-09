
using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IContactService
    {
        Task<Contact> AddContact(string id, string friendId);
        Task DeleteContact(int? id);
        Task<Contact> GetContact(int? id);
        Task<IEnumerable<Contact>> GetContacts(string id);
        Task<Contact> ChangeContactName(ContactViewModel model);
        Task<bool> UserHasContact(string friendId, string id);
    }
}
