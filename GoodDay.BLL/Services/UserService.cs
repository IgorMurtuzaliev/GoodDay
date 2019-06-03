using GoodDay.BLL.Interfaces;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using System;
using System.Threading.Tasks;

namespace GoodDay.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;
        public UserService( IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<User> ShowUsersProfile(string id)
        {
            try
            {
                User user = await unitOfWork.Users.Get(id);
                return user;
            }
            catch(Exception ex)
            {
                throw ex;
            }          
        }
        
        public bool UserExists(string id)
        {
            return unitOfWork.Users.UserExists(id);
        }
    }
}
