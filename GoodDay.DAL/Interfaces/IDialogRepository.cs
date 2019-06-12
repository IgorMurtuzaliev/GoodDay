using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IDialogRepository
    {
        Task<IEnumerable<Dialog>> GetAll();
        Task<Dialog> Get(int? id);
        Task Add(Dialog item);
        Task Edit(Dialog item);
        Task Delete(int? id);
        Task Save();
    }
}
