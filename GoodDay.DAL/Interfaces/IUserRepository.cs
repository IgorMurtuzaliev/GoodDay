using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task Edit(User item);      
        Task Delete(string id);
        Task<User> Get(string id);     
        Task Save();
    }
}
