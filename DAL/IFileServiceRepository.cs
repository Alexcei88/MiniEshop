using MiniEshop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.DAL
{
    public interface IFileServiceRepository
    {
        Task<FileLink> SaveFileAsync(string dbPath);

        Task<FileLink> GetFileLink(Guid id);

        Task<FileLink> DeleteFileAsync(Guid id);

        Task<bool> IsExistFileAsync(string dbPath);

    }
}
