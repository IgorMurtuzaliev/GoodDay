using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.DAL.EF;
using GoodDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class SearchService : ISearchService
    {
        private ApplicationDbContext dbContext;
        private IContactService contactService;
        private IBlockListService blockListService;
        private IChatService chatService;
        public SearchService(ApplicationDbContext _dbContext, IChatService _chatService, IContactService _contactService, IBlockListService _blockListService)
        {
            dbContext = _dbContext;
            chatService = _chatService;
            contactService = _contactService;
            blockListService = _blockListService;
        }
        public async Task<IEnumerable<UserViewModel>> Search(string id, string search)
        {
            try
            {
                var result = new List<UserViewModel>();
                var users = await dbContext.Users.Where(p => p.Email.Contains(search) || p.Phone.Contains(search)).Where(p => p.Id != id).ToListAsync();
                foreach (var item in users)
                {
                    var profile = new UserViewModel(item);
                    result.Add(profile);
                    if (chatService.IsOnline(item.Id))
                    {
                        profile.IsOnline = true;
                    }
                    else
                    {
                        profile.LastTimeOnline = item.LastTimeOnline.ToString("MM/dd/yyyy h:mm tt");
                        profile.IsOnline = false;
                    }
                    if (await blockListService.IsUserBlocked(id, item.Id))
                    {
                        profile.IsBlocked = true;
                    }
                    else profile.IsBlocked = false;
                    if (await contactService.IsInContacts(id, item.Id))
                    {
                        var contact = await contactService.FindContact(id, item.Id);
                        profile.ContactWithUserId = contact.Id;
                        profile.IsInContacts = true;
                    }
                    else profile.IsInContacts = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
