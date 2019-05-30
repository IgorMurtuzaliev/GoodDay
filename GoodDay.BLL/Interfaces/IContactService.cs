using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IContactService
    {
        Task<Contact> AddContact(string id, string friendId);
        Task DeleteContact(int? id);
        Task<Contact> GetContact(int? id);
    }
}
