using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface IDialogService
    {
        Task<bool> HasUserDialog(string id, string friendId);
        Task<Dialog> CreateDialog(string id, string friendId);
    }
}
