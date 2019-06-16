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
    public class BlockRepository : IBlockRepository
    {

        private readonly ApplicationDbContext dbContext;
        public BlockRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Add(BlockList item)
        {
            await dbContext.BlockLists.AddAsync(item);
            await Save();
        }

        public async Task Delete(BlockList block)
        {            
            dbContext.BlockLists.Remove(block);
            await Save();
        }
        public bool IsUserBlocked(User user, string friendId)
        {
            return user.UsersBlockList.Any(c =>c.FriendId == friendId);
        }
        public BlockList BlockedUser(User user, string friendId)
        {
            return  user.UsersBlockList.Single(c => c.FriendId == friendId);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
  
    }
}
