using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.DTO;
using GoodDay.BLL.Interfaces;
using GoodDay.Models.Entities;
using GoodDay.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize]
        [Route("contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            var result = await contactService.GetContacts(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeContactName( int? id, [FromForm]ContactViewModel model)
        {          
            if (id == null)
            {
                return BadRequest("Id is not valid");
            }
            else
            {
                Contact contact = await contactService.GetContact(id);
                var userId = User.Claims.First(c => c.Type == "Id").Value;
                if (userId!= contact.UserId )
                {
                    return BadRequest("This id is not current user id");
                }
                else
                {
                    var contactModel = new ContactDTO
                    {
                        Id = model.Id,
                        ContactName = model.ContactName
                    };
                    var result = await contactService.ChangeContactName(contactModel);
                    return Ok(result);
                }
               
            }           
        }

    }
}