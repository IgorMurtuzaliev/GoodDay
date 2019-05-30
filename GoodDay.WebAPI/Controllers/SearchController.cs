using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Http;
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
                return BadRequest("The search string is ");
            }
            else
            {
                var searchResult = await searchService.Search(search);
                return Ok(searchResult);
            }
        }
    }
}