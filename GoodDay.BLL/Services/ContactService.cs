using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class ContactService : IContactService
    {
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        private ApplicationDbContext dbContext;
        public ContactService(UserManager<User> _userManager, IUnitOfWork _unitOfWork, ApplicationDbContext _dbContext)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
            dbContext = _dbContext;
        }
        public async Task<Contact> AddContact(string id, string friendId)
        {
            try
            {
                User friend = await userManager.FindByIdAsync(friendId);
                User user = await userManager.FindByIdAsync(id);

                var contact = new Contact
                {
                    FriendId = friendId,
                    UserId = id,
                    Blocked = false,
                    Confirmed = true,
                    ContactName = friend.Name + " " + friend.Surname,
                };

                var anotherContact = new Contact
                {
                    FriendId = id,
                    UserId = friendId,
                    Blocked = false,
                    Confirmed = false,
                    ContactName = user.Name + "" + user.Surname,
                };
                await unitOfWork.Contacts.Add(contact);
                if (!unitOfWork.Contacts.IsUserInContact(friendId, id)) { await unitOfWork.Contacts.Add(anotherContact); }

                return contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contact> ChangeContactName(Contact contact, EditContactViewModel model)
        {
            try
            {
                if (contact != null)
                {
                    contact.ContactName = model.ContactName;
                }
                await unitOfWork.Contacts.Edit(contact);
                await unitOfWork.Contacts.Save();
                return contact;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ConfirmContact(int? id)
        {
            try
            {
                var contact = await unitOfWork.Contacts.Get(id);
                contact.Confirmed = true;
                await unitOfWork.Contacts.Edit(contact);
                await unitOfWork.Contacts.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task BlockContact(int? id)
        {
            try
            {
                var contact = await unitOfWork.Contacts.Get(id);
                contact.Blocked = true;
                await unitOfWork.Contacts.Edit(contact);
                await unitOfWork.Contacts.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UnlockContact(int? id)
        {
            try
            {
                var contact = await unitOfWork.Contacts.Get(id);
                contact.Blocked = false;
                await unitOfWork.Contacts.Edit(contact);
                await unitOfWork.Contacts.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Contact> GetContact(int? id)
        {
            return await unitOfWork.Contacts.Get(id);
        }

        public async Task<Contact> FindContact(string id, string friendId)
        {
            return await unitOfWork.Contacts.FindContact(id, friendId);
        }

        public async Task<IEnumerable<Contact>> GetContacts(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            try
            {
                var contacts = user.UsersContacts;
                return contacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UserHasContact(string friendId, string id)
        {
            User user = await userManager.FindByIdAsync(id);
            var userHasContact = dbContext.Contacts.Where(c => c.FriendId == friendId && c.UserId == id).Count();
            if (userHasContact == 0) return true;
            else return false;
        }
    }
}
