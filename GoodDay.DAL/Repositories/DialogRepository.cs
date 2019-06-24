using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private readonly ApplicationDbContext dbContext;
        public DialogRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Add(Dialog item)
        {
            await dbContext.Dialogs.AddAsync(item);
            await Save();
        }

        public async Task Delete(int? id)
        {
            Dialog dialog = await dbContext.Dialogs.FindAsync(id);
            if (dialog != null)
                dbContext.Dialogs.Remove(dialog);

        }
        public async Task Edit(Dialog item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            await Save();
        }

        public async Task<IEnumerable<Dialog>> GetAll()
        {
            return await dbContext.Dialogs.ToListAsync();
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
        public bool UserHasDialog(User user, string friendId)
        {
            return user.UsersDialogs.Any(c => c.User2Id == friendId || c.User1Id == friendId) || user.InterlocutorsDialogs.Any(c => c.User2Id == friendId || c.User1Id == friendId);
        }
        public Dialog GetDialog(User user, string friendId)
        {
            return user.UsersDialogs.Single(c => c.User2Id == friendId || c.User1Id == friendId);
        }

        public async Task<Dialog> FindDialog(int? id)
        {
            return await dbContext.Dialogs.FindAsync(id);
        }
    }
}
