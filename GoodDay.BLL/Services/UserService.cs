using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using System;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;
        private IContactService contactService;
        public UserService( IUnitOfWork _unitOfWork, IContactService _contactService)
        {
            unitOfWork = _unitOfWork;
            contactService = _contactService;
        }

        public async Task<User> ShowUsersProfile(string id)
        {
            try
            {
                User user = await unitOfWork.Users.Get(id);
                return user;
            }
            catch(Exception ex)
            {
                throw ex;
            }          
        }
        
        public bool UserExists(string id)
        {
            return unitOfWork.Users.UserExists(id);
        }
        public async Task BlockUser(string id, string friendId)
        {
            if (IsInContacts(id, friendId))
            {
                 var contact = await unitOfWork.Contacts.FindContact(id, friendId);
                if (contact != null)
                {
                    await contactService.DeleteContact(contact.Id);
                }
            }
           
            BlockList block = new BlockList
            {
                UserId = id,
                FriendId = friendId
            };
            await unitOfWork.Blocks.Add(block);
        }
        public async Task UnlockUser(string id, string friendId)
        {
            var block = await unitOfWork.Blocks.BlockedUser(id, friendId);
            await unitOfWork.Blocks.Delete(block);
        }
        public bool IsUserBlocked(string id, string friendId)
        {
            if (unitOfWork.Blocks.IsUserBlocked(id, friendId)) return true;
            else return false;
        }
        public bool IsInContacts(string id, string friendId)
        {
            if (unitOfWork.Contacts.IsUserInContact(id, friendId)) return true;
            else return false;
        }

    }
}
