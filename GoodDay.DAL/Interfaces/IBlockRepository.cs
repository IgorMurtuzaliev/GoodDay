using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IBlockRepository
    {
        Task Add(BlockList item);
        Task Delete(BlockList block);
        bool IsUserBlocked(User user, string friendId);
        BlockList BlockedUser(User user, string friendId);
        Task Save();
    }
}
