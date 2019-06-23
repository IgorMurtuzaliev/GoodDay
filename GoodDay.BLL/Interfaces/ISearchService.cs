using GoodDay.BLL.ViewModels;
using GoodDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodDay.BLL.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<UserViewModel>> Search(string id, string search);
    }
}
