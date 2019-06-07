using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GoodDay.BLL.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public  string Email { get; set; }
        public string AvatarPath { get; set; }

        public UserViewModel(User user)
        {
            if (user != null)
            {
                { Id = user.Id; }
                if (!String.IsNullOrEmpty(user.Name)){
                    { Name = user.Name; }
                }
                if (!String.IsNullOrEmpty(user.Surname))
                {
                    { Surname = user.Surname; }
                }
                if (!String.IsNullOrEmpty(user.Email))
                {
                    { Email = user.Email; }
                }
                if (!String.IsNullOrEmpty(user.Phone))
                {
                    { Phone = user.Phone; }
                }
                if (user.File != null)
                {
                    { AvatarPath = user.File.Path; }
                }
                else AvatarPath = "\\Shared\\user.png";
            }
        }
    }
}
