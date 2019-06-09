using GoodDay.Models.Entities;
using System;

namespace GoodDay.BLL.ViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public bool Blocked { get; set; }
        public string ContactName { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string FriendName { get; set; }
        public string FriendSurname { get; set; }
        public string FriendEmail { get; set; }
        public string FriendPhone { get; set; }
        public string ContactImage { get; set; }
        public ContactViewModel(Contact contact)
         {
            if (contact != null)
            {
                { Id = contact.Id; }
                { Blocked = contact.Blocked; }
               
                if (!String.IsNullOrEmpty(contact.ContactName))
                {
                    { ContactName = contact.ContactName; }
                }
                if (!String.IsNullOrEmpty(contact.FriendId))
                {
                    { FriendId = contact.FriendId; }
                }
                if (contact.Friend != null)
                {
                    { FriendName = contact.Friend.Name; }
                    { FriendSurname = contact.Friend.Surname; }
                    { FriendEmail = contact.Friend.Email; }
                    { FriendPhone = contact.Friend.Phone; }
                    if (contact.Friend.File != null)
                    {
                        { ContactImage = contact.Friend.File.Path; }
                    }
                    else ContactImage = "\\Shared\\user.png";
                }
            }
        }
    }
}
