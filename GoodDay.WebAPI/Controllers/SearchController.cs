using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private IChatService chatService;
        
        public SearchController(ISearchService _searchService, IUserService _userService, IContactService _contactService, IBlockListService _blockListService, IChatService _chatService)
        { 
            searchService = _searchService;
            userService = _userService;
            contactService = _contactService;
            blockListService = _blockListService;
            chatService = _chatService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> Search(string search)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
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
            return Ok(searchResult);
            }
        }
       
    }
}