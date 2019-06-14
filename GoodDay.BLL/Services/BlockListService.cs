using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class BlockListService:IBlockListService
    {
        private UserManager<User> userManager;
        private IUnitOfWork unitOfWork;
        public BlockListService(UserManager<User> _userManager, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
        }
        public async Task<IEnumerable<BlockList>> GetBlockList(string id)
        {
            try
            {
                var blocks = await unitOfWork.Blocks.GetBlockedUsers(id);
                return blocks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
