using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext dbContext;
        public MessageRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Add(Message item)
        {
            await dbContext.Messages.AddAsync(item);
        }

        public async Task Delete(int? id)
        {
            Message message = await dbContext.Messages.FindAsync(id);
            if (message != null)
                dbContext.Messages.Remove(message);
        }
        public async Task<Message> GetUser(int? id)
        {
            Message message = await dbContext.Messages.FindAsync(id);
            return message;

        }
    }
}
