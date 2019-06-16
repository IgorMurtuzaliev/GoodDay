using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;
        private IContactService contactService;
        private UserManager<User> userManager;
        public UserService(IUnitOfWork _unitOfWork, IContactService _contactService, UserManager<User> _userManager)
        {
            unitOfWork = _unitOfWork;
            contactService = _contactService;
            userManager = _userManager;
        }

        public async Task<User> ShowUsersProfile(string id)
        {
            try
            {
                User user = await unitOfWork.Users.Get(id);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UserExists(string id)
        {
            return unitOfWork.Users.UserExists(id);
        }
        public async Task BlockUser(string id,string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            if (await IsInContacts(id, friendId))
            {
                var contact = unitOfWork.Contacts.FindContact(user, friendId);
                if (contact != null)
                {
                    await contactService.DeleteContact(contact.Id);
                }
            }

            try
            {
                BlockList block = new BlockList
                {
                    UserId = id,
                    FriendId = friendId
                };
                await unitOfWork.Blocks.Add(block);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async Task UnlockUser(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            var block = unitOfWork.Blocks.BlockedUser(user, friendId);
            await unitOfWork.Blocks.Delete(block);
        }
        public async Task<bool> IsUserBlocked(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            if (unitOfWork.Blocks.IsUserBlocked(user, friendId)) return true;
            else return false;
        }
        public async Task<bool> IsInContacts(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            if (unitOfWork.Contacts.IsUserInContact(user, friendId)) return true;
            else return false;
        }

    }
}
