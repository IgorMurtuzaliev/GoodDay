using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        public UserRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Delete(string id)
        {
            User user = await dbContext.Users.FindAsync(id);
            if (user != null)
                dbContext.Users.Remove(user);
        }

        public async Task Edit(User item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            await Save();
        }

        public async Task<User> Get(string id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }

        public bool UserExists(string id)
        {
            return dbContext.Users.Any(e => e.Id == id);
        }
        public bool PhoneExists(string phone)
        {
            return dbContext.Users.Any(e => e.Phone == phone);
        }
        public async Task<User> FindByPhone(string phone)
        {
            return await dbContext.Users.SingleAsync(c => c.Phone == phone);
        }
        
    }
}
