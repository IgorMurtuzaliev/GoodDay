using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
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
        private IChatService chatService;
        private IBlockListService blockListService;
        public UserService(IUnitOfWork _unitOfWork, IContactService _contactService, UserManager<User> _userManager, IChatService _chatService, IBlockListService _blockListService)
        {
            unitOfWork = _unitOfWork;
            contactService = _contactService;
            userManager = _userManager;
            chatService = _chatService;
            blockListService = _blockListService;
        }

        public async Task<UserViewModel> ShowUsersProfile(string id, string friendId)
        {
            try
            {
                User user = await userManager.FindByIdAsync(id);
                var profile = new UserViewModel(user);
                if (chatService.IsOnline(friendId))
                {
                    profile.IsOnline = true;
                }
                else
                {
                    profile.IsOnline = false;
                    profile.LastTimeOnline = user.LastTimeOnline.ToString("MM/dd/yyyy h:mm tt");
                }
                if (await blockListService.IsUserBlocked(id, friendId))
                {
                    profile.IsBlocked = true;
                }
                else profile.IsBlocked = false;
                if (await contactService.IsInContacts(id, friendId))
                {
                    var contact = await contactService.FindContact(id, friendId);
                    profile.ContactWithUserId = contact.Id;
                    profile.IsInContacts = true;
                }
                else profile.IsInContacts = false;
                return profile;
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

    }
}
