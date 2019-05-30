using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.DAL.Repositories;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task DeleteContact(int? id)
        {
           await unitOfWork.Contacts.Delete(id);

        }

        public async Task<Contact> GetContact(int? id)
        {
            return await unitOfWork.Contacts.Get(id);
        }
    }
}
