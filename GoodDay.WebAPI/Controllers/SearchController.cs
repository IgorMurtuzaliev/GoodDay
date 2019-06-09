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
        private ISearchService searchService;
        public SearchController(ISearchService _searchService)
        {
            searchService = _searchService;
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
                    result.Add(new UserViewModel(item));

                }
            return result;
            }
        }
       
    }
}