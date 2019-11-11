using Microsoft.EntityFrameworkCore;
using MiniEshop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.DAL
{
    public class FileServiceRepository
        : IFileServiceRepository
    {
        private readonly MiniEshopDbContext _eshopDbContext;

        public FileServiceRepository(MiniEshopDbContext eshopDbContext)
        {
            _eshopDbContext = eshopDbContext;
        }

        public async Task<FileLink> DeleteFileAsync(Guid id)
        {
            var link = await GetFileLinkAsync(id);
            if (link != null)
            {
                _eshopDbContext.FileLinks.Remove(link);
                await _eshopDbContext.SaveChangesAsync();
                return link;
            }
            return null;
        }

        public Task<FileLink> GetFileLinkAsync(Guid id)
        {
            return _eshopDbContext.FileLinks.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsExistFileAsync(string dbPath)
        {
            return await _eshopDbContext.FileLinks.FirstOrDefaultAsync(x => x.FileUrl == dbPath) == null;
        }

        public async Task<FileLink> SaveFileAsync(string dbPath)
        {
            var link = new FileLink()
            {
                FileUrl = dbPath
            };
            _eshopDbContext.FileLinks.Add(link);
            await _eshopDbContext.SaveChangesAsync();
            return link;
        }
    }
}
