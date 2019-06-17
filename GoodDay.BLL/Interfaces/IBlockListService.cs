using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IBlockListService
    {
        Task<IEnumerable<BlockList>> GetBlockList(string id);
        Task BlockUser(string id, string friendId);
        Task<bool> IsUserBlocked(string id, string friendId);
        Task UnlockUser(string id, string friendId);
    }
}
