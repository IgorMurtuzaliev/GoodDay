using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
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
                FriendId = friendId,
                UserId = id,
                Blocked = false,
                ContactName = friend.Email,
            };
            try
            {
                await unitOfWork.Contacts.Add(contact);            
                return contact;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contact> ChangeContactName(ContactViewModel model)
        {
            Contact contact = await unitOfWork.Contacts.Get(model.Id);
            try
            {
               if( contact!= null)
                {
                    contact.ContactName = model.ContactName;
                }
                await unitOfWork.Contacts.Edit(contact);
                await unitOfWork.Contacts.Save();
                return contact;
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteContact(int? id)
        {
            try
            {
                await unitOfWork.Contacts.Delete(id);
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contact> GetContact(int? id)
        {
            return await unitOfWork.Contacts.Get(id);
        }

        public async Task<IEnumerable<Contact>> GetContacts(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            try
            {
                var contacts = user.UsersContacts;
                return contacts;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
