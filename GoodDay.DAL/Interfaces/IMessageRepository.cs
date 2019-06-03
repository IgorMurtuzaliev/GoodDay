using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IMessageRepository
    {
        Task Add(Message item);
        Task Delete(int? id);
    }
}
