using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IUserService userService;
        private ISearchService searchService;
        private IContactService contactService;
        private IBlockListService blockListService;
        
        public SearchController(ISearchService _searchService, IUserService _userService, IContactService _contactService, IBlockListService _blockListService)
        { 
            searchService = _searchService;
            userService = _userService;
            contactService = _contactService;
            blockListService = _blockListService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> Search(string search)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            var result = new List<UserViewModel>();
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("The search string is empty");
            }
            else
            {
                var searchResult = await searchService.Search(id, search);
                if(searchResult == null)
                {
                    return BadRequest("Not found by your response");
                }
                foreach(var item in searchResult)
                {
                    var profile = new UserViewModel(item);
                    result.Add(profile);
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
        }
       
    }
}