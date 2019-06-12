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
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ContactRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Add(Contact item)
        {
            await dbContext.Contacts.AddAsync(item);
            await Save();
        }

        public async Task Delete(int? id)
        {
            Contact contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
                dbContext.Contacts.Remove(contact);
            await Save();
        }

        public async Task Edit(Contact item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            await Save();
        }

        public async Task<Contact> Get(int? id)
        {
            return await dbContext.Contacts.FindAsync(id);
        }

        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await dbContext.Contacts.ToListAsync();
        }
        public bool IsUserInContact(string id, string friendId)
        {
            return dbContext.Contacts.Any(c => c.UserId == id && c.FriendId == friendId);

        }
        public async Task<Contact> FindContact(string id, string friendId)
        {
            return await dbContext.Contacts.SingleAsync(c => c.UserId == id && c.FriendId == friendId);
        }
        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
