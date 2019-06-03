using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class ContactService : IContactService
    {
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        public ContactService(UserManager<User> _userManager, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
        }
        public async Task<Contact> AddContact(string id, string friendId)
        {
            User friend = await userManager.FindByIdAsync(friendId);
            var contact = new Contact
            {
                UserFriendId = friendId,
                UserId = id,
                Blocked = false,
                ContactName = friend.Email,
            };
            await unitOfWork.Contacts.Add(contact);            
            return contact;
        }

        public async Task<Contact> ChangeContactName(ContactDTO model)
        {
            Contact contact = await unitOfWork.Contacts.Get(model.Id);
            if( contact!= null)
            {
                contact.ContactName = model.ContactName;
            }
            await unitOfWork.Contacts.Edit(contact);
            await unitOfWork.Contacts.Save();
            return contact;
        }

        public async Task DeleteContact(int? id)
        {
           await unitOfWork.Contacts.Delete(id);

        }

        public async Task<Contact> GetContact(int? id)
        {
            return await unitOfWork.Contacts.Get(id);
        }

        public async Task<IEnumerable<Contact>> GetContacts(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            var contacts = user.Contacts;
            return contacts;
        }
    }
}
