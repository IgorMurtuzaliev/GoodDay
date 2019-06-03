using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int? id);
        Task Add(T item);
        Task Edit(T item);
        Task Delete(int? id);
        Task Save();
    }
}
