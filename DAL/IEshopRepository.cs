using MiniEshop.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniEshop.DAL
{
    public interface IEshopRepository
    {
        Task<(List<Category> categories, Category root)> GetCategoriesAsync();
        Task<Good[]> GetGoodsAsync(Guid categoryId, int skip, int limit);
        Task<int> GetGoodCountAsync(Guid categoryId);
        Task<Good> CreateGoodAsync(Good good);
        Task<Good> UpdateGoodAsync(Good good);
        Task<Good[]> DeleteGoodAsync(List<Guid> ids);
    }
}
