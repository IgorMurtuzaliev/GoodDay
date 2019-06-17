using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class BlockListService:IBlockListService
    {
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        private IContactService contactService;
        public BlockListService(UserManager<User> _userManager, IUnitOfWork _unitOfWork, IContactService _contactService)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
            contactService = _contactService;
        }
        public async Task<IEnumerable<BlockList>> GetBlockList(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            try
            {
                var blocks = user.UsersBlockList;
                return blocks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task BlockUser(string id, string friendId)
        {
            User user = await userManager.FindByIdAsync(id);
            if (await contactService.IsInContacts(id, friendId))
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
    }
}
