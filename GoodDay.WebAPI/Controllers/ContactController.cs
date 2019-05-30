using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private IContactService contactService;
        private IUserService userService;
        public ContactController(IContactService _contactService, IUserService _userService)
        {
            contactService = _contactService;
            userService = _userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddContact([FromForm]string friendId)
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            if(friendId == null)
            {
                return BadRequest("Choose the user");
            }
            else
            {
                bool userExists = userService.UserExists(friendId);
                if(userExists == false)
                {
                    return BadRequest("User doesn't exist");
                }
                else
                {
                    var result = await contactService.AddContact(id, friendId);
                    return Ok(result);
                }
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteContact(int? id)
        {
            var userId = User.Claims.First(c => c.Type == "Id").Value;
            Contact contact = await contactService.GetContact(id);
            if (contact != null)
            {
                if(contact.UserId == userId)
                {
                    await contactService.DeleteContact(id);
                    return Ok(contact);
                }
                else return BadRequest("Contact not found");
            }
            else return BadRequest("Contact not found");
        }
    }
}