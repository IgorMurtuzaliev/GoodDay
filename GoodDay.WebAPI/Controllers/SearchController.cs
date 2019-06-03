using System.Collections.Generic;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
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
        public async Task<ActionResult<IEnumerable<User>>> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("The search string is empty");
            }
            else
            {
                var searchResult = await searchService.Search(search);
                if(searchResult == null)
                {
                    return BadRequest("Not found by your response");
                }
                else return Ok(searchResult);
            }
        }
    }
}