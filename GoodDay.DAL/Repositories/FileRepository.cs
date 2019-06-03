using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodDay.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext dbContext;
        public FileRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Add(File item)
        {
            await dbContext.Files.AddAsync(item);
            await Save();
        }

        public async Task Delete(int? id)
        {
            File file = await dbContext.Files.FindAsync(id);
            if (file != null)
                dbContext.Files.Remove(file);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
