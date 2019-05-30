using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.BLL.Interfaces
{
    public interface IUserService
    {
        bool UserExists(string id);
    }
}
