using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoodDay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private IContactService contactService;
        private IUserService userService;
        private IBlockListService blockListService;
        private UserManager<User> userManager;
        public ContactController(IContactService _contactService, IUserService _userService, IBlockListService _blockListService, UserManager<User> _userManager)
        {
            contactService = _contactService;
            userService = _userService;
            blockListService = _blockListService;
            userManager = _userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("add/{friendId}")]
        public async Task<IActionResult> AddContact(string friendId)
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
                    bool userHasContact = await contactService.UserHasContact(friendId, id);
                    var isUserBlocked =  blockListService.IsUserBlocked(id, friendId);
                    if (!userHasContact)
                    {
                        return BadRequest("You have contact with this user");
                    }
                    if (await isUserBlocked == true)
                    {
                        return BadRequest("Unlock this user");
                    }
                    var result = await contactService.AddContact(id, friendId);                   
                    return Ok(result);
                }
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var userId = User.Claims.First(c => c.Type == "Id").Value;
            Contact contact = await contactService.GetContact(id);
            if (contact != null)
            {
                if (contact.UserId == userId)
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
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmContact(int id)
        {
            var userId = User.Claims.First(c => c.Type == "Id").Value;
            Contact contact = await contactService.GetContact(id);
            if (contact != null)
            {
                if (contact.UserId == userId)
                {
                    if(contact.Confirmed == true)
                    {
                        return BadRequest("Contact is confirmed");
                    }
                    await contactService.ConfirmContact(id);
                    return Ok(contact);
                }
                else return BadRequest("Contact not found");
            }
            else return BadRequest("Contact not found");
        }

        [HttpGet]
        [Authorize]
        [Route("contacts")]
        public async Task<ActionResult<IEnumerable<ContactViewModel>>> GetContacts()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            var result = new List<ContactViewModel>();
            var contacts = await contactService.GetContacts(id);
            foreach(var item in contacts)
            {
                result.Add(new ContactViewModel(item));
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ChangeContactName( int? id, EditContactViewModel model)
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
                    var result = await contactService.ChangeContactName(contact, model);
                    return Ok(result);
                }
               
            }           
        }

        [HttpGet]
        [Authorize]
        [Route("icontact/{id}")]
        public async Task<IActionResult> GetContactDetails(int? id)
        {
            try
            {
            if (id == null)
            {
                return BadRequest("Id is not valid");
            }
            else
            {
                Contact contact = await contactService.GetContact(id);
                var contactVM = new ContactViewModel(contact);
                return Ok(contactVM);
            }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        
    }
}